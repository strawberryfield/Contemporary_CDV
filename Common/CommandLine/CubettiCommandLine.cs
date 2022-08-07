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
using Mono.Options;
using System.Text.Json;

namespace Casasoft.CCDV;

/// <summary>
/// CommandLine manager for Cubetti
/// </summary>
public class CubettiCommandLine : CommandLine
{
    /// <summary>
    /// Number of rows to generate
    /// </summary>
    public int Rows { get; set; }
    /// <summary>
    /// Number of Columns to generate
    /// </summary>
    public int Columns { get; set; }
    /// <summary>
    /// Size of any cube (mm)
    /// </summary>
    public int Size { get; set; }

    private string sRows = "2";
    private string sColumns = "3";  
    private string sSize = "50"; 

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="outputname">Default output file name</param>
    /// <param name="desc">brief description of the program</param>
    public CubettiCommandLine(string outputname, string desc) : base(outputname, desc)
    {

    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="exename">Name of the program exe</param>
    /// <param name="outputname">Default output file name</param>
    /// <param name="desc">brief description of the program</param>
    public CubettiCommandLine(string exename, string outputname, string desc) : base(exename, outputname, desc)
    {
        Options = new OptionSet
            {
                { "r|rows=", $"set the number of rows of the output matrix (default {sRows})", t => sRows = t },
                { "c|columns=", $"set the number of columns of the output matrix (default {sRows})", t => sColumns = t },
                { "s|size=", $"set the size of each cube (default {sSize}mm)", t => sSize = t },
            };
        AddBaseOptions();
    }

    /// <summary>
    /// Does the dirty work
    /// </summary>
    /// <param name="args">command line arguments</param>
    /// <returns>true if nothing to (ie. help)</returns>
    public override bool Parse(string[] args)
    {
        if (base.Parse(args)) return true;

        Rows = GetIntParameter(sRows, Rows,
            $"Incorrect rows value '{sRows}'. Using default value.");
        Columns = GetIntParameter(sColumns, Columns,
            $"Incorrect columns value '{sColumns}'. Using default value.");
        Size = GetIntParameter(sSize, Size,
            $"Incorrect size value '{sSize}'. Using default value.");

        return false;
    }

    /// <summary>
    /// Prints a json schema for pameters
    /// </summary>
    /// <returns></returns>
    public override string JsonTemplate()
    {
        CubettiParameters p = new();
        return JsonSerializer.Serialize(p, new JsonSerializerOptions { WriteIndented = true });
    }

    /// <summary>
    /// Prints a script template
    /// </summary>
    /// <returns></returns>
    public override string ScriptTemplate()
    {
        CubettiScripting sc = new();
        return base.ScriptTemplate() + sc.Template();
    }

}
