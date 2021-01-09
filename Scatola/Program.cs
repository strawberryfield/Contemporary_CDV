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
BaseBuilderCommandLine par = new("box");
par.Usage = "[options]*";
if (par.Parse(args)) return;
#endregion

#region main
Formats fmt = new(par.Dpi);
Images img = new(fmt);
MagickImage output = img.InCartha20x27_o();
ScatolaBuilder sc = new(par.thickness, fmt, par.FillColor, par.BorderColor);

// only for test 
if(par.useSampleImages) sc.CreateTestImages();

sc.SetFrontImage(par.frontImage);
sc.SetBackImage(par.backImage, par.isHorizontal);
sc.SetTopImage(par.topImage);
sc.SetBottomImage(par.bottomImage);
sc.SetLeftImage(par.leftImage);
sc.SetRightImage(par.rightImage);

output.Composite(sc.Build(), Gravity.Center);
fmt.SetImageParameters(output);
output.Write($"{par.OutputName}.jpg");
#endregion

