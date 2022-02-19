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
using ImageMagick;
using Mono.Options;
using System.Text.Json;

namespace Casasoft.CCDV;

/// <summary>
/// Command line extensions for credit cards builder
/// </summary>
public class CreditCardCommandLine : CommandLine
{
    /// <summary>
    /// Text to print on the front (like cardholder)
    /// </summary>
    public string FrontText { get; set; }
    /// <summary>
    /// Front text font
    /// </summary>
    public string FrontTextFont { get; set; }
    /// <summary>
    /// Front text fill color
    /// </summary>
    public MagickColor FrontTextColor { get; set; }
    /// <summary>
    /// front text border color
    /// </summary>
    public MagickColor FrontTextBorder { get; set; }
    /// <summary>
    /// front text background color
    /// </summary>
    public MagickColor FrontTextBackground { get; set; }

    /// <summary>
    /// use bold weight for front text (if available for font)
    /// </summary>
    public bool fontBold { get; set; }
    /// <summary>
    /// use italic style for front text (if available for font)
    /// </summary>
    public bool fontItalic { get; set; }
    /// <summary>
    /// Pseudo magnetic band color
    /// </summary>
    public MagickColor MagneticBandColor { get; set; }
    /// <summary>
    /// Pseudo magnetic band image
    /// </summary>
    public string MagneticBandImage { get; set; }
    /// <summary>
    /// backgroud image
    /// </summary>
    public string BackImage { get; set; }
    /// <summary>
    /// text to put in the back side
    /// </summary>
    public string BackText { get; set; }

    private string sFrontTextColor = MagickColors.Black.ToHexString();
    private string sFrontTextBorder = MagickColors.Black.ToHexString();
    private string sFrontTextBackground = MagickColors.None.ToHexString();
    private string sMagneticBandColor = MagickColors.SaddleBrown.ToHexString();

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="outputname">Default output file name</param>
    /// <param name="desc">brief description of the program</param>
    public CreditCardCommandLine(string outputname, string desc = "") :
    this(ExeName(), outputname, desc)
    { }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="exename">Name of the program exe</param>
    /// <param name="outputname">Default output file name</param>
    /// <param name="desc">brief description of the program</param>
    public CreditCardCommandLine(string exename, string outputname, string desc = "") :
        base(exename, outputname, desc)
    {
        FrontText = string.Empty;
        FrontTextFont = "Arial";
        FrontTextColor = GetColor(sFrontTextColor);
        FrontTextBorder = GetColor(sFrontTextBorder);
        FrontTextBackground = GetColor(sFrontTextBackground);
        MagneticBandColor = GetColor(sMagneticBandColor);
        MagneticBandImage = string.Empty;
        BackImage = string.Empty;
        BackText = string.Empty;

        Options = new OptionSet
            {
                { "fronttext=", "text in front (Cardholder name)", o => FrontText = o  },
                { "fronttextfont=", $"front text font (default '{FrontTextFont}')", o => FrontTextFont = o },
                { "fronttextcolor=", $"front text color (default {sFrontTextColor})", o => sFrontTextColor = o },
                { "fronttextborder=", $"front text border color (default {sFrontTextBorder})", o => sFrontTextBorder = o },
                { "fronttextbackground=", $"front text background color (default {sFrontTextBackground})", o => sFrontTextBackground = o },
                { "fontbold", "use bold font weight", s => fontBold = s != null },
                { "fontitalic", "use italic font style", s => fontItalic = s != null },
                { "mbcolor=", $"magnetic band color (default {sMagneticBandColor})", o => sMagneticBandColor = o },
                { "mbimage=", $"magnetic band overlay image", o => MagneticBandImage = o },
                { "backimage=", "image for back side", o => BackImage = o },
                { "backtext=", @"pango markup for text on back side.  
Text can be stored in a file instead of a string.  
The file must be referenced as '@filename'",
                    o => BackText = GetFileParameter(o) },

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

        FrontTextColor = GetColor(sFrontTextColor);
        FrontTextBorder = GetColor(sFrontTextBorder);
        FrontTextBackground = GetColor(sFrontTextBackground);
        MagneticBandColor = GetColor(sMagneticBandColor);
        return false;
    }

    /// <summary>
    /// Prints a json schema for pameters
    /// </summary>
    /// <returns></returns>
    public override string JsonTemplate()
    {
        CreditCardParameters p = new();
        return JsonSerializer.Serialize(p, new JsonSerializerOptions { WriteIndented = true });
    }
}
