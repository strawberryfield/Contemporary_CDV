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

using Casasoft.CCDV;
using ImageMagick;
using Mono.Options;
using System;

#region command line
MontaggioFotoCommandLine par = new("card");
par.Usage = "[options]* inputfile+";
if (par.Parse(args)) return;

par.ExpandWildcards();
#endregion

#region main
Formats fmt = new(par.Dpi);
Images img = new(fmt);

for (int i = 0; i < par.FilesList.Count; i++)
{
    MagickImage final = img.FineArt10x15_o();
    MagickImage img1 = Get(par.FilesList[i]);

    MagickImage img2;
    i++;
    if (i < par.FilesList.Count)
    {
        img2 = Get(par.FilesList[i]);
    }
    else
    {
        img2 = img.CDV_Internal_v();
    }

    final.Composite(HalfCard(img1), Gravity.West);
    final.Composite(HalfCard(img2), Gravity.East);

    fmt.SetImageParameters(final);
    final.Write($"{par.OutputName}-{(i + 1) / 2,3:D3}.jpg");
}
#endregion

Drawables BaseText()
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

MagickImage HalfCard(MagickImage image)
{
    image.BorderColor = par.BorderColor;
    image.Border(1);
    MagickImage half = new(MagickColors.White, fmt.FineArt10x15_o.Width / 2, fmt.FineArt10x15_o.Height);
    half.Composite(image, Gravity.Center);
    BaseText().Text(fmt.ToPixels(5), -half.Width + fmt.ToPixels(3), $"Source: {image.FileName}")
        .Text(fmt.ToPixels(5), -fmt.ToPixels(3), par.WelcomeBannerText())
        .Text(half.Height/2, -fmt.ToPixels(3), $"Run {DateTime.Now.ToString("R")}")
        .Draw(half);
    return half;
}

MagickImage Get(string filename)
{
    Console.WriteLine($"Processing: {filename}");
    MagickImage img1 = Utils.RotateResizeAndFill(new(filename),
        par.FullSize ? fmt.CDV_Full_v : fmt.CDV_Internal_v,
        par.FillColor);
    if (par.Trim) img1.Trim();
    if (par.WithBorder)
    {
        MagickImage img2 = img.CDV_Full_v(par.FillColor);
        img2.Composite(img1, Gravity.Center);
        return img2;
    }
    else
        return img1;
}

#region command line
internal class MontaggioFotoCommandLine : CommandLine
{
    public bool FullSize { get; set; }
    public bool WithBorder { get; set; }
    public bool Trim { get; set; }

    public MontaggioFotoCommandLine(string outputname) :
    this(ExeName(), outputname)
    { }
    public MontaggioFotoCommandLine(string exename, string outputname) :
        base(exename, outputname)
    {
        FullSize = false;
        WithBorder = false;
        Trim = false;

        Options = new OptionSet
            {
                { "fullsize", "resize image to full format", o => FullSize = o != null },
                { "withborder", "include border to full format", o => WithBorder = o != null },
                { "trim", "trim white space", o => Trim = o != null },
            };
        AddBaseOptions();
    }
}
#endregion
