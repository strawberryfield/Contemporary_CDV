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
using System.Text.Json;

namespace Casasoft.CCDV.Engines;

/// <summary>
/// Creates a matrix of cubes with 6 images
/// </summary>
public class CubettiEngine : BaseEngine
{
    #region
    /// <summary>
    /// Number of rows to generate
    /// </summary>
    public int Rows { get; set; } = 2;
    /// <summary>
    /// Number of Columns to generate
    /// </summary>
    public int Columns { get; set; } = 3;
    /// <summary>
    /// Size of any cube (mm)
    /// </summary>
    public int Size { get; set; } = 50;
    #endregion

    #region constructors
    /// <summary>
    /// Constructor
    /// </summary>
    public CubettiEngine()
    {
        parameters = new CubettiParameters();
        ScriptingClass = new CubettiScripting();
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="par">Command line options</param>
    public CubettiEngine(ICommandLine par) : base(par)
    {
        CubettiCommandLine p = (CubettiCommandLine)par;
        Rows = p.Rows;
        Columns = p.Columns;    
        Size = p.Size;

        ScriptingClass = new CubettiScripting();
        Script = p.Script;
        parameters = new CubettiParameters();
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
        CubettiParameters p = (CubettiParameters)parameters;
        p.Rows = Rows;
        p.Columns = Columns;
        p.Size = Size;

        return JsonSerializer.Serialize(p);
    }

    /// <summary>
    /// Sets the parameters from json formatted string
    /// </summary>
    /// <param name="json"></param>
    public override void SetJsonParams(string json)
    {
        CubettiParameters p = JsonSerializer.Deserialize<CubettiParameters>(json);
        parameters = p;
        SetBaseJsonParams();

        Rows = p.Rows;
        Columns = p.Columns;
        Size = p.Size;

    }
    #endregion

    #region build
    /// <summary>
    /// Does the dirty work
    /// </summary>
    /// <returns>Image to print</returns>
    public List<MagickImage> GetResults(bool quiet)
    {
        List<MagickImage> final = new();
        return final;
    }
    #endregion

}
