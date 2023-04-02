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
/// Command line management for MontaggioDorsi
/// </summary>
public class MontaggioDorsiCommandLine : CommandLine
{
    /// <summary>
    /// Output paper size
    /// </summary>
    public string Paper { get; set; }

    /// <summary>
    /// Output paper size
    /// </summary>
    public PaperFormats PaperFormat
    {
        get => Utils.GetPaperFormat(Paper);
    }

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
    public MontaggioDorsiCommandLine(string outputname, string desc = "") :
        this(ExeName(), outputname, desc)
    {
    }
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="exename">Name of the program exe</param>
    /// <param name="outputname">Default output file name</param>
    /// <param name="desc">brief description of the program</param>
    public MontaggioDorsiCommandLine(string exename, string outputname, string desc = "") :
        base(exename, outputname, desc)
    {
        Paper = string.Empty;

        Options = new OptionSet
            {
                { "paper=", "Output paper size:\nLarge (default) 20x27cm\nMedium 15x20cm\nA4 210x297mm", o => Paper = o  },
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

        CanvasGravity = GetGravity(sGravity);
        return false;
    }

    /// <summary>
    /// Adds help for built-in images and canvases
    /// </summary>
    protected override void ExtraHelp()
    {
        Console.WriteLine("\nBuilt-in images and renders");
        Console.Write(ImageMagickHelp.BuiltInHelp);
    }

    /// <summary>
    /// Handles --patterns
    /// </summary>
    /// <returns></returns>
    protected override bool ShouldShowExtra()
    {
        if(shouldShowPatterns)
        {
            Console.WriteLine(ImageMagickHelp.PatternsHelp);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Adds help for built-in images and canvases
    /// </summary>
    protected override string ExtraMan() =>  @$"
# BUILT-IN IMAGES AND RENDERS
{ImageMagickHelp.BuiltInMan}
";

    /// <summary>
    /// Prints a json schema for pameters
    /// </summary>
    /// <returns></returns>
    public override string JsonTemplate()
    {
        MontaggioDorsiParameters p = new();
        return JsonSerializer.Serialize(p, new JsonSerializerOptions { WriteIndented = true });
    }

    /// <summary>
    /// Prints a script template
    /// </summary>
    /// <returns></returns>
    public override string ScriptTemplate()
    {
        MontaggioDorsiScripting sc = new();
        return base.ScriptTemplate() + sc.Template();
    }
}
