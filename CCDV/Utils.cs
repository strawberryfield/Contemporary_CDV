// copyright (c) 2020-2023 Roberto Ceccarelli - Casasoft
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
using System.Windows.Media;

namespace Casasoft.CCDV.UI;

public static class Utils
{
    public static MagickColor ColorFromPicker(ColorPicker.PortableColorPicker cp) =>
    new MagickColor((ushort)(cp.SelectedColor.R * 257),
        (ushort)(cp.SelectedColor.G * 257),
        (ushort)(cp.SelectedColor.B * 257),
        (ushort)(cp.SelectedColor.A * 257));

    public static Color ColorFromMagick(MagickColor magickColor)
    {
        Color ret = new();
        ret.R = (byte)magickColor.R;
        ret.G = (byte)magickColor.G;
        ret.B = (byte)magickColor.B;
        ret.A = (byte)magickColor.A;
        return ret;
    }
}
