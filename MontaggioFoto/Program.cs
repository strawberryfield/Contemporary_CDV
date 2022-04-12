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
string desc = "Assembling two images over a 10x15 cm paper";
string longDesc = @"This program gathers two images on a 10x15cm surface 
that you can print on a paper of ypur choice.";
#endregion

#region command line
MontaggioFotoCommandLine par = new("card", desc);
par.Usage = "[options]* inputfile+";
par.LongDesc = longDesc;
if (par.Parse(args)) return;

par.ExpandWildcards();
#endregion

#region main
MontaggioFotoEngine engine = new(par);
for (int i = 0; i < par.FilesList.Count; i += 2)
{
    MagickImage final = engine.GetResult(false, i);
    engine.fmt.SetImageParameters(final, par.Extension);
    final.Write($"{par.OutputName}-{i / 2 + 1,3:D3}.{par.Extension}");
}
#endregion