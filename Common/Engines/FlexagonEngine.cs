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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.Json;

namespace Casasoft.CCDV.Engines;

/// <summary>
/// Generates a Tetra Flexagon with 3, 4 or 6 faces
/// </summary>
public class FlexagonEngine : BaseEngine
{
    /// <summary>
    /// Number of faces of the flexagons
    /// </summary>
    /// <remarks>
    /// Valid numbers are 3, 4 or 6
    /// </remarks>
    public int Faces { get; set; } = 3;

    #region constructors
    /// <summary>
    /// Constructor
    /// </summary>
    public FlexagonEngine()
    {
        parameters = new FlexagonParameters();
        ScriptingClass = new FlexagonScripting();
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="par">Command line options</param>
    public FlexagonEngine(ICommandLine par) : base(par)
    {
        FlexagonCommandLine p = (FlexagonCommandLine)par;
        Faces = p.Faces;

        ScriptingClass = new FlexagonScripting();
        Script = p.Script;
        parameters = new FlexagonParameters();
        parameters.OutputName = p.OutputName;
    }
    #endregion

    #region json
    /// <summary>
    /// Returns the parameters in json format
    /// </summary>
    /// <returns></returns>
    public override string GetJsonParams()
    {
        GetBaseJsonParams();
        FlexagonParameters p = (FlexagonParameters)parameters;
        p.Faces = Faces;

        return JsonSerializer.Serialize(p);
    }

    /// <summary>
    /// Sets the parameters from json formatted string
    /// </summary>
    /// <param name="json"></param>
    public override void SetJsonParams(string json)
    {
        FlexagonParameters p = JsonSerializer.Deserialize<FlexagonParameters>(json);
        parameters = p;
        SetBaseJsonParams();

        Faces = p.Faces;
    }
    #endregion

}
