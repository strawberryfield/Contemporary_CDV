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
        public int DPI => _dpi;

        #region commercial formats
        public MagickGeometry InCartha20x27_o => new(ToPixels(270), ToPixels(200));
        public MagickGeometry InCartha20x27_v => swap(InCartha20x27_o);
        public MagickGeometry FineArt10x15_o => new(ToPixels(152), ToPixels(102));
        public MagickGeometry FineArt10x15_v => swap(FineArt10x15_o);
        public MagickGeometry FineArt10x18_o => new(ToPixels(180), ToPixels(102));
        public MagickGeometry FineArt10x18_v => swap(FineArt10x18_o);
        #endregion

        #region cdv
        public MagickGeometry CDV_Full_o => new(ToPixels(100), ToPixels(64));
        public MagickGeometry CDV_Full_v => swap(CDV_Full_o);
        public MagickGeometry CDV_Internal_o => new(ToPixels(90), ToPixels(54));
        public MagickGeometry CDV_Internal_v => swap(CDV_Internal_o);
        #endregion

        private MagickGeometry swap(MagickGeometry g)
        {
            int tmp = g.Width;
            g.Width = g.Height;
            g.Height = tmp;
            return g;
        }

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
