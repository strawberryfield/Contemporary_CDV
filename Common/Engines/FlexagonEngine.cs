// copyright (c) 2020-2023 Roberto Ceccarelli - Casasoft
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
            tileX = fmt.CDV_Full_v.Width / Columns;
            tileY = fmt.CDV_Full_v.Height / Rows;
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
    public override void SetJsonParams(string json) =>
        SetJsonParams(JsonSerializer.Deserialize<FlexagonParameters>(json));

    /// <summary>
    /// Sets the parameters from json deserialized object
    /// </summary>
    /// <param name="json"></param>
    public override void SetJsonParams(IParameters json) =>
        SetJsonParams((FlexagonParameters)json);

    private void SetJsonParams(FlexagonParameters p)
    {
        parameters = p;
        SetBaseJsonParams();

        Faces = p.Faces;
    }
    #endregion

    #region build
    /// <summary>
    /// Does the dirty work
    /// </summary>
    /// <returns>Images to print</returns>
    public override List<MagickImage> GetResults(bool quiet)
    {
        _ = base.GetResults(quiet);
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

        // Split images
        if (!quiet)
            Console.Write("Splitting:  ");

        MagickImage[,,] tiles = new MagickImage[Faces, Rows, Columns];
        for (int i = 0; i < Faces; i++)
        {
            for (int r = 0; r < Rows; r++)
            {
                int startY = r * tileY;
                for (int c = 0; c < Columns; c++)
                {
                    int startX = c * tileX;
                    if (!quiet)
                        Console.Write(".");

                    tiles[i, r, c] = (MagickImage)sources[i].Clone();
                    tiles[i, r, c].Crop(new MagickGeometry(startX, startY, tileX, tileY), Gravity.Northwest);
                    tiles[i, r, c].RePage();
                    tiles[i, r, c].BorderColor = BorderColor;
                    tiles[i, r, c].Border(1);
                }
            }
        }

        List<MagickImage> final = Faces switch
        {
            3 => TriTetraFlexagon(tiles),
            4 => TetraTetraFlexagon(tiles),
            6 => HexaTetraFlexagon(tiles),
            _ => new(),
        };
        return final;
    }

    private MagickImage EmptyTile()
    {
        MagickImage empty = new(FillColor, tileX, tileY);
        empty.BorderColor = BorderColor;
        empty.Border(1);
        return empty;
    }
    private List<MagickImage> TriTetraFlexagon(MagickImage[,,] tiles)
    {
        List<MagickImage> final = new();
        MagickImage empty = EmptyTile();

        // Recto
        MagickImageCollection ColStrip = new();
        MagickImageCollection RowStrip = new()
        {
            tiles[0, 0, 0],
            tiles[0, 0, 1],
            tiles[1, 0, 0],
            empty.Clone(),
            empty.Clone()
        };
        ColStrip.Add(RowStrip.AppendHorizontally());

        RowStrip = new()
        {
            empty.Clone(),
            empty.Clone(),
            tiles[1, 1, 0],
            tiles[2, 1, 0],
            tiles[2, 1, 1]
        };
        ColStrip.Add(RowStrip.AppendHorizontally());

        MagickImage print = img.FineArt10x18_o();
        print.Composite(ColStrip.AppendVertically(), Gravity.Center, 0, 0);
        final.Add(print);

        // Verso
        ColStrip = new();
        RowStrip = new()
        {
            empty.Clone(),
            empty.Clone(),
            tiles[2, 0, 0],
            tiles[2, 0, 1],
            tiles[1, 0, 1]
        };
        ColStrip.Add(RowStrip.AppendHorizontally());

        RowStrip = new()
        {
            tiles[1, 1, 1],
            tiles[0, 1, 0],
            tiles[0, 1, 1],
            empty.Clone(),
            empty.Clone()
        };
        ColStrip.Add(RowStrip.AppendHorizontally());

        print = img.FineArt10x18_o();
        print.Composite(ColStrip.AppendVertically(), Gravity.Center, 0, 0);
        final.Add(print);

        // return data
        return final;
    }

    private List<MagickImage> TetraTetraFlexagon(MagickImage[,,] tiles)
    {
        List<MagickImage> final = new();

        // Recto
        MagickImageCollection ColStrip = new();
        MagickImageCollection RowStrip = new()
        {
            tiles[0, 0, 0],
            tiles[0, 0, 1],
            tiles[1, 0, 0],
            tiles[2, 0, 1],
        };
        ColStrip.Add(RowStrip.AppendHorizontally());
        RowStrip = new()
        {
            tiles[2, 1, 0],
            tiles[1, 1, 1],
            tiles[0, 1, 0],
            tiles[0, 1, 1],
        };
        ColStrip.Add(RowStrip.AppendHorizontally());
        RowStrip = new()
        {
            tiles[0, 2, 0],
            tiles[0, 2, 1],
            tiles[1, 2, 0],
            tiles[2, 2, 1],
        };
        ColStrip.Add(RowStrip.AppendHorizontally());
        MagickImage print = img.FineArt10x15_o();
        print.Composite(ColStrip.AppendVertically(), Gravity.Center, 0, 0);
        final.Add(print);

        // Verso
        ColStrip = new();
        RowStrip = new()
        {
            tiles[3, 0, 0],
            tiles[3, 0, 1],
            tiles[2, 0, 0],
            tiles[1, 0, 1],
        };
        ColStrip.Add(RowStrip.AppendHorizontally());
        RowStrip = new()
        {
            tiles[1, 1, 0],
            tiles[2, 1, 1],
            tiles[3, 1, 0],
            tiles[3, 1, 1],
        };
        ColStrip.Add(RowStrip.AppendHorizontally());
        RowStrip = new()
        {
            tiles[3, 2, 0],
            tiles[3, 2, 1],
            tiles[2, 2, 0],
            tiles[1, 2, 1],
        };
        ColStrip.Add(RowStrip.AppendHorizontally());
        print = img.FineArt10x15_o();
        print.Composite(ColStrip.AppendVertically(), Gravity.Center, 0, 0);
        final.Add(print);

        // return data
        return final;
    }

    private List<MagickImage> HexaTetraFlexagon(MagickImage[,,] tiles)
    {
        List<MagickImage> final = new();
        MagickImage empty = EmptyTile();

        // Recto
        MagickImageCollection ColStrip = new();
        MagickImageCollection RowStrip = new()
        {
            tiles[3, 0, 0],
            tiles[4, 0, 1],
            tiles[5, 0, 0],
            tiles[5, 0, 1],
        };
        ColStrip.Add(RowStrip.AppendHorizontally());
        RowStrip = new()
        {
            tiles[3, 1, 0],
            empty.Clone(),
            empty.Clone(),
            tiles[2, 1, 1],
        };
        ColStrip.Add(RowStrip.AppendHorizontally());
        RowStrip = new()
        {
            tiles[2, 1, 0],
            empty.Clone(),
            empty.Clone(),
            tiles[3, 1, 1],
        };
        ColStrip.Add(RowStrip.AppendHorizontally());
        RowStrip = new()
        {
            tiles[5, 1, 0],
            tiles[5, 1, 1],
            tiles[4, 1, 0],
            tiles[3, 1, 1],
        };
        ColStrip.Add(RowStrip.AppendHorizontally());

        MagickImage print = img.InCartha15x20_v();
        print.Composite(ColStrip.AppendVertically(), Gravity.Center, 0, 0);
        final.Add(print);

        // Verso
        ColStrip = new();
        RowStrip = new()
        {
            tiles[4, 0, 0],
            tiles[1, 0, 1],
            tiles[0, 0, 0],
            tiles[2, 0, 1],
        };
        ColStrip.Add(RowStrip.AppendHorizontally());
        RowStrip = new()
        {
            tiles[0, 1, 0],
            empty.Clone(),
            empty.Clone(),
            tiles[1, 1, 1],
        };
        ColStrip.Add(RowStrip.AppendHorizontally());
        RowStrip = new()
        {
            tiles[1, 1, 0],
            empty.Clone(),
            empty.Clone(),
            tiles[0, 1, 1],
        };
        ColStrip.Add(RowStrip.AppendHorizontally());
        RowStrip = new()
        {
            tiles[2, 1, 0],
            tiles[0, 1, 1],
            tiles[1, 1, 0],
            tiles[4, 1, 1],
        };
        ColStrip.Add(RowStrip.AppendHorizontally());

        print = img.InCartha15x20_v();
        print.Composite(ColStrip.AppendVertically(), Gravity.Center, 0, 0);
        final.Add(print);

        // return data
        return final;
    }

    #endregion

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
                    draw.FontPointSize(tileY / 10)
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
