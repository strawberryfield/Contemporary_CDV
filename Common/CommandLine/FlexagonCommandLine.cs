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
using System.Drawing;
using System.Text.Json;

namespace Casasoft.CCDV;

/// <summary>
/// Command line extensions for flexagons builder
/// </summary>
public class FlexagonCommandLine : CommandLine
{
    /// <summary>
    /// Number of faces of the flexagons
    /// </summary>
    /// <remarks>
    /// Valid numbers are 3, 4 or 6
    /// </remarks>
    public int Faces { get; set; }

    private string sFaces = "3";

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="outputname">Default output file name</param>
    /// <param name="desc">brief description of the program</param>
    public FlexagonCommandLine(string outputname, string desc = "") : this(ExeName(), outputname, desc)
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="exename">Name of the program exe</param>
    /// <param name="outputname">Default output file name</param>
    /// <param name="desc">brief description of the program</param>
    public FlexagonCommandLine(string exename, string outputname, string desc = "") : base(exename, outputname, desc)
    {
        Options = new OptionSet
            {
                { "f|faces=", $"number of faces of the flexagaon (3,4 or 6; default {sFaces})", t => sFaces = t },
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

        Faces = GetIntParameter(sFaces, Faces,
            $"Incorrect faces value '{sFaces}'. Using default value.");

        return false;
    }

    /// <summary>
    /// Prints a json schema for pameters
    /// </summary>
    /// <returns></returns>
    public override string JsonTemplate()
    {
        FlexagonParameters p = new();
        return JsonSerializer.Serialize(p, new JsonSerializerOptions { WriteIndented = true });
    }

    /// <summary>
    /// Prints a script template
    /// </summary>
    /// <returns></returns>
    public override string ScriptTemplate()
    {
        FlexagonScripting sc = new();
        return base.ScriptTemplate() + sc.Template();
    }
}
