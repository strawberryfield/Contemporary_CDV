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
using System;

namespace Casasoft.CCDV
{
    public static class Utils
    {
        public static MagickImage ResizeAndFill(MagickImage img, MagickGeometry size, MagickColor fill)
        {
            MagickImage i = (MagickImage)img.Clone();
            i.Resize(size);
            i.Extent(size, Gravity.Center, fill);
            return i;
        }
        public static MagickImage ResizeAndFill(MagickImage img, MagickGeometry size) =>
            ResizeAndFill(img, size, MagickColors.White);

        public static void WelcomeBanner(string exeName) => 
            Console.Error.WriteLine($"Casasoft Contemporary Carte de Visite {exeName}\nCopyright (c) 2020 Roberto Ceccarelli - Casasoft\n");
    }
}
