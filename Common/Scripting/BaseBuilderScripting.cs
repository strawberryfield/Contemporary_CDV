// copyright (c) 2020-2023 Roberto Ceccarelli - Casasoft
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

namespace Casasoft.CCDV.Scripting;

internal class BaseBuilderScripting : BaseScripting
{
    public override string WrapScript(string script) => base.WrapScript(script, "BaseBuilderEngine");

    public override string Template() => base.Template() +
        OnLoad("Front") + OnLoad("Back") +
        OnLoad("Top") + OnLoad("Bottom") +
        OnLoad("Left") + OnLoad("Right");

    private string OnLoad(string name) => $@"
/// <summary> 
/// Preprocesses the loaded image for {name}
/// </summary>
/// <param name=""image"">The loaded image</param>
/// <returns>The Processed image</returns>
public MagickImage ProcessOnLoad{name}(MagickImage image) => image;
";
}
