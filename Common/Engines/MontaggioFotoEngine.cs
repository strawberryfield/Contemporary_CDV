// copyright (c) 2020-2021 Roberto Ceccarelli - Casasoft
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

using ImageMagick;
using System;

namespace Casasoft.CCDV.Engines;

public class MontaggioFotoEngine : BaseEngine
{
    public bool FullSize { get; set; } = false;
    public bool Trim { get; set; } = false;
    public bool WithBorder { get; set; } = false;

    public MontaggioFotoEngine() : base()
    {
    }

    public MontaggioFotoEngine(CommandLine par) : base(par)
    {
    }

    public override MagickImage GetResult(bool quiet) => GetResult(quiet, 0);
    public MagickImage GetResult(bool quiet, int i)
    {
        img = new(fmt);
        MagickImage final = img.FineArt10x15_o();
        MagickImage img1 = Get(FilesList[i]);

        MagickImage img2;
        i++;
        if (i < FilesList.Count)
        {
            img2 = Get(FilesList[i]);
        }
        else
        {
            img2 = img.CDV_Internal_v();
        }

        final.Composite(HalfCard(img1), Gravity.West);
        final.Composite(HalfCard(img2), Gravity.East);
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

    private MagickImage HalfCard(MagickImage image)
    {
        image.BorderColor = BorderColor;
        image.Border(1);
        MagickImage half = new(MagickColors.White, fmt.FineArt10x15_o.Width / 2, fmt.FineArt10x15_o.Height);
        half.Composite(image, Gravity.Center);
        BaseText().Text(fmt.ToPixels(5), -half.Width + fmt.ToPixels(3), $"Source: {image.FileName}")
            .Text(fmt.ToPixels(5), -fmt.ToPixels(3), WelcomeBannerText())
            .Text(half.Height / 2, -fmt.ToPixels(3), $"Run {DateTime.Now.ToString("R")}")
            .Draw(half);
        return half;
    }

    private MagickImage Get(string filename)
    {
        Console.WriteLine($"Processing: {filename}");
        MagickImage img1 = Utils.RotateResizeAndFill(new(filename),
            FullSize ? fmt.CDV_Full_v : fmt.CDV_Internal_v,
            FillColor);
        if (Trim) img1.Trim();
        if (WithBorder)
        {
            MagickImage img2 = img.CDV_Full_v(FillColor);
            img2.Composite(img1, Gravity.Center);
            return img2;
        }
        else
            return img1;
    }

}
