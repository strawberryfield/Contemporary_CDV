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
using System.Drawing;
using System.Text.Json;

namespace Casasoft.CCDV.Engines;

/// <summary>
/// Generates a Tetra Flexagon with 3, 4 or 6 faces
/// </summary>
public class FlexagonEngine : BaseEngine
{
    #region properties
    /// <summary>
    /// Number of faces of the flexagons
    /// </summary>
    /// <remarks>
    /// Valid numbers are 3, 4 or 6
    /// </remarks>
    public int Faces
    {
        get => _faces;
        set
        {
            _faces = value;
            Rows = Faces == 4 ? 3 : 2;
            Columns = 2;
            tileX = fmt.CDV_Full_v.X / Columns;
            tileY = fmt.CDV_Full_v.Y / Rows;
        }
    }

    /// <summary>
    /// True if samples images will be created
    /// </summary>
    public bool useSampleImages { get; set; }
    #endregion

    #region private properties
    int _faces;

    int Rows;
    int Columns;
    int tileX;
    int tileY;
    #endregion

    #region constructors
    /// <summary>
    /// Constructor
    /// </summary>
    public FlexagonEngine()
    {
        parameters = new FlexagonParameters();
        ScriptingClass = new FlexagonScripting();
        Faces = 3;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="par">Command line options</param>
    public FlexagonEngine(ICommandLine par) : base(par)
    {
        FlexagonCommandLine p = (FlexagonCommandLine)par;
        Faces = p.Faces;
        useSampleImages = p.useSampleImages;

        ScriptingClass = new FlexagonScripting();
        Script = p.Script;
        parameters = new FlexagonParameters();
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
        FlexagonParameters p = (FlexagonParameters)parameters;
        p.Faces = Faces;

        return JsonSerializer.Serialize(p);
    }

    /// <summary>
    /// Sets the parameters from json formatted string
    /// </summary>
    /// <param name="json"></param>
    public override void SetJsonParams(string json)
    {
        FlexagonParameters p = JsonSerializer.Deserialize<FlexagonParameters>(json);
        parameters = p;
        SetBaseJsonParams();

        Faces = p.Faces;
    }
    #endregion

    /// <summary>
    /// Does the dirty work
    /// </summary>
    /// <returns>Images to print</returns>
    public List<MagickImage> GetResults(bool quiet)
    {
        _ = GetResult(quiet);
        List<MagickImage> final = new();

        MagickImage[] sources = new MagickImage[Faces];

        // Load images
        if (useSampleImages)
        {
            sources = Samples();
        }
        else
        {
            for (int i = 0; i < Faces; i++)
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

                sources[i] = Utils.RotateResizeAndFill(image, fmt.CDV_Full_v, FillColor);
            }
        }

        return final;
    }

    #region samples
    private MagickImage[] Samples()
    {
        MagickImage[] ret = new MagickImage[Faces];

        for (int i = 0; i < Faces; i++)
        {
            ret[i] = new MagickImage();
            MagickImageCollection RowStrips = new();
            for (int row = 0; row < Rows; row++)
            {
                MagickImageCollection ColStrip = new();
                for (int col = 0; col < Columns; col++)
                {
                    MagickImage tile = new(MagickColors.White, tileX, tileY);
                    Utils.CenteredText(((char)('A' + i)).ToString(), tileY / 2, tileX, tileY).Draw(tile);
                    Drawables draw = new();
                    draw.FontPointSize(tileY / 5)
                        .Font("Arial")
                        .FillColor(MagickColors.Black)
                        .Gravity(Gravity.South)
                        .Text(0, tileY / 10, $"r={row + 1},c={col + 1}")
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
