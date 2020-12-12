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
    public class Images
    {
        protected Formats fmt;

        public Images(int dpi)
        {
            fmt = new(dpi);
        }

        public Images() : this(300) { }

        public Images(Formats f)
        {
            fmt = f;
        }

        public static string ColorWhite => "#FFFFFF";
        public static string ColorBlack => "#000";

        public MagickImage InCartha20x27_o(string c) => new(new MagickColor(c), fmt.InCartha20x27_o.Width, fmt.InCartha20x27_o.Height);
        public MagickImage InCartha20x27_v(string c) => new(new MagickColor(c), fmt.InCartha20x27_v.Width, fmt.InCartha20x27_v.Height);
        public MagickImage FineArt10x15_o(string c) => new(new MagickColor(c), fmt.FineArt10x15_o.Width, fmt.FineArt10x15_o.Height);
        public MagickImage FineArt10x15_v(string c) => new(new MagickColor(c), fmt.FineArt10x15_v.Width, fmt.FineArt10x15_v.Height);

        public MagickImage CDV_Full_o(string c) => new(new MagickColor(c), fmt.CDV_Full_o.Width, fmt.CDV_Full_o.Height);
        public MagickImage CDV_Full_v(string c) => new(new MagickColor(c), fmt.CDV_Full_v.Width, fmt.CDV_Full_v.Height);
        public MagickImage CDV_Internal_o(string c) => new(new MagickColor(c), fmt.CDV_Internal_o.Width, fmt.CDV_Internal_o.Height);
        public MagickImage CDV_Internal_v(string c) => new(new MagickColor(c), fmt.CDV_Internal_v.Width, fmt.CDV_Internal_v.Height);

        public MagickImage InCartha20x27_o() => InCartha20x27_o(ColorWhite);
        public MagickImage InCartha20x27_v() => InCartha20x27_v(ColorWhite);
        public MagickImage FineArt10x15_o() => FineArt10x15_o(ColorWhite);
        public MagickImage FineArt10x15_v() => FineArt10x15_v(ColorWhite);


        public MagickImage CDV_Full_o() => CDV_Full_o(ColorWhite);
        public MagickImage CDV_Full_v() => CDV_Full_v(ColorWhite);
        public MagickImage CDV_Internal_o() => CDV_Internal_o(ColorWhite);
        public MagickImage CDV_Internal_v() => CDV_Internal_v(ColorWhite);

    }
}
