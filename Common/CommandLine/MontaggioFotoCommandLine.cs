// copyright (c) 2020-2021 Roberto Ceccarelli - Casasoft
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

using Mono.Options;

namespace Casasoft.CCDV;

public class MontaggioFotoCommandLine : CommandLine
{
    public bool FullSize { get; set; }
    public bool WithBorder { get; set; }
    public bool Trim { get; set; }

    public MontaggioFotoCommandLine(string outputname, string desc = "") :
    this(ExeName(), outputname, desc)
    { }
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
}

