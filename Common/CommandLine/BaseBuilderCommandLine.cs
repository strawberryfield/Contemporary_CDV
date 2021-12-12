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
using System;

namespace Casasoft.CCDV;

public enum TargetType { cdv, cc }

public class BaseBuilderCommandLine : CommandLine
{
    public int thickness { get; set; }
    public string topImage { get; set; }
    public string bottomImage { get; set; }
    public string frontImage { get; set; }
    public string backImage { get; set; }
    public string leftImage { get; set; }
    public string rightImage { get; set; }
    public bool isHorizontal { get; set; }
    public bool useSampleImages { get; set; }
    public string borderText { get; set; }
    public string font { get; set; }
    public bool fontBold { get; set; }
    public bool fontItalic { get; set; }    

    public TargetType targetType { get; set; }

    private string sThickness = "5";
    private string sTargetType = "CDV";
    private string sOrientation = "PORTRAIT";

    public BaseBuilderCommandLine(string outputname) :
        this(ExeName(), outputname)
    { }
    public BaseBuilderCommandLine(string exename, string outputname) :
        base(exename, outputname)
    {
        topImage = string.Empty;
        bottomImage = string.Empty;
        frontImage = string.Empty;
        backImage = string.Empty;
        leftImage = string.Empty;
        rightImage = string.Empty;
        borderText = string.Empty;
        font = "Arial";

        Options = new OptionSet
            {
                { "a|aboveimage=", "set the image for the top cover", i => topImage = i },
                { "z|bottomimage=", "set the image for the bottom", i => bottomImage = i },
                { "l|leftimage=", "set the image for the left border", i => leftImage = i },
                { "r|rightimage=", "set the image for the right border", i => rightImage = i },
                { "f|frontimage=", "set the image for the front", i => frontImage = i },
                { "b|backimage=", "set the image for the back", i => backImage = i },
                { "t|thickness=", $"set the box thickness (default {sThickness}mm)", t => sThickness = t },
                { "bordertext=", "text to print on left border", t => borderText = t },
                { "font=", $"text font (default {font})", t => font = t },
                { "fontbold", "use bold font weight", s => fontBold = s != null },
                { "fontitalic", "use italic font style", s => fontItalic = s != null },
                { "format=", $"size of the box: 'cdv' or 'cc' (default '{sTargetType}')", t => sTargetType = t.ToUpper() },
                { "orientation=", $"orientation of the box: 'portrait' or 'landscape' (default '{sOrientation}')", t => sOrientation = t.ToUpper() },
                { "sample", "generate sample images", s => useSampleImages = s != null },
            };
        AddBaseOptions();
    }

    public override bool Parse(string[] args)
    {
        if (base.Parse(args)) return true;

        thickness = GetIntParameter(sThickness, thickness,
            $"Incorrect thickness value '{sThickness}'. Using default value.");

        if (sTargetType == "CDV")
        {
            targetType = TargetType.cdv;
        }
        else if (sTargetType == "CC")
        {
            targetType = TargetType.cc;
        }
        else
        {
            Console.Error.WriteLine($"Incorrect format value '{sTargetType}'. Using default value.");
            targetType = TargetType.cdv;
        }

        if (sOrientation == "PORTRAIT")
        {
            isHorizontal = false;
        }
        else if (sOrientation == "LANDSCAPE")
        {
            isHorizontal = true;
        }
        else
        {
            Console.Error.WriteLine($"Incorrect orientation value '{sOrientation}'. Using default value.");
            isHorizontal = false;
        }

        return false;
    }
}

