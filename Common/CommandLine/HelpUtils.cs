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

using System.Xml.Linq;

namespace Casasoft.CCDV;

/// <summary>
/// Utilities for command line help
/// </summary>
public static class HelpUtils
{
    /// <summary>
    /// Escape markdown special characters
    /// </summary>
    /// <param name="s">string to process</param>
    /// <returns>escaped string</returns>
    public static string EscapeMarkdown(string s) => s.Replace("\r", "  \r")
        .Replace("\\", "\\\\")
        .Replace("#", "\\#")
        .Replace("*", "\\*")
        .Replace("_", "\\_")
        .Replace("(", "\\(")
        .Replace(")", "\\)")
        .Replace("[", "\\[")
        .Replace("]", "\\]")
        .Replace("{", "\\{")
        .Replace("}", "\\}");

    /// <summary>
    /// Copyright and disclaimer notice for man
    /// </summary>
    /// <param name="prgName"></param>
    /// <returns></returns>
    public static string MDCopyright(string prgName) => @$"# COPYRIGHT
Casasoft {prgName} is free software:  
you can redistribute it and/or modify it  
under the terms of the GNU Affero General Public License as published by  
the Free Software Foundation, either version 3 of the License, or  
\(at your option\) any later version.  

You should have received a copy of the GNU AGPL v.3  
along with Casasoft {prgName}.  
If not, see <http://www.gnu.org/licenses/>.  

# DISCLAIMER
Casasoft {prgName} is distributed in the hope that it will be useful,  
but WITHOUT ANY WARRANTY; without even the implied warranty of  
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.   
See the GNU General Public License for more details.";

}
