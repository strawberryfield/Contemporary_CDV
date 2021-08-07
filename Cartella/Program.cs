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

#region command line
BaseBuilderCommandLine par = new("folder");
par.Usage = "[options]*";
if (par.Parse(args)) return;
#endregion

#region main
Formats fmt = new(par.Dpi);
Images img = new(fmt);
//MagickImage output = img.InCartha20x27_o();
MagickImage output = img.Info(par.WelcomeBannerText(), $"{par.OutputName}.jpg");
FolderBuilder sc = new(par, fmt);
MagickImage result = sc.Build();

// linee guida per il ritaglio
Drawables draw = new();
draw.StrokeColor(MagickColors.Black).StrokeWidth(1);
int yTop = output.Height / 2 - result.Height / 2;
draw.Line(0, yTop - 1, output.Width, yTop - 1);
draw.Line(0, yTop + result.Height, output.Width, yTop + result.Height);
int xleft = output.Width / 2 - result.Width / 2;
draw.Line(xleft - 1, 0, xleft - 1, yTop + result.Height);
draw.Line(xleft + result.Width, 0, xleft + result.Width, output.Height);
draw.Draw(output);

output.Composite(result, Gravity.Center);
fmt.SetImageParameters(output);
output.Write($"{par.OutputName}.jpg");
#endregion
