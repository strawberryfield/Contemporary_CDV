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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Casasoft.CCDV
{
    public class CommandLine
    {
        protected bool shouldShowHelp { get; set; }
        protected string exeName { get; set; }
        protected OptionSet baseOptions { get; set; }
        protected bool noBanner { get; set; }
        protected string outputDir { get; private set; }

        #region properties
        public string OutputName { get; set; }
        public int Dpi { get; set; }
        public List<string> FilesList { get; set; }
        public OptionSet Options { get; set; }
        public string Usage { get; set; }
        public MagickColor FillColor { get; set; }
        public MagickColor BorderColor { get; set; }
        #endregion

        #region defaults
        private string sDpi = "300";
        private string sFillColor = "#FFFFFF";
        private string sBorderColor = "#000000";
        #endregion

        #region constructors
        public static string ExeName() => Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName);

        public CommandLine(string outputname) :
            this(ExeName(), outputname)
        { }
        public CommandLine(string exename, string outputname)
        {
            exeName = exename;
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
            };
        }
        #endregion

        public virtual void WelcomeBanner() =>
            Console.WriteLine(WelcomeBannerText());

        public virtual string WelcomeBannerText() =>
            $"Casasoft Contemporary Carte de Visite {exeName}\nCopyright (c) 2020-2021 Roberto Ceccarelli - Casasoft\n";

        public void AddBaseOptions()
        {
            foreach (var opt in baseOptions)
            {
                Options.Add(opt);
            }
        }

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

            if (!noBanner) WelcomeBanner();

            if (shouldShowHelp)
            {
                Console.WriteLine($"Usage: {exeName} {Usage}");
                Console.WriteLine("\nOptions:");
                Options.WriteOptionDescriptions(Console.Out);
                Console.WriteLine();
                Console.WriteLine(@"Colors can be written in any of these formats:
  #rgb
  #rrggbb
  #rrggbbaa
  #rrrrggggbbbb
  #rrrrggggbbbbaaaa");
                Console.WriteLine("\nEnvironment variables");
                Console.WriteLine(@"The program can read values from these variables:
  CDV_OUTPATH  Base path for output files
  CDV_DPI      Resolution for output files
  CDV_FILL     Color used to fill images
  CDV_BORDER   Border color");

                return true;
            }

            if(!Path.IsPathRooted(OutputName))
            {
                outputDir = Environment.GetEnvironmentVariable("CDV_OUTPATH");
                if (!string.IsNullOrWhiteSpace(outputDir))
                    OutputName = Path.Combine(outputDir, OutputName);
            }

            GetDPI();
            FillColor = GetColor(sFillColor);
            return false;
        }

        protected void GetEnvVars()
        {
            string eDpi = Environment.GetEnvironmentVariable("CDV_DPI");
            if (!string.IsNullOrWhiteSpace(eDpi))
                Dpi = GetIntParameter(eDpi, Dpi, "Incorrect CDV_DPI environment variable value '{0}'.");

            string eFill = Environment.GetEnvironmentVariable("CDV_FILL");
            if(!string.IsNullOrWhiteSpace(eFill))
                sFillColor = eFill;

            string eBorder = Environment.GetEnvironmentVariable("CDV_BORDER");
            if (!string.IsNullOrWhiteSpace(eBorder))
                sFillColor = eBorder;
        }

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

        protected void GetDPI()
        {
            if(!string.IsNullOrWhiteSpace(sDpi))
                Dpi = GetIntParameter(sDpi, Dpi, "Incorrect dpi value '{0}'. Using default value.");
        }

        protected static MagickColor GetColor(string color) => new MagickColor(color);

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
    }
}
