// copyright (c) 2020-2022 Roberto Ceccarelli - Casasoft
// http://strawberryfield.altervista.org 
// 
// This file is part of Casasoft Contemporary Carte de Visite Tools
// https://github.com/strawberryfield/Contemporary_CDV
// 
// Casasoft CCDV Tools is free software: 
// you can redistribute it and/or modify it
// under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Casasoft CCDV Tools is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
// See the GNU General Public License for more details.
// 
// You should have received a copy of the GNU AGPL v.3
// along with Casasoft CCDV Tools.  
// If not, see <http://www.gnu.org/licenses/>.

using Casasoft.CCDV.JSON;
using Casasoft.CCDV.Scripting;
using ImageMagick;
using System;
using System.Collections.Generic;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace Casasoft.CCDV.Engines;

/// <summary>
/// Creates a matrix of cubes with 6 images
/// </summary>
public class CubettiEngine : BaseEngine
{
    #region properties
    /// <summary>
    /// Number of rows to generate
    /// </summary>
    public int Rows { get; set; } = 2;

    /// <summary>
    /// Number of Columns to generate
    /// </summary>
    public int Columns { get; set; } = 3;

    /// <summary>
    /// Size of any cube (mm)
    /// </summary>
    public int Size { get; set; } = 50;

    /// <summary>
    /// True if samples images will be created
    /// </summary>
    public bool useSampleImages { get; set; }
    #endregion

    #region constructors
    /// <summary>
    /// Constructor
    /// </summary>
    public CubettiEngine()
    {
        parameters = new CubettiParameters();
        ScriptingClass = new CubettiScripting();
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="par">Command line options</param>
    public CubettiEngine(ICommandLine par) : base(par)
    {
        CubettiCommandLine p = (CubettiCommandLine)par;
        Rows = p.Rows;
        Columns = p.Columns;
        Size = p.Size;

        PaperFormat = p.PaperFormat;
        useSampleImages = p.useSampleImages;
        ScriptingClass = new CubettiScripting();
        Script = p.Script;
        parameters = new CubettiParameters();
        parameters.OutputName = p.OutputName;
    }
    #endregion

    #region json
    /// <summary>
    /// Returns the parameters in json format
    /// </summary>
    /// <returns></returns>
    public override string GetJsonParams()
    {
        GetBaseJsonParams();
        CubettiParameters p = (CubettiParameters)parameters;
        p.Rows = Rows;
        p.Columns = Columns;
        p.Size = Size;
        p.PaperFormat = PaperFormat;

        return JsonSerializer.Serialize(p);
    }

    /// <summary>
    /// Sets the parameters from json formatted string
    /// </summary>
    /// <param name="json"></param>
    public override void SetJsonParams(string json)
    {
        CubettiParameters p = JsonSerializer.Deserialize<CubettiParameters>(json);
        parameters = p;
        SetBaseJsonParams();

        Rows = p.Rows;
        Columns = p.Columns;
        Size = p.Size;
        PaperFormat = p.PaperFormat;
    }
    #endregion

    #region build
    private int faceSize;

    /// <summary>
    /// Does the dirty work
    /// </summary>
    /// <returns>Image to print</returns>
    public List<MagickImage> GetResults(bool quiet)
    {
        _ = GetResult(quiet);
        List<MagickImage> final = new();

        // Compute grid size
        int sizeX = fmt.ToPixels(Columns * Size);
        int sizeY = fmt.ToPixels(Rows * Size);
        MagickGeometry sourceFormat = new(sizeX, sizeY);
        faceSize = fmt.ToPixels(Size);

        MagickImage[] sources = new MagickImage[6];

        // Load images
        if (useSampleImages)
        {
            sources = Samples();
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                if (!quiet)
                    Console.WriteLine($"Reading: {FilesList[i]}");

                MagickImage image = new(FilesList[i]);
                if (ScriptInstance is not null)
                {
                    var img = Compiler.Run(ScriptInstance, "ProcessOnLoad", new object[] { image });
                    if (img is not null)
                    {
                        image = (MagickImage)img;
                    }
                }

                sources[i] = Utils.RotateResizeAndFill(image, sourceFormat, FillColor);
            }
        }

        // Split images
        if (!quiet)
            Console.Write("Splitting:  ");

        List<MagickImage[]> faces = new();
        for (int row = 0; row < Rows; row++)
        {
            int startY = fmt.ToPixels(row * Size);
            for (int col = 0; col < Columns; col++)
            {
                MagickImage[] face = new MagickImage[6];
                int startX = fmt.ToPixels(col * Size);

                for (int i = 0; i < 6; i++)
                {
                    if (!quiet)
                        Console.Write(".");

                    face[i] = (MagickImage)sources[i].Clone();
                    face[i].Crop(new MagickGeometry(startX, startY, faceSize, faceSize), Gravity.Northwest);
                    face[i].RePage();
                    face[i].BorderColor = BorderColor;
                    face[i].Border(1);
                }
                faces.Add(face);
            }
        }
        if (!quiet)
            Console.WriteLine();

        // Clips
        BuildClips(20);

        // Cubes assembling
        if (!quiet)
            Console.Write("Generating: ");

        int cnt = 0;
        foreach (MagickImage[] face in faces)
        {
            if (!quiet)
                Console.Write("#");

            cnt++;
            MagickImageCollection img2 = new();
            img2.Add(TopClipStrip);
            img2.Add(AssemblyPartialCube(face, 0));
            img2.Add(BottomClipStrip);
            img2.Add(AssemblyPartialCube(face, 3));

            MagickImage image = GetOutputPaper(PaperFormat);
            image.Composite(img2.AppendVertically(), Gravity.Center, 0, 0);
            AddCuttingLines(image, cnt);
            fmt.SetImageParameters(image, parameters.Extension);
            final.Add(image);
        }

        if (!quiet)
            Console.WriteLine();
        return final;
    }

    private MagickImage AssemblyPartialCube(MagickImage[] face, int start)
    {
        MagickImageCollection img3 = new();
        img3.Add(LeftClip);
        for (int i = 0; i < 3; i++)
        {
            img3.Add(face[i + start]);
        }
        img3.Add(RightClip);
        return (MagickImage)img3.AppendHorizontally();
    }

    /// <summary>
    /// Creates lines for cut
    /// </summary>
    /// <param name="img"></param>
    /// <param name="i">Current image number</param>
    public void AddCuttingLines(MagickImage img, int i)
    {
        MagickImage trim = (MagickImage)img.Clone();
        trim.Trim();
        int h_offset = (img.Width - trim.Width) / 2;
        int v_offset = (img.Height - trim.Height) / 2;

        Drawables d = new();
        d.StrokeColor(BorderColor).StrokeWidth(1);
        d.Line(0, v_offset, img.Width, v_offset);
        d.Line(0, img.Height - v_offset, img.Width, img.Height - v_offset);
        d.Line(0, img.Height - v_offset - faceSize, img.Width, img.Height - v_offset - faceSize);
        d.Line(h_offset, 0, h_offset, img.Height - v_offset);
        d.Line(img.Width - h_offset, 0, img.Width - h_offset, img.Height);
        d.Draw(img);

        d = new();
        d.FontPointSize(fmt.ToPixels(3))
            .Font("Arial")
            .FillColor(MagickColors.Black)
            .Gravity(Gravity.Northwest)
            .Text(h_offset + fmt.ToPixels(5), fmt.ToPixels(2), $"{parameters.OutputName} {i} of {Rows * Columns}")
            .Draw(img);

        d = new();
        d.FontPointSize(fmt.ToPixels(2))
            .Font("Arial")
            .FillColor(MagickColors.Black)
            .Gravity(Gravity.Northwest)
            .Text(fmt.ToPixels(5), img.Height - v_offset, $"{WelcomeBannerText()}")
            .Draw(img);
    }

    #region clips
    private int clipSize;
    private MagickImage TopClipStrip;
    private MagickImage BottomClipStrip;
    private MagickImage RightClip;
    private MagickImage LeftClip;

    private void BuildClips(int size)
    {
        MagickImage BottomClip;
        MagickImage TopClip;
        MagickImage NoseClip;
        MagickImage EmptyClip;

        clipSize = fmt.ToPixels(size);
        RightClip = new(MagickColors.White, clipSize, faceSize);

        Drawables d = new();
        d.StrokeColor(BorderColor)
            .StrokeWidth(1)
            .Line(0, 0, clipSize - 1, clipSize / 2)
            .Line(clipSize - 1, clipSize / 2, clipSize - 1, faceSize - clipSize / 2)
            .Line(clipSize - 1, faceSize - clipSize / 2, 0, faceSize)
            .Draw(RightClip);

        LeftClip = (MagickImage)RightClip.Clone();
        LeftClip.Rotate(180);
        BottomClip = (MagickImage)RightClip.Clone();
        BottomClip.Rotate(90);
        TopClip = (MagickImage)LeftClip.Clone();
        TopClip.Rotate(90);

        EmptyClip = new(MagickColors.White, faceSize, clipSize);
        MagickImage ClipFiller = new(MagickColors.White, clipSize, clipSize);

        NoseClip = (MagickImage)EmptyClip.Clone();
        int noseLeft = faceSize / 2 - clipSize / 2;
        int noseRight = faceSize / 2 + clipSize / 2;
        d = new();
        d.StrokeColor(BorderColor)
            .StrokeWidth(1)
            .Line(noseLeft, 0, noseLeft, clipSize - 1)
            .Line(noseRight, 0, noseRight, clipSize - 1)
            .Line(noseLeft, clipSize - 1, noseRight, clipSize - 1)
            .Draw(NoseClip);

        MagickImageCollection strip = new();
        strip.Add(ClipFiller);
        strip.Add(BottomClip);
        strip.Add(NoseClip);
        strip.Add(BottomClip.Clone());
        BottomClipStrip = (MagickImage)strip.AppendHorizontally();

        strip = new();
        strip.Add(ClipFiller);
        strip.Add(TopClip);
        strip.Add(EmptyClip);
        strip.Add(TopClip.Clone());
        TopClipStrip = (MagickImage)strip.AppendHorizontally();
    }
    #endregion

    #endregion

    #region samples
    private MagickImage[] Samples()
    {
        MagickImage[] ret = new MagickImage[6];

        for (int i = 0; i < 6; i++)
        {
            ret[i] = new MagickImage();
            MagickImageCollection RowStrips = new();
            for (int row = 0; row < Rows; row++)
            {
                MagickImageCollection ColStrip = new();
                for (int col = 0; col < Columns; col++)
                {
                    MagickImage tile = new(MagickColors.White, faceSize, faceSize);
                    Utils.CenteredText(((char)('A' + i)).ToString(), faceSize / 2, faceSize, faceSize).Draw(tile);
                    Drawables draw = new();
                    draw.FontPointSize(faceSize / 5)
                        .Font("Arial")
                        .FillColor(MagickColors.Black)
                        .Gravity(Gravity.South)
                        .Text(0, faceSize / 10, $"r={row + 1},c={col + 1}")
                        .Draw(tile);

                    ColStrip.Add(tile);
                }
                RowStrips.Add(ColStrip.AppendHorizontally());
            }
            ret[i] = (MagickImage)RowStrips.AppendVertically();
        }
        return ret;
    }
    #endregion
}
