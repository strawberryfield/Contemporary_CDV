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

#region texts
string desc = "Creates a cube matrix with 6 photos.";
string longDesc = @"This program allows you to create a cube matrix with 6 photos.   
Each face of the cube has a portion of one of the six images.";
#endregion

#region command line
CubettiCommandLine par = new("cubes", desc)
{
    Usage = "[options]* inputfiles",
    LongDesc = longDesc
};
if (par.Parse(args)) return;
if (par.FilesList.Count < 6)
{
    Console.Error.WriteLine("Not enough images.");
    return;
}
#endregion

CubettiEngine engine = new(par);
Utils.WriteImages(engine.GetResults(false), par);