﻿// copyright (c) 2020-2021 Roberto Ceccarelli - Casasoft
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

namespace Casasoft.CCDV
{
    public class BaseBuilderCommandLine : CommandLine
    {
        public int thickness = 5;
        public string topImage = string.Empty;
        public string bottomImage = string.Empty;
        public string frontImage = string.Empty;
        public string backImage = string.Empty;
        public string leftImage = string.Empty;
        public string rightImage = string.Empty;

        private string sThickness = "5";
        public bool isHorizontal { get; set; }
        public bool useSampleImages { get; set; }

        public BaseBuilderCommandLine(string outputname) : base(outputname) { }
        public BaseBuilderCommandLine(string exename, string outputname) :
            base(exename, outputname)
        {
            Options = new OptionSet
            {
                { "a|aboveimage=", "set the image for the top cover", i => topImage = i },
                { "z|bottomimage=", "set the image for the bottom", i => bottomImage = i },
                { "l|leftimage=", "set the image for the left border", i => leftImage = i },
                { "r|rightimage=", "set the image for the right border", i => rightImage = i },
                { "f|frontimage=", "set the image for the front", i => frontImage = i },
                { "b|backimage=", "set the image for the back", i => backImage = i },
                { "t|thickness=", $"set the box thickness (default {sThickness}mm)", t => sThickness = t },
                { "horizontal", "configure box in horizontal mode", o => isHorizontal = o != null },
                { "sample", "generate sample images", s => useSampleImages = s != null },
            };
            AddBaseOptions();
        }

        public override bool Parse(string[] args)
        {
            if (base.Parse(args)) return true;

            thickness = GetIntParameter(sThickness, thickness,
                $"Incorrect thickness value '{sThickness}'. Using default value.");
            return false;
        }
    }
}
