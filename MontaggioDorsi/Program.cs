﻿// copyright (c) 2020-2021 Roberto Ceccarelli - Casasoft
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

using Casasoft.CCDV;
using ImageMagick;
using System;

#region command line
CommandLine par = new("dorsi");
par.AddBaseOptions();
par.Usage = "[options]* inputfile+";
if (par.Parse(args)) return;
#endregion

Formats fmt = new(par.Dpi);
Images img = new(fmt);

//MagickImage final = img.InCartha20x27_o();
MagickImage final = img.Info(par.WelcomeBannerText(), $"{par.OutputName}.jpg");
MagickImageCollection imagesV = new();
MagickImageCollection imagesO = new();

// if no file specified use a blank image
if (par.FilesList.Count == 0)
{
    MagickImage dorsoOrig = img.CDV_Full_v();
    for (int i = 0; i < 4; i++) imagesV.Add(dorsoOrig.Clone());
    dorsoOrig.Rotate(90);
    for (int i = 0; i < 2; i++) imagesO.Add(dorsoOrig.Clone());
}
else
{
    int nImg = 0;
    for (int i = 0; i < 4; i++)
    {
        Console.WriteLine($"Processing: {par.FilesList[nImg]}");
        MagickImage dorso = Utils.RotateResizeAndFill(new MagickImage(par.FilesList[nImg]), fmt.CDV_Full_v, par.FillColor);
        dorso.BorderColor = par.BorderColor;
        dorso.Border(1);
        imagesV.Add(dorso);

        nImg++;
        if (nImg >= par.FilesList.Count) nImg = 0;
    }

    for (int i = 0; i < 2; i++)
    {
        Console.WriteLine($"Processing: {par.FilesList[nImg]}");
        MagickImage dorso = Utils.RotateResizeAndFill(new MagickImage(par.FilesList[nImg]), fmt.CDV_Full_o, par.FillColor);
        dorso.BorderColor = par.BorderColor;
        dorso.Border(1);
        imagesO.Add(dorso);

        nImg++;
        if (nImg >= par.FilesList.Count) nImg = 0;
    }
}

// Margini di taglio
Drawables draw = new();
draw.StrokeColor(par.BorderColor).StrokeWidth(1);
draw.Line(0, fmt.ToPixels(10), final.Width, fmt.ToPixels(10));
draw.Line(0, fmt.ToPixels(10) + fmt.CDV_Full_v.Height, final.Width, fmt.ToPixels(10) + fmt.CDV_Full_v.Height);
draw.Line(0, fmt.ToPixels(10) + fmt.CDV_Full_v.Height + fmt.CDV_Full_v.Width, final.Width, fmt.ToPixels(10) + fmt.CDV_Full_v.Height + fmt.CDV_Full_v.Width);
draw.Draw(final);

final.Composite(imagesV.AppendHorizontally(), Gravity.North, new PointD(0, fmt.ToPixels(10)));
final.Composite(imagesO.AppendHorizontally(), Gravity.North, new PointD(0, fmt.ToPixels(10) + fmt.CDV_Full_v.Height - 1));

fmt.SetImageParameters(final);
final.Write($"{par.OutputName}.jpg");
