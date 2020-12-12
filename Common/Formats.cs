// copyright (c) 2020 Roberto Ceccarelli - CasaSoft
// http://strawberryfield.altervista.org 
// 
// This file is part of CasaSoft Contemporary Carte de Visite Tools
// 
// CasaSoft CCDV Tools is free software: 
// you can redistribute it and/or modify it
// under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// CasaSoft CCDV Tools is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
// See the GNU General Public License for more details.
// 
// You should have received a copy of the GNU AGPL v.3
// along with CasaSoft CCDV Tools.  
// If not, see <http://www.gnu.org/licenses/>.

using ImageMagick;

namespace Casasoft.CCDV
{
    public class Formats
    {
        private int _dpi;
        private double _inch = 25.4;

        public Formats() : this(300)
        {
        }

        public Formats(int dpi) 
        {
            _dpi = dpi;
        }

        public int ToPixels(int mm) => (int)(mm * _dpi / _inch);

        public MagickGeometry InCartha20x27_o => new(ToPixels(270 - 6), ToPixels(196 - 6));
        public MagickGeometry InCartha20x27_v => new(ToPixels(196 - 6), ToPixels(270 - 6));
        public MagickGeometry FineArt10x15_o => new(ToPixels(152), ToPixels(102));
        public MagickGeometry FineArt10x15_v => new(ToPixels(102), ToPixels(152));

        public MagickGeometry CDV_Full_o => new(ToPixels(100), ToPixels(65));
        public MagickGeometry CDV_Full_v => new(ToPixels(65), ToPixels(100));
        public MagickGeometry CDV_Internal_o => new(ToPixels(90), ToPixels(55));
        public MagickGeometry CDV_Internal_v => new(ToPixels(55), ToPixels(90));
    }
}
