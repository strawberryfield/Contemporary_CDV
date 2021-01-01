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
using System.IO;

namespace Casasoft.CCDV
{
    public class BaseBuilder
    {
        protected int spessore;
        protected Formats fmt;
        protected MagickColor fillColor;
        protected MagickColor borderColor;


        #region constructors
        public BaseBuilder(int Spessore, int dpi) :
            this(Spessore, new Formats(dpi))
        { }
        public BaseBuilder(int Spessore, Formats formats) :
            this(Spessore, formats, MagickColors.White, MagickColors.Black)
        { }
        public BaseBuilder(int Spessore, int dpi, MagickColor fillcolor) :
            this(Spessore, new Formats(dpi), fillcolor, MagickColors.Black)
        { }
        public BaseBuilder(int Spessore, int dpi, MagickColor fillcolor, MagickColor bordercolor) :
            this(Spessore, new Formats(dpi), fillcolor, bordercolor)
        { }

        public BaseBuilder(int Spessore, Formats formats, MagickColor fillcolor, MagickColor bordercolor)
        {
            fmt = formats;
            spessore = fmt.ToPixels(Spessore);
            fillColor = fillcolor;
            borderColor = bordercolor;
            makeEmptyImages();
        }

        public BaseBuilder(int Spessore) : this(Spessore, 300) { }
        public BaseBuilder() : this(5) { }
        #endregion

        protected virtual void makeEmptyImages() { }

        protected MagickImage checkAndLoad(string filename, MagickImage template) =>
            (!string.IsNullOrWhiteSpace(filename) && File.Exists(filename)) ?
            Utils.RotateResizeAndFill(new(filename), template, fillColor) : template;
    }
}
