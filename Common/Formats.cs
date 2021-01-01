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

namespace Casasoft.CCDV
{
    public class Formats
    {
        private int _dpi;
        private double _inch = 25.4;

        #region constructors
        public Formats() : this(300) { }

        public Formats(int dpi) 
        {
            _dpi = dpi;
        }
        #endregion

        public int ToPixels(int mm) => (int)(mm * _dpi / _inch);

        #region commercial formats
        public MagickGeometry InCartha20x27_o => new(ToPixels(270 - 6), ToPixels(196 - 6));
        public MagickGeometry InCartha20x27_v => new(ToPixels(196 - 6), ToPixels(270 - 6));
        public MagickGeometry FineArt10x15_o => new(ToPixels(152), ToPixels(102));
        public MagickGeometry FineArt10x15_v => new(ToPixels(102), ToPixels(152));
        public MagickGeometry FineArt10x18_o => new(ToPixels(180), ToPixels(102));
        public MagickGeometry FineArt10x18_v => new(ToPixels(102), ToPixels(180));
        #endregion

        #region cdv
        public MagickGeometry CDV_Full_o => new(ToPixels(100), ToPixels(65));
        public MagickGeometry CDV_Full_v => new(ToPixels(65), ToPixels(100));
        public MagickGeometry CDV_Internal_o => new(ToPixels(90), ToPixels(55));
        public MagickGeometry CDV_Internal_v => new(ToPixels(55), ToPixels(90));
        #endregion

        public void SetImageParameters(MagickImage img)
        {
            img.Format = MagickFormat.Jpg;
            img.Quality = 95;
            img.Density = new Density(_dpi);
            img.ColorSpace = ColorSpace.sRGB;

            ExifProfile exif = new();
            exif.SetValue(ExifTag.Make, "Casasoft");
            exif.SetValue(ExifTag.Model, "Contemporary Carte de Visite Tools");
            exif.SetValue(ExifTag.Software, "Casasoft Contemporary Carte de Visite Tools");
            img.SetProfile(exif);
        }
    }
}
