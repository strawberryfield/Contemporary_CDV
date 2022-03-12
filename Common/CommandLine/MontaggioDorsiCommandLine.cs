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
using Mono.Options;
using System.Text.Json;

namespace Casasoft.CCDV;

/// <summary>
/// Command line management for MontaggioDorsi
/// </summary>
public class MontaggioDorsiCommandLine : CommandLine
{
    /// <summary>
    /// Output paper size
    /// </summary>
    public string Paper { get; set; }

    /// <summary>
    /// Output paper size
    /// </summary>
    public PaperFormats PaperFormat
    {
        get => Utils.GetThickFormat(Paper);
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="outputname">Default output file name</param>
    /// <param name="desc">brief description of the program</param>
    public MontaggioDorsiCommandLine(string outputname, string desc = "") :
        this(ExeName(), outputname, desc)
    {
    }
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="exename">Name of the program exe</param>
    /// <param name="outputname">Default output file name</param>
    /// <param name="desc">brief description of the program</param>
    public MontaggioDorsiCommandLine(string exename, string outputname, string desc = "") :
        base(exename, outputname, desc)
    {
        Paper = string.Empty;

        Options = new OptionSet
            {
                { "paper=", "Output paper size:\nLarge (default) 20x27cm\nMedium 15x20cm", o => Paper = o  },
            };
        AddBaseOptions();
    }

    /// <summary>
    /// Prints a json schema for pameters
    /// </summary>
    /// <returns></returns>
    public override string JsonTemplate()
    {
        MontaggioDorsiParameters p = new();
        return JsonSerializer.Serialize(p, new JsonSerializerOptions { WriteIndented = true });
    }

    /// <summary>
    /// Prints a script template
    /// </summary>
    /// <returns></returns>
    public override string ScriptTemplate()
    {
        MontaggioDorsiScripting sc = new();
        return base.ScriptTemplate() + sc.Template;
    }
}
