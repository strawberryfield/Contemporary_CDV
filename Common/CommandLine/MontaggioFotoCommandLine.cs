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

using Casasoft.CCDV.JSON;
using Casasoft.CCDV.Scripting;
using ImageMagick;
using Mono.Options;
using System;
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
    /// Blank border around the image
    /// </summary>
    public int Padding { get; set; }
    private string sPadding = "0";
    /// <summary>
    /// Canvas gravity
    /// </summary>
    public Gravity CanvasGravity { get; set; }
    private string sGravity = "CENTER";

    /// <summary>
    /// true if list of patterns is requested
    /// </summary>
    protected bool shouldShowPatterns { get; set; }

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
                { "p|padding=", "blank border around the image", s => sPadding = s },
                { "gravity=", $"canvas gravity, {ImageMagickHelp.GravityDesc()}", s => sGravity = s },
                { "patterns", "show built-in patterns list", h => shouldShowPatterns = h != null },
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

        Padding = GetIntParameter(sPadding, Padding,
            $"Incorrect padding value '{sPadding}'. Using default value.");
        CanvasGravity = GetGravity(sGravity);
        return false;
    }

    /// <summary>
    /// Adds help for built-in images and canvases
    /// </summary>
    protected override void ExtraHelp()
    {
        Console.WriteLine("\nBuilt-in images and renders");
        Console.WriteLine(ImageMagickHelp.BuiltInHelp);
    }

    /// <summary>
    /// Handles --patterns
    /// </summary>
    /// <returns></returns>
    protected override bool ShouldShowExtra()
    {
        if (shouldShowPatterns)
        {
            Console.WriteLine(ImageMagickHelp.PatternsHelp);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Adds help for built-in images and canvases
    /// </summary>
    protected override string ExtraMan() => @$"
# BUILT-IN IMAGES AND RENDERS
{ImageMagickHelp.BuiltInMan}
";

    /// <summary>
    /// Prints a json schema for parameters
    /// </summary>
    /// <returns></returns>
    public override string JsonTemplate()
    {
        MontaggioFotoParameters p = new();
        return JsonSerializer.Serialize(p, new JsonSerializerOptions { WriteIndented = true });
    }

    /// <summary>
    /// Prints a script template
    /// </summary>
    /// <returns></returns>
    public override string ScriptTemplate()
    {
        MontaggioFotoScripting sc = new();
        return base.ScriptTemplate() + sc.Template();
    }
}

