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
using Mono.Options;
using System.Text.Json;

namespace Casasoft.CCDV;

/// <summary>
/// Command line management for MontaggioFoto
/// </summary>
public class MontaggioFotoCommandLine : CommandLine
{
    /// <summary>
    /// true if images must fit the entire 100x64mm format
    /// </summary>
    public bool FullSize { get; set; }
    /// <summary>
    /// true if images will fit the entire 100x64mm format with 5mm border on each side
    /// </summary>
    public bool WithBorder { get; set; }
    /// <summary>
    /// true if white space around the images must be deleted
    /// </summary>
    public bool Trim { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="outputname">Default output file name</param>
    /// <param name="desc">brief description of the program</param>
    public MontaggioFotoCommandLine(string outputname, string desc = "") :
    this(ExeName(), outputname, desc)
    { }
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="exename">Name of the program exe</param>
    /// <param name="outputname">Default output file name</param>
    /// <param name="desc">brief description of the program</param>
    public MontaggioFotoCommandLine(string exename, string outputname, string desc = "") :
        base(exename, outputname, desc)
    {
        FullSize = false;
        WithBorder = false;
        Trim = false;

        Options = new OptionSet
            {
                { "fullsize", "resize image to full format", o => FullSize = o != null },
                { "withborder", "include border to full format", o => WithBorder = o != null },
                { "trim", "trim white space", o => Trim = o != null },
            };
        AddBaseOptions();
    }

    /// <summary>
    /// Prints a json schema for pameters
    /// </summary>
    /// <returns></returns>
    public override string JsonTemplate()
    {
        BaseBuilderParameters p = new();
        return JsonSerializer.Serialize(p, new JsonSerializerOptions { WriteIndented = true });
    }
}

