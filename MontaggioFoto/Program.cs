// copyright (c) 2020 Roberto Ceccarelli - CasaSoft
// http://strawberryfield.altervista.org 
// 
// This file is part of CasaSoft Contemporary Carte de Visite Tools
// 
// CasaSoft CCDV Tools is free software: 
// you can redistribute it and/or modify it
// under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// CasaSoft CCDV Tools is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
// See the GNU General Public License for more details.
// 
// You should have received a copy of the GNU AGPL v.3
// along with CasaSoft CCDV Tools.  
// If not, see <http://www.gnu.org/licenses/>.

using Casasoft.CCDV;
using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

Formats fmt = new(300);
Images img = new(fmt);

List<string> files = new();
for(int p=0; p<args.Length; p++)
{
    string filename = args[p];
    files.AddRange(Directory.GetFiles(Path.GetDirectoryName(filename), Path.GetFileName(filename)).ToList());
}

for(int i = 0; i < files.Count; i++)
{
    MagickImage final = img.FineArt10x15_o();
    Console.WriteLine($"Processing: {files[i]}");
    MagickImage img1 = Utils.ResizeAndFill(new(files[i]), fmt.CDV_Internal_v);

    MagickImage img2;
    i++;
    if(i < files.Count)
    {
        Console.WriteLine($"Processing: {files[i]}");
        img2 = Utils.ResizeAndFill(new(files[i]), fmt.CDV_Internal_v);
    }
    else
    {
        img2 = img.CDV_Internal_v();
    }

    final.Composite(HalfCard(img1), Gravity.West);
    final.Composite(HalfCard(img2), Gravity.East);

    final.Write($"card{(i+1)/2,3:D3}.jpg");
}

MagickImage HalfCard(MagickImage img)
{
    img.BorderColor = MagickColors.Black;
    img.Border(1);
    MagickImage half = new(MagickColors.White, fmt.FineArt10x15_o.Width / 2, fmt.FineArt10x15_o.Height);
    half.Composite(img, Gravity.Center);
    return half;
}
