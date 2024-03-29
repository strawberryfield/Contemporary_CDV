﻿// copyright (c) 2020-2023 Roberto Ceccarelli - Casasoft
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
using Casasoft.CCDV.Engines;
using ImageMagick;

#region texts
string desc = "Assembling six images over a 20x27 cm paper";
string longDesc = @"This program gathers six images on a 20x27cm surface 
that I print on a cardboard coated only on the side of the image.";
#endregion

#region command line
MontaggioDorsiCommandLine par = new("dorsi", desc)
{
    Usage = "[options]* inputfile+",
    LongDesc = longDesc
};
if (par.Parse(args)) return;

par.ExpandWildcards();
#endregion

MontaggioDorsiEngine engine = new(par);
MagickImage final = engine.GetResult();
engine.fmt.SetImageParameters(final, par.Extension);
final.Write($"{par.OutputName}.{par.Extension}");
