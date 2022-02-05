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

using ImageMagick;
using Mono.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Casasoft.CCDV;

/// <summary>
/// Base command line handling
/// </summary>
public class CommandLine : ICommandLine
{
    #region internal properties
    /// <summary>
    /// true if help is requested
    /// </summary>
    protected bool shouldShowHelp { get; set; }
    /// <summary>
    /// true if colors list is requested
    /// </summary>
    protected bool shouldShowColors { get; set; }
    /// <summary>
    /// true if license is requested
    /// </summary>
    protected bool shouldShowLicense { get; set; }
    /// <summary>
    /// true if man page is requested
    /// </summary>
    protected bool shouldShowMan { get; set; }
    /// <summary>
    /// name of currently running assembly
    /// </summary>
    protected string exeName { get; set; }
    /// <summary>
    /// brief program description
    /// </summary>
    protected string exeDesc { get; set; }
    /// <summary>
    /// list of base MonoOptions options
    /// </summary>
    protected OptionSet baseOptions { get; set; }
    /// <summary>
    /// true if banner suppression is requested
    /// </summary>
    protected bool noBanner { get; set; }
    /// <summary>
    /// Output directory
    /// </summary>
    protected string outputDir { get; private set; }
    /// <summary>
    /// Colors conversion utilities
    /// </summary>
    protected Colors colors;
    #endregion

    #region public properties
    /// <summary>
    /// Output file name
    /// </summary>
    public string OutputName { get; set; }
    /// <summary>
    /// Output resolution
    /// </summary>
    public int Dpi { get; set; }
    /// <summary>
    /// Input files list
    /// </summary>
    public List<string> FilesList { get; set; }
    /// <summary>
    /// MonoOptions options list
    /// </summary>
    public OptionSet Options { get; set; }
    /// <summary>
    /// Usage example
    /// </summary>
    public string Usage { get; set; }
    /// <summary>
    /// Color to fill images
    /// </summary>
    public MagickColor FillColor { get; set; }
    /// <summary>
    /// Color to use for lines and borders
    /// </summary>
    public MagickColor BorderColor { get; set; }
    /// <summary>
    /// Long description for man pages
    /// </summary>
    public string LongDesc { get; set; }
    #endregion

    #region defaults
    private string sDpi = "300";
    private string sFillColor = "#FFFFFF";
    private string sBorderColor = "#000000";
    #endregion

    #region constructors
    /// <summary>
    /// Gets the name of currently running assembly
    /// </summary>
    /// <returns></returns>
    public static string ExeName() => Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName);

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="outputname">Default output file name</param>
    /// <param name="desc">brief description of the program</param>
    public CommandLine(string outputname, string desc = "") :
        this(ExeName(), outputname, desc)
    { }
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="exename">Name of the program exe</param>
    /// <param name="outputname">Default output file name</param>
    /// <param name="desc">brief description of the program</param>
    public CommandLine(string exename, string outputname, string desc = "")
    {
        colors = new();

        exeName = exename;
        exeDesc = desc;
        OutputName = outputname;
        Dpi = Convert.ToInt16(sDpi);
        sDpi = string.Empty;
        shouldShowHelp = false;
        noBanner = false;
        GetEnvVars();
        FillColor = GetColor(sFillColor);
        BorderColor = GetColor(sBorderColor);

        Options = new();
        baseOptions = new OptionSet
            {
                { "fillcolor=", $"set the color used to fiil the images\n(default {sFillColor})", c => sFillColor = c },
                { "bordercolor=", $"set the color used to border the images\n(default {sBorderColor})", c => sBorderColor = c },
                { "dpi=", $"set output resolution (default {Dpi})", res => sDpi = res },
                { "o|output=", "set output dir/filename", o => OutputName = o },
                { "nobanner", "suppress the banner", h => noBanner = h != null },
                { "h|help", "show this message and exit", h => shouldShowHelp = h != null },
                { "man", "show the man page source and exit", h => shouldShowMan = h != null },
                { "colors", "list available colors by name", h => shouldShowColors = h != null },
                { "license", "show program license (AGPL 3.0)", h => shouldShowLicense = h != null }
            };
    }
    #endregion

    /// <summary>
    /// Sets base options in derived classes
    /// </summary>
    public void AddBaseOptions()
    {
        foreach (var opt in baseOptions)
        {
            Options.Add(opt);
        }
    }

    /// <summary>
    /// Does the dirty work
    /// </summary>
    /// <param name="args">command line arguments</param>
    /// <returns>true if nothing to (ie. help)</returns>
    public virtual bool Parse(string[] args)
    {
        try
        {
            FilesList = Options.Parse(args);
        }
        catch (OptionException e)
        {
            Console.Error.WriteLine($"{exeName}: {e.Message}");
            Console.Error.WriteLine($"Try '{exeName} --help' for more informations.");
            return true;
        }

        if (shouldShowMan)
        {
            Console.WriteLine(PrintMan());
            return true;
        }

        if (!noBanner) WelcomeBanner();

        if (shouldShowHelp)
        {
            Console.WriteLine($"Usage: {exeName} {Usage}");
            Console.WriteLine("\nOptions:");
            Options.WriteOptionDescriptions(Console.Out);
            Console.WriteLine();
            Console.WriteLine(ColorsSyntax);
            Console.WriteLine("\nEnvironment variables");
            Console.WriteLine(EnvVarsHelp);

            return true;
        }

        if (shouldShowColors)
        {
            Console.WriteLine("Available colors are:");
            foreach (var color in colors.colorDictionary)
            {
                Console.WriteLine("{0,-24}{1}", color.Key, color.Value.ToHexString());
            }
            return true;
        }

        if (shouldShowLicense)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream("Casasoft.CCDV.LICENSE"))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string result = reader.ReadToEnd();
                    Console.WriteLine("This program is released under:\n");
                    Console.WriteLine(result);
                }
            }
            return true;
        }

        if (!Path.IsPathRooted(OutputName))
        {
            outputDir = Environment.GetEnvironmentVariable("CDV_OUTPATH");
            if (!string.IsNullOrWhiteSpace(outputDir))
                OutputName = Path.Combine(outputDir, OutputName);
        }

        GetDPI();
        FillColor = GetColor(sFillColor);
        BorderColor = GetColor(sBorderColor);
        return false;
    }

    #region texts
    /// <summary>
    /// Prints the welcome banner
    /// </summary>
    public virtual void WelcomeBanner() =>
    Console.WriteLine(WelcomeBannerText());

    /// <summary>
    /// Text for welcome banner
    /// </summary>
    /// <returns></returns>
    public virtual string WelcomeBannerText() =>
        $"Casasoft Contemporary Carte de Visite {exeName}\nCopyright (c) 2020-2022 Roberto Ceccarelli - Casasoft\n";

    /// <summary>
    /// Colors syntax help
    /// </summary>
    protected string ColorsSyntax => @$"Colors can be written in any of these formats:
  #rgb
  #rrggbb
  #rrggbbaa
  #rrrrggggbbbb
  #rrrrggggbbbbaaaa
  colorname    (use {exeName} --colors  to see all available colors)";

    /// <summary>
    /// Environment varaibles help
    /// </summary>
    protected string EnvVarsHelp => @"The program can read values from these variables:
  CDV_OUTPATH  Base path for output files
  CDV_DPI      Resolution for output files
  CDV_FILL     Color used to fill images
  CDV_BORDER   Border color";

    /// <summary>
    /// Text of man page in markdown format
    /// </summary>
    /// <returns></returns>
    protected string PrintMan()
    {
        StringBuilder ret = new StringBuilder();
        ret.AppendLine(@$"% {exeName.ToUpper()}(1)  
% Roberto Ceccarelli - Casasoft  
% Jan 2022

# NAME
{exeName} - {exeDesc}

# SYNOPSIS
**{exeName}** {EscapeMarkdown(Usage)}");

        if (!string.IsNullOrWhiteSpace(LongDesc))
        {
            ret.AppendLine($"\n# DESCRIPTION\n{EscapeMarkdown(LongDesc)}");
        }

        ret.AppendLine("\n# OPTIONS");
        StringWriter sw = new StringWriter();
        Options.WriteOptionDescriptions(sw);
        string[] opts = sw.ToString().Split(
            new string[] { Environment.NewLine },
            StringSplitOptions.None);
        bool first = true;
        foreach (string s in opts)
        {
            if (string.IsNullOrWhiteSpace(s)) continue;

            string o = s.Substring(0, 29).Trim();
            if (string.IsNullOrWhiteSpace(o))
            {
                ret.Append(EscapeMarkdown(s.Trim()));
            }
            else
            {
                ret.Append(first ? "" : "\n\n");
                ret.Append($"**{o}**\n: {EscapeMarkdown(s.Substring(29).Trim())}");
            }
            first = false;
        }

        ret.Append($@"

## COLORS
{EscapeMarkdown(ColorsSyntax.Replace("\r", "  \r"))}

## ENVIRONMENT VARIABLES
{EscapeMarkdown(EnvVarsHelp.Replace("\r", "  \r"))}

# COPYRIGHT
Casasoft {exeName} is free software:  
you can redistribute it and/or modify it  
under the terms of the GNU Affero General Public License as published by  
the Free Software Foundation, either version 3 of the License, or  
\(at your option\) any later version.  

You should have received a copy of the GNU AGPL v.3  
along with Casasoft {exeName}.  
If not, see <http://www.gnu.org/licenses/>.  

# DISCLAIMER
Casasoft CCDV Tools is distributed in the hope that it will be useful,  
but WITHOUT ANY WARRANTY; without even the implied warranty of  
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.   
See the GNU General Public License for more details.");

        return ret.ToString();
    }
    #endregion

    #region internal utils
    /// <summary>
    /// Gets environment variables values
    /// </summary>
    protected void GetEnvVars()
    {
        string eDpi = Environment.GetEnvironmentVariable("CDV_DPI");
        if (!string.IsNullOrWhiteSpace(eDpi))
            Dpi = GetIntParameter(eDpi, Dpi, "Incorrect CDV_DPI environment variable value '{0}'.");

        string eFill = Environment.GetEnvironmentVariable("CDV_FILL");
        if (!string.IsNullOrWhiteSpace(eFill))
            sFillColor = eFill;

        string eBorder = Environment.GetEnvironmentVariable("CDV_BORDER");
        if (!string.IsNullOrWhiteSpace(eBorder))
            sFillColor = eBorder;
    }

    /// <summary>
    /// Gets string from filename.<br/>
    /// If the argument begins with '@' then the string is the name of the file containing the text
    /// </summary>
    /// <param name="p">parameter argument</param>
    /// <returns></returns>
    protected string GetFileParameter(string p)
    {
        if (!string.IsNullOrWhiteSpace(p) && p[0] == '@')
        {
            string ret = string.Empty;
            int l = p.Length;
            if (l < 2)
            {
                Console.Error.WriteLine("Missing filename for '@' parameter");
            }
            else
            {
                string filename = p.Substring(1);
                if (File.Exists(filename))
                {
                    ret = File.ReadAllText(filename);
                }
                else
                {
                    Console.Error.WriteLine($"File '{filename}' not found.");
                }
            }
            return ret;
        }
        else
        {
            return p;
        }
    }

    /// <summary>
    /// Get integer value from string
    /// </summary>
    /// <param name="val">input string</param>
    /// <param name="fallback">default value in case of parsing error</param>
    /// <param name="message">error message</param>
    /// <returns></returns>
    public static int GetIntParameter(string val, int fallback, string message)
    {
        int ret;
        if (!Int32.TryParse(val, out ret))
        {
            Console.Error.WriteLine(string.Format(message, val));
            ret = fallback;
        }
        return ret;
    }

    /// <summary>
    /// sets the dpi value from command line string
    /// </summary>
    protected void GetDPI()
    {
        if (!string.IsNullOrWhiteSpace(sDpi))
            Dpi = GetIntParameter(sDpi, Dpi, "Incorrect dpi value '{0}'. Using default value.");
    }


    /// <summary>
    /// Gets the color by a string
    /// </summary>
    /// <param name="color">name or components values</param>
    /// <returns><see cref="MagickColor"/></returns>
    protected MagickColor GetColor(string color)
    {
        MagickColor ret = MagickColors.Transparent;
        if (!string.IsNullOrWhiteSpace(color))
        {
            MagickColor r = colors.GetColor(color);
            if (r != null)
            {
                ret = r;
            }
            else
            {
                Console.Error.WriteLine($"Unknown color '{color}'\nTry {exeName} --colors");
            }
        }
        else
        {
            Console.Error.WriteLine("Invalid empty color");
        }
        return ret;
    }

    /// <summary>
    /// Windows shell does not expand wildcards
    /// </summary>
    public void ExpandWildcards()
    {
        List<string> files = new();
        foreach (string filename in FilesList)
        {
            files.AddRange(Directory.GetFiles(Path.GetDirectoryName(filename), Path.GetFileName(filename)).ToList());
        }
        FilesList.Clear();
        FilesList.AddRange(files);
    }

    /// <summary>
    /// Escape markdown special characters
    /// </summary>
    /// <param name="s">string to process</param>
    /// <returns>escaped string</returns>
    public string EscapeMarkdown(string s) =>
        s.Replace("\\", "\\\\")
        .Replace("#", "\\#")
        .Replace("*", "\\*")
        .Replace("_", "\\_")
        .Replace("(", "\\(")
        .Replace(")", "\\)")
        .Replace("[", "\\[")
        .Replace("]", "\\]")
        .Replace("{", "\\{")
        .Replace("}", "\\}");
    #endregion
}
