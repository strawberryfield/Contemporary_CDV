// copyright (c) 2020 Roberto Ceccarelli - Casasoft
// http://strawberryfield.altervista.org 
// 
// This file is part of Casasoft Contemporary Carte de Visite Tools
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
            shouldShowHelp = false;
            FillColor = GetColor(sFillColor);
            BorderColor = GetColor(sBorderColor);

            Options = new();
            baseOptions = new OptionSet
            {
                { "fillcolor=", $"set the color used to fiil the images\n(default {sFillColor})", c => sFillColor = c },
                { "bordercolor=", $"set the color used to border the images\n(default {sBorderColor})", c => sBorderColor = c },
                { "dpi=", $"set output resolution (default {sDpi})", res => sDpi = res },
                { "o|output=", "set output dir/filename", o => OutputName = o },
                { "h|help", "show this message and exit", h => shouldShowHelp = h != null },
            };
        }
        #endregion

        public void WelcomeBanner() =>
            Console.Error.WriteLine($"Casasoft Contemporary Carte de Visite {exeName}\nCopyright (c) 2020 Roberto Ceccarelli - Casasoft\n");

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

            if (shouldShowHelp)
            {
                Console.WriteLine($"Usage: {exeName} {Usage}");
                Console.WriteLine();

                Console.WriteLine("Options:");
                Options.WriteOptionDescriptions(Console.Out);
                return true;
            }

            Dpi = GetIntParameter(sDpi, Dpi, "Incorrect dpi value '{0}'. Using the default value.");
            FillColor = GetColor(sFillColor);
            return false;
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

        protected void GetDPI() =>
            Dpi = GetIntParameter(sDpi, Dpi, "Incorrect dpi value '{0}'. Using default value.");

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
