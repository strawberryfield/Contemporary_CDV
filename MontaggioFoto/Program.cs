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
using System;

#region command line
CommandLine par = new("card");
par.AddBaseOptions();
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
    Console.WriteLine($"Processing: {par.FilesList[i]}");
    MagickImage img1 = Utils.RotateResizeAndFill(new(par.FilesList[i]), fmt.CDV_Internal_v, par.FillColor);

    MagickImage img2;
    i++;
    if (i < par.FilesList.Count)
    {
        Console.WriteLine($"Processing: {par.FilesList[i]}");
        img2 = Utils.RotateResizeAndFill(new(par.FilesList[i]), fmt.CDV_Internal_v, par.FillColor);
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

MagickImage HalfCard(MagickImage img)
{
    img.BorderColor = par.BorderColor;
    img.Border(1);
    MagickImage half = new(MagickColors.White, fmt.FineArt10x15_o.Width / 2, fmt.FineArt10x15_o.Height);
    half.Composite(img, Gravity.Center);
    return half;
}
