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

using Mono.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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

    /// <summary>
    /// Converts Mono Options generated help to Markdown
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public static string MDMonoOptions(OptionSet options) {
        StringBuilder ret = new();
        StringWriter sw = new StringWriter();
        options.WriteOptionDescriptions(sw);
        string[] opts = sw.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        bool first = true;
        foreach (string s in opts)
        {
            if (string.IsNullOrWhiteSpace(s))
                continue;

            string o = s.Substring(0, 29).Trim();
            if (string.IsNullOrWhiteSpace(o))
            {
                ret.Append($"{EscapeMarkdown(s.Trim())}  \n");
            }
            else
            {
                ret.Append(first ? string.Empty : "\n\n");
                ret.Append($"**{o}** :  \n{EscapeMarkdown(s.Substring(29).Trim())}  \n");
            }
            first = false;
        }
        return ret.ToString();
    }


    /// <summary>
    /// Item for lists of descriptions
    /// </summary>
    /// <param name="syntax"></param>
    /// <param name="desc"></param>
    public record ParItem(string syntax, string desc);

    /// <summary>
    /// Format a list of options to text
    /// </summary>
    /// <param name="optList"></param>
    /// <returns></returns>
    public static string OptionsList(List<ParItem> optList)
    {
        StringBuilder sb = new();
        foreach (ParItem item in optList)
        {
            sb.Append("  ");
            sb.Append(item.syntax.PadRight(32));
            string[] rdesc = item.desc.Split('\n');
            sb.AppendLine(rdesc[0]);
            for (int i = 1; i < rdesc.Length; i++)
            {
                sb.Append(new string(' ', 36));
                sb.AppendLine(rdesc[i]);
            }
        }
        return sb.ToString();
    }

    /// <summary>
    /// Format a list of options to Markdown
    /// </summary>
    /// <param name="optList"></param>
    /// <returns></returns>
    public static string MDOptionsList(List<ParItem> optList)
    {
        StringBuilder sb = new();
        foreach (ParItem item in optList)
        {
            sb.AppendLine($"**{EscapeMarkdown(item.syntax)}** :  ");
            sb.AppendLine(EscapeMarkdown(item.desc));
            sb.AppendLine("\n");
        }
        return sb.ToString();
    }
}
