﻿// copyright (c) 2020-2022 Roberto Ceccarelli - Casasoft
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
/// Montaggio dorsi engine
/// </summary>
public class MontaggioDorsiEngine : BaseEngine
{
    /// <summary>
    /// Output paper size
    /// </summary>
    public PaperFormats PaperFormat { get; set; }

    #region constructors
    /// <summary>
    /// Constructor
    /// </summary>
    public MontaggioDorsiEngine() : base()
    {
        parameters = new MontaggioDorsiParameters();
        ScriptingClass = new MontaggioDorsiScripting();
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="par"></param>
    public MontaggioDorsiEngine(CommandLine par) : base(par)
    {
        parameters = new MontaggioDorsiParameters();
        MontaggioDorsiCommandLine p = (MontaggioDorsiCommandLine)par;
        PaperFormat = p.PaperFormat;
        ScriptingClass = new MontaggioDorsiScripting();
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
        MontaggioDorsiParameters p = (MontaggioDorsiParameters)parameters;
        p.PaperFormat = PaperFormat;
        return JsonSerializer.Serialize(p);
    }

    /// <summary>
    /// Sets the parameters from json formatted string
    /// </summary>
    /// <param name="json"></param>
    public override void SetJsonParams(string json)
    {
        MontaggioDorsiParameters p = JsonSerializer.Deserialize<MontaggioDorsiParameters>(json);
        parameters = p;
        SetBaseJsonParams();
        PaperFormat = p.PaperFormat;
    }
    #endregion

    #region build
    /// <summary>
    /// Does the dirty work
    /// </summary>
    /// <param name="quiet"></param>
    /// <returns></returns>
    public override MagickImage GetResult(bool quiet)
    {
        if (CustomCode != null)
        {
            ScriptInstance = Compiler.New(CustomCode, this);
        }

        img = new(fmt);

        MagickImage final = PaperFormat == PaperFormats.Medium ? img.InCartha15x20_o() : img.InCartha20x27_o();
        MagickImageCollection imagesV = new();
        MagickImageCollection imagesO = new();

        // if no file specified use a blank image
        if (FilesList.Count == 0)
        {
            MagickImage dorsoOrig = img.CDV_Full_v();
            if (PaperFormat == PaperFormats.Medium)
            {
                for (int i = 0; i < 3; i++) imagesV.Add(dorsoOrig.Clone());
            }
            else
            {
                for (int i = 0; i < 4; i++) imagesV.Add(dorsoOrig.Clone());
                dorsoOrig.Rotate(90);
                for (int i = 0; i < 2; i++) imagesO.Add(dorsoOrig.Clone());
            }
        }
        else
        {
            int nImg = 0;
            if (PaperFormat == PaperFormats.Medium)
            {
                _ = LoadImages(3, nImg, imagesV, quiet, fmt.CDV_Full_v);
            }
            else
            {
                nImg = LoadImages(4, nImg, imagesV, quiet, fmt.CDV_Full_v);
                _ = LoadImages(2, nImg, imagesO, quiet, fmt.CDV_Full_o);
            }
        }

        // Margini di taglio
        Drawables draw = new();
        draw.StrokeColor(BorderColor).StrokeWidth(1);
        if (PaperFormat == PaperFormats.Medium)
        {
            int top = (final.Height - fmt.CDV_Full_v.Height) / 2;
            int left = (final.Width - fmt.CDV_Full_v.Width * 3) / 2;

            Utils.HLine(draw, top, final.Width);
            Utils.HLine(draw, final.Height - top, final.Width);
            Utils.VLine(draw, left, final.Height);
            Utils.VLine(draw, final.Width - left, final.Height);
        }
        else
        {
            int h = fmt.ToPixels(10);
            Utils.HLine(draw, h, final.Width);
            h += fmt.CDV_Full_v.Height;
            Utils.HLine(draw, h, final.Width);
            h += fmt.CDV_Full_v.Width;
            Utils.HLine(draw, h, final.Width);
        }
        draw.Draw(final);

        if (PaperFormat == PaperFormats.Medium)
        {
            final.Composite(imagesV.AppendHorizontally(), Gravity.Center, new PointD(0, 0));
        }
        else
        {
            final.Composite(imagesV.AppendHorizontally(), Gravity.North, new PointD(0, fmt.ToPixels(10)));
            final.Composite(imagesO.AppendHorizontally(), Gravity.North,
                new PointD(0, fmt.ToPixels(10) + fmt.CDV_Full_v.Height - 1));
        }

        return final;
    }

    #region functions
    private int LoadImages(int n, int counter, MagickImageCollection dest, bool quiet, MagickGeometry orientation)
    {
        int nImg = counter;
        for (int i = 0; i < n; i++)
        {
            if (!quiet) Console.WriteLine($"Processing: {FilesList[nImg]}");

            MagickImage image = new(FilesList[nImg]);

            if (ScriptInstance != null)
            {
                image = (MagickImage)Compiler.Run(ScriptInstance, "ProcessOnLoad", new object[] { image });
            }

            MagickImage dorso = Utils.RotateResizeAndFill(image, orientation, FillColor);
            dorso.BorderColor = BorderColor;
            dorso.Border(1);
            dest.Add(dorso);

            nImg++;
            if (nImg >= FilesList.Count) nImg = 0;
        }
        return nImg;
    }

    #endregion

    #endregion
}
