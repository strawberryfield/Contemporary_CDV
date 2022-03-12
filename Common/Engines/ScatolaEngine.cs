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

using Casasoft.CCDV.JSON;
using Casasoft.CCDV.Scripting;
using ImageMagick;

namespace Casasoft.CCDV.Engines;

/// <summary>
/// Box Builder Engine
/// </summary>
public class ScatolaEngine : BaseBuilderEngine
{
    #region constructors
    /// <summary>
    /// Constructor
    /// </summary>
    public ScatolaEngine()
    {
        parameters = new BaseBuilderParameters();
        Builder = new ScatolaBuilder();
        ScriptingClass = new BaseBuilderScripting();
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="par"></param>
    public ScatolaEngine(ICommandLine par) : base(par)
    {
        parameters = new BaseBuilderParameters();
        Builder = new ScatolaBuilder((BaseBuilderCommandLine)par, fmt);
        ScriptingClass = new BaseBuilderScripting();
        Script = parameters.Script;
    }
    #endregion

    #region build
    /// <summary>
    /// Does the dirty work
    /// </summary>
    /// <param name="quiet"></param>
    /// <returns></returns>
    public override MagickImage GetResult(bool quiet)
    {
        MagickImage output = Builder.GetOutputImage();
        ScatolaBuilder sc = (ScatolaBuilder)Builder;
        output.Composite(sc.Build(), Gravity.Center);
        sc.AddCuttingLines(output);
        
        return output;
    }
    #endregion
}
