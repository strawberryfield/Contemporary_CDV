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

using ImageMagick;
using Mono.Options;

namespace Casasoft.CCDV;

public class CreditCardCommandLine : CommandLine
{
    public string FrontText { get; set; }
    public string FrontTextFont { get; set; }
    public MagickColor FrontTextColor { get; set; }
    public MagickColor FrontTextBorder { get; set; }
    public bool fontBold { get; set; }
    public bool fontItalic { get; set; }
    public MagickColor MagneticBandColor { get; set; }
    public string MagneticBandImage { get; set; }
    public string BackImage { get; set; }
    public string BackText { get; set; }

    private string sFrontTextColor = MagickColors.Black.ToHexString();
    private string sFrontTextBorder = MagickColors.Black.ToHexString();
    private string sMagneticBandColor = MagickColors.SaddleBrown.ToHexString();

    public CreditCardCommandLine(string outputname, string desc = "") :
    this(ExeName(), outputname, desc)
    { }

    public CreditCardCommandLine(string exename, string outputname, string desc = "") :
        base(exename, outputname, desc)
    {
        FrontText = string.Empty;
        FrontTextFont = "Arial";
        FrontTextColor = GetColor(sFrontTextColor);
        FrontTextBorder = GetColor(sFrontTextBorder);
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
                { "fontbold", "use bold font weight", s => fontBold = s != null },
                { "fontitalic", "use italic font style", s => fontItalic = s != null },
                { "mbcolor=", $"magnetic band color (default {sMagneticBandColor})", o => sMagneticBandColor = o },
                { "mbimage=", $"magnetic band overlay image", o => MagneticBandImage = o },
                { "backimage=", "image for back side", o => BackImage = o },
                { "backtext=", "pango markup for text on back side." +
                    "\nText can be stored in a file instead of a string." +
                    "The file must be referenced as '@filename'",
                    o => BackText = GetFileParameter(o) },

            };
        AddBaseOptions();
    }

    public override bool Parse(string[] args)
    {
        if (base.Parse(args)) return true;

        FrontTextColor = GetColor(sFrontTextColor);
        FrontTextBorder = GetColor(sFrontTextBorder);
        MagneticBandColor = GetColor(sMagneticBandColor);
        return false;
    }

}
