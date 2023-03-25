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

using ImageMagick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Casasoft.CCDV;

/// <summary>
/// Texts for help of ImageMagick options
/// </summary>
public static class ImageMagickHelp
{
    #region built-in
    /// <summary>
    /// Imagemagick built-in templates
    /// </summary>
    public static string BuiltInHelp
    {
        get
        {
            StringBuilder sb = new();
            sb.AppendLine("Instead of a filename you can use the following built-in templates:");
            foreach (builtin item in builtins)
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
            sb.AppendLine();
            sb.AppendLine("The parameters can be stored in a file instead of a string.");
            sb.AppendLine("The file must be referenced as '@filename'");
            return sb.ToString();
        }
    }

    /// <summary>
    /// Imagemagick built-in templates formatted in Markdown
    /// </summary>
    public static string BuiltInMan
    {
        get
        {
            StringBuilder sb = new();
            sb.AppendLine("Instead of a filename you can use the following built-in templates:\n");
            foreach (builtin item in builtins)
            {
                sb.AppendLine($"**{HelpUtils.EscapeMarkdown(item.syntax)}** :  ");
                sb.AppendLine(HelpUtils.EscapeMarkdown(item.desc));
                sb.AppendLine("\n");

            }
            sb.AppendLine();
            sb.AppendLine("The parameters can be stored in a file instead of a string.  ");
            sb.AppendLine("The file must be referenced as '@filename'");
            return sb.ToString();
        }
    }

    private record builtin(string syntax, string desc);
    private static List<builtin> builtins = new() {
        new("xc:color","Fill the image with the specified color"),
        new("gradient:color1-color2","Fill the image with linear gradient\nfrom color1 to color2,\nif colors are omitted is white to black"),
        new("radial-gradient:color1-color2","Radial gradient as above"),
        new("plasma:color1-color2","Plasma gradient from color1 to color2,\nif colors are omitted is black to black"),
        new("plasma:fractal","Creates a random plasma"),
        new("label:text","Render the plain text with no word-wrap"),
        new("caption:text","Render the plain text with auto word-wrap"),
        new("pango:text","Render the text with pango markup")
    };
    #endregion

    #region gravity

    /// <summary>
    /// Descriptive values for gravity
    /// </summary>
    /// <returns></returns>
    public static string GravityDesc()
    {
        StringBuilder sb = new();
        sb.AppendLine("valid values are:");
        foreach (var s in Enum.GetValues(typeof(Gravity)).Cast<Gravity>())
        {
            if (s != 0)
                sb.AppendLine(s.ToString());
        }
        return sb.ToString();
    }
    #endregion

}
