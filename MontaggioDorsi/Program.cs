// copyright (c) 2020 Roberto Ceccarelli - Casasoft
// http://strawberryfield.altervista.org 
// 
// This file is part of Casasoft Contemporary Carte de Visite Tools
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

#region command line
CommandLine par = new("dorsi");
par.AddBaseOptions();
par.Usage = "[options]* inputfile";
if (par.Parse(args)) return;
#endregion

Formats fmt = new(par.Dpi);
Images img = new(fmt);

MagickImage final = img.InCartha20x27_o();
MagickImageCollection images = new();

// if no file specified use a blank image
MagickImage dorsoOrig;
if (par.FilesList.Count > 0)
    dorsoOrig = new(par.FilesList[0]);
else
    dorsoOrig = img.CDV_Full_v();

MagickImage dorso = Utils.RotateResizeAndFill(dorsoOrig, fmt.CDV_Full_v, par.FillColor);
dorso.BorderColor = par.BorderColor;
dorso.Border(1);

for (int i = 0; i < 4; i++) images.Add(dorso.Clone());
final.Composite(images.AppendHorizontally(), Gravity.North, new PointD(0, fmt.ToPixels(10)));

images.Clear();
dorso.Rotate(90);
for (int i = 0; i < 2; i++) images.Add(dorso.Clone());
final.Composite(images.AppendHorizontally(), Gravity.North, new PointD(0, fmt.ToPixels(10) + dorso.Width - 1));

fmt.SetImageParameters(final);
final.Write($"{par.OutputName}.jpg");
