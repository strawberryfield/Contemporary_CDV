// copyright (c) 2020-2022 Roberto Ceccarelli - Casasoft
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
string desc = "Folder builder using a 20x27 cm paper";
string longDesc = @"This program allows you to create an exploded view of a folder case,
without glued parts, for Carte de Visite.  
It has dimensions 105x70mm with a thickness that can be changed by a parameter on the command line.";
#endregion

#region command line
BaseBuilderCommandLine par = new("folder", desc);
par.Usage = "[options]*";
par.LongDesc = longDesc;
if (par.Parse(args)) return;
#endregion

#region main
FolderEngine engine = new(par);
MagickImage output = engine.GetResult();
engine.SetImageParameters(output);

output.Write($"{par.OutputName}.{par.Extension}");
#endregion
