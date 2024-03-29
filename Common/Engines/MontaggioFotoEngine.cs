﻿// copyright (c) 2020-2023 Roberto Ceccarelli - Casasoft
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
using System.Text.Json;

namespace Casasoft.CCDV.Engines;

/// <summary>
/// MontaggioFoto engine
/// </summary>
public class MontaggioFotoEngine : BaseEngine
{
    #region properties
    /// <summary>
    /// Set if image has full CDV size (100x64mm)
    /// </summary>
    public bool FullSize { get; set; } = false;
    /// <summary>
    /// Set if white border is removed
    /// </summary>
    public bool Trim { get; set; } = false;
    /// <summary>
    /// Set if a border to full CDV size (100x64mm) is added
    /// </summary>
    public bool WithBorder { get; set; } = false;
    /// <summary>
    /// Blank border around the image
    /// </summary>
    public int Padding { get; set; } = 0;
    /// <summary>
    /// Canvas gravity
    /// </summary>
    public Gravity CanvasGravity { get; set; } = Gravity.Center;

    #endregion

    #region constructors
    /// <summary>
    /// Constructor
    /// </summary>
    public MontaggioFotoEngine() : base()
    {
        parameters = new MontaggioFotoParameters();
        ScriptingClass = new MontaggioFotoScripting();
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="par"></param>
    public MontaggioFotoEngine(CommandLine par) : base(par)
    {
        parameters = new MontaggioFotoParameters();
        MontaggioFotoCommandLine p = (MontaggioFotoCommandLine)par;
        FullSize = p.FullSize ? true : FullSize;
        Trim = p.Trim ? true : Trim;
        WithBorder = p.WithBorder ? true : WithBorder;
        Padding = p.Padding;
        CanvasGravity = p.CanvasGravity;
        ScriptingClass = new MontaggioFotoScripting();
        Script = p.Script;
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
        MontaggioFotoParameters p = (MontaggioFotoParameters)parameters;
        p.FullSize = FullSize;
        p.WithBorder = WithBorder;
        p.Trim = Trim;
        p.Padding = Padding;
        p.CanvasGravity = CanvasGravity;
        return JsonSerializer.Serialize(p);
    }

    /// <summary>
    /// Sets the parameters from json formatted string
    /// </summary>
    /// <param name="json"></param>
    public override void SetJsonParams(string json) =>
        SetJsonParams(JsonSerializer.Deserialize<MontaggioFotoParameters>(json));

    /// <summary>
    /// Sets the parameters from json deserialized object
    /// </summary>
    /// <param name="json"></param>
    public override void SetJsonParams(IParameters json) =>
        SetJsonParams((MontaggioFotoParameters)json);

    private void SetJsonParams(MontaggioFotoParameters p)
    {
        parameters = p;
        SetBaseJsonParams();
        FullSize = p.FullSize;
        WithBorder = p.WithBorder;
        Trim = p.Trim;
        Padding = p.Padding;
        Script = p.Script;
        CanvasGravity = p.CanvasGravity;
    }
    #endregion

    #region build
    /// <summary>
    /// Does the dirty work
    /// </summary>
    /// <param name="quiet"></param>
    /// <returns></returns>
    public override MagickImage GetResult(bool quiet) => GetResult(quiet, 0);
    /// <summary>
    /// Does the dirty work
    /// </summary>
    /// <param name="quiet"></param>
    /// <param name="i"></param>
    /// <returns></returns>
    public MagickImage GetResult(bool quiet, int i)
    {
        _ = base.GetResult(quiet);

        MagickImage final = GetOutputPaper(PaperFormat);

        MagickImage img1 = Get(FilesList[i], quiet);
        MagickImage img2;
        string name1 = FilesList[i];
        string name2 = string.Empty;
        i++;
        if (i < FilesList.Count)
        {
            img2 = Get(FilesList[i], quiet);
            name2 = FilesList[i];
        }
        else
        {
            img2 = img.CDV_Internal_v();
        }

        final.Composite(HalfCard(img1, name1), Gravity.West);
        final.Composite(HalfCard(img2, name2), Gravity.East);
        return final;
    }

    private Drawables BaseText()
    {
        Drawables d = new Drawables();
        d.FontPointSize(fmt.ToPixels(3) / 2)
            .Font("Arial")
            .FillColor(MagickColors.Black)
            .TextAlignment(TextAlignment.Left)
            .Gravity(Gravity.Northwest)
            .Rotation(90);
        return d;
    }

    private MagickImage HalfCard(MagickImage image, string filename)
    {
        image.BorderColor = BorderColor;
        image.Border(1);
        MagickImage half = new(MagickColors.White, fmt.FineArt10x15_o.Width / 2, fmt.FineArt10x15_o.Height);
        half.Composite(image, Gravity.Center);
        BaseText().Text(fmt.ToPixels(5), -half.Width + fmt.ToPixels(3), $"Source: {filename}")
            .Text(fmt.ToPixels(5), -fmt.ToPixels(3), WelcomeBannerText())
            .Text(half.Height / 2, -fmt.ToPixels(3), $"Run {DateTime.Now:R}")
            .Draw(half);
        return half;
    }

    private MagickImage Get(string filename, bool quiet)
    {
        if (!quiet) Console.WriteLine($"Processing: {filename}");
        MagickImage image = Utils.GetImage(filename, fmt.CDV_Internal_v, CanvasGravity);

        if (ScriptInstance is not null)
        {
            var i = Compiler.Run(ScriptInstance, "ProcessOnLoad", new object[] { image });
            if (i is not null)
            {
                image = (MagickImage)i;
            }
        }

        MagickImage img1 = Utils.RotateResizeAndFill(image,
            FullSize ? fmt.CDV_Full_v : fmt.CDV_Internal_v,
            FillColor, CanvasGravity);

        if (Trim) img1.Trim();

        if (WithBorder)
        {
            int offset = Math.Min(fmt.CDV_Full_v.Width - img1.Width, fmt.CDV_Full_v.Height - img1.Height) / 2;
            int offsetX = 0;
            int offsetY = 0;
            switch (CanvasGravity)
            {
                case Gravity.Northwest:
                case Gravity.Northeast:
                case Gravity.Southwest:
                case Gravity.Southeast:
                    offsetX = offset;
                    offsetY = offset;
                    break;
                case Gravity.South:
                case Gravity.North:
                    offsetY = offset;
                    break;
                case Gravity.West:
                case Gravity.East:
                    offsetX = offset;
                    break;
                default:
                    break;
            }
            MagickImage img2 = img.CDV_Full_v(FillColor);
            img2.Composite(img1, CanvasGravity, offsetX, offsetY);
            return img2;
        }

        if (Padding > 0)
        {
            MagickGeometry size = fmt.CDV_Internal_v;
            if (Trim)
            {
                size = new MagickGeometry(img1.Width, img1.Height);
            }
            MagickImage img2 = img.Padded(FillColor, size, Padding);
            img2.Composite(img1, CanvasGravity);
            return img2;
        }

        return img1;
    }
    #endregion
}
