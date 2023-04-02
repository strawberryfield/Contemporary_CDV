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
            sb.AppendLine(HelpUtils.OptionsList(builtins));
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
            sb.AppendLine(HelpUtils.MDOptionsList(builtins));
            sb.AppendLine("The parameters can be stored in a file instead of a string.  ");
            sb.AppendLine("The file must be referenced as '@filename'");
            return sb.ToString();
        }
    }

    private static List<HelpUtils.ParItem> builtins = new()
    {
        new("xc:color", "Fill the image with the specified color"),
        new(
        "gradient:color1-color2",
        "Fill the image with linear gradient\nfrom color1 to color2,\nif colors are omitted is white to black"),
        new("radial-gradient:color1-color2", "Radial gradient as above"),
        new("plasma:color1-color2", "Plasma gradient from color1 to color2,\nif colors are omitted is black to black"),
        new("plasma:fractal", "Creates a random plasma"),
        new("label:text", "Render the plain text with no word-wrap"),
        new("caption:text", "Render the plain text with auto word-wrap"),
        new("pango:text", "Render the text with pango markup"),
        new("pattern:pattern_name", "Creates the image tiling built-in pattern\nuse --patterns to see the patterns list"),
    };

    private static List<HelpUtils.ParItem> patterns = new()
    {
        new("BRICKS", "brick pattern, 16x16"),
        new("CHECKERBOARD", "checkerboard pattern, 30x30"),
        new("CIRCLES", "circles pattern, 16x16"),
        new("CROSSHATCH", "crosshatch pattern, 8x4"),
        new("CROSSHATCH30", "crosshatch pattern with lines at 30 degrees, 8x4"),
        new("CROSSHATCH45", "crosshatch pattern with lines at 45 degrees, 8x4"),
        new("FISHSCALES", "fish scales pattern, 16x8"),
        new("GRAY0", "0% intensity gray, 32x32"),
        new("GRAY5", "5% intensity gray, 32x32"),
        new("GRAY10", "10% intensity gray, 32x32"),
        new("GRAY15", "15% intensity gray, 32x32"),
        new("GRAY20", "20% intensity gray, 32x32"),
        new("GRAY25", "25% intensity gray, 32x32"),
        new("GRAY30", "30% intensity gray, 32x32"),
        new("GRAY35", "35% intensity gray, 32x32"),
        new("GRAY40", "40% intensity gray, 32x32"),
        new("GRAY45", "45% intensity gray, 32x32"),
        new("GRAY50", "50% intensity gray, 32x32"),
        new("GRAY55", "55% intensity gray, 32x32"),
        new("GRAY60", "60% intensity gray, 32x32"),
        new("GRAY65", "65% intensity gray, 32x32"),
        new("GRAY70", "70% intensity gray, 32x32"),
        new("GRAY75", "75% intensity gray, 32x32"),
        new("GRAY80", "80% intensity gray, 32x32"),
        new("GRAY85", "85% intensity gray, 32x32"),
        new("GRAY90", "90% intensity gray, 32x32"),
        new("GRAY95", "95% intensity gray, 32x32"),
        new("GRAY100", "100% intensity gray, 32x32"),
        new("HEXAGONS", "hexagon pattern, 30x18"),
        new("HORIZONTAL", "horizontal line pattern, 8x4"),
        new("HORIZONTAL2", "horizontal line pattern, 8x8"),
        new("HORIZONTAL3", "horizontal line pattern, 9x9"),
        new("HORIZONTALSAW", "horizontal saw-tooth pattern, 16x8"),
        new("HS_BDIAGONAL", "backward diagonal line pattern (45 degrees slope), 8x8"),
        new("HS_CROSS", "cross line pattern, 8x8"),
        new("HS_DIAGCROSS", "diagonal line cross pattern (45 degrees slope), 8x8"),
        new("HS_FDIAGONAL", "forward diagonal line pattern (45 degrees slope), 8x8"),
        new("HS_HORIZONTAL", "horizontal line pattern, 8x8"),
        new("HS_VERTICAL", "vertical line pattern, 8x8"),
        new("LEFT30", "forward diagonal pattern (30 degrees slope), 8x4"),
        new("LEFT45", "forward diagonal line pattern (45 degrees slope), 8x8"),
        new("LEFTSHINGLE", "left shingle pattern, 24x24"),
        new("OCTAGONS", "octagons pattern, 16x16"),
        new("RIGHT30", "backward diagonal line pattern (30 degrees) 8x4"),
        new("RIGHT45", "backward diagonal line pattern (30 degrees), 8x8"),
        new("RIGHTSHINGLE", "right shingle pattern, 24x24"),
        new("SMALLFISHSCALES", "small fish scales pattern, 8x8"),
        new("VERTICAL", "vertical line pattern, 8x8"),
        new("VERTICAL2", "vertical line pattern, 8x8"),
        new("VERTICAL3", "vertical line pattern, 9x9"),
        new("VERTICALBRICKS", "vertical brick pattern, 16x16"),
        new("VERTICALLEFTSHINGLE", "vertical left shingle pattern, 24x24"),
        new("VERTICALRIGHTSHINGLE", "vertical right shingle pattern, 24x24"),
        new("VERTICALSAW", "vertical saw-tooth pattern, 8x16"),
    };

    /// <summary>
    /// Imagemagick built-in patterns
    /// </summary>
    public static string PatternsHelp
    {
        get
        {
            StringBuilder sb = new();
            sb.AppendLine("Available built-in patterns:");
            sb.AppendLine(HelpUtils.OptionsList(patterns));
            return sb.ToString();
        }
    }

    /// <summary>
    /// Imagemagick built-in patterns formatted in Markdown
    /// </summary>
    public static string PatternsMan => HelpUtils.MDOptionsList(patterns);
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
        foreach(var s in Enum.GetValues(typeof(Gravity)).Cast<Gravity>())
        {
            if(s != 0)
                sb.AppendLine(s.ToString());
        }
        return sb.ToString();
    }
    #endregion

    #region colors
    /// <summary>
    /// List of predefined colors
    /// </summary>
    /// <returns></returns>
    public static string ColorsDesc()
    {
        StringBuilder sb = new();
        sb.AppendLine("Predefined color are:");
        Colors colors = new();
        foreach(var color in colors.colorDictionary)
        {
            sb.AppendLine($"{color.Key,-24}{color.Value.ToHexString()}");
        }
        return sb.ToString();
    }
    #endregion 
}
