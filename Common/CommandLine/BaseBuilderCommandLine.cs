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

using Mono.Options;
using System;

namespace Casasoft.CCDV;

/// <summary>
/// Available target formats
/// </summary>
public enum TargetType
{
    /// <summary>
    /// Carte de viste 64x100mm
    /// </summary>
    cdv, 
    /// <summary>
    /// Credit card 86x54mm
    /// </summary>
    cc
}

/// <summary>
/// Command line extensions for boxes and folders
/// </summary>
public class BaseBuilderCommandLine : CommandLine
{
    #region properties
    /// <summary>
    /// Box thickness
    /// </summary>
    public int thickness { get; set; }
    /// <summary>
    /// Image for top border
    /// </summary>
    public string topImage { get; set; }
    /// <summary>
    /// Image for bottom border
    /// </summary>
    public string bottomImage { get; set; }
    /// <summary>
    /// Front image
    /// </summary>
    public string frontImage { get; set; }
    /// <summary>
    /// Back side image
    /// </summary>
    public string backImage { get; set; }
    /// <summary>
    /// Image for left border
    /// </summary>
    public string leftImage { get; set; }
    /// <summary>
    /// Image for right border
    /// </summary>
    public string rightImage { get; set; }
    /// <summary>
    /// true if longer size is horizontal
    /// </summary>
    public bool isHorizontal { get; set; }
    /// <summary>
    /// True if samples images will be created
    /// </summary>
    public bool useSampleImages { get; set; }
    /// <summary>
    /// Text to print on left border
    /// </summary>
    public string borderText { get; set; }
    /// <summary>
    /// fonst for text
    /// </summary>
    public string font { get; set; }
    /// <summary>
    /// use font bold weight (if available)
    /// </summary>
    public bool fontBold { get; set; }
    /// <summary>
    /// use font italic style  (if available)
    /// </summary>
    public bool fontItalic { get; set; }

    /// <summary>
    /// Size of box
    /// </summary>
    public TargetType targetType { get; set; }

    private string sThickness = "5";
    private string sTargetType = "CDV";
    private string sOrientation = "PORTRAIT";
    #endregion

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="outputname">Default output file name</param>
    /// <param name="desc">brief description of the program</param>
    public BaseBuilderCommandLine(string outputname, string desc = "") :
        this(ExeName(), outputname, desc)
    { }
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="exename">Name of the program exe</param>
    /// <param name="outputname">Default output file name</param>
    /// <param name="desc">brief description of the program</param>
    public BaseBuilderCommandLine(string exename, string outputname, string desc = "") :
        base(exename, outputname, desc)
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

    /// <summary>
    /// Does the dirty work
    /// </summary>
    /// <param name="args">command line arguments</param>
    /// <returns>true if nothing to (ie. help)</returns>
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

