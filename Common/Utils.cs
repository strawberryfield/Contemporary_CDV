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

namespace Casasoft.CCDV
{
    public static class Utils
    {
        #region image resize
        public static MagickImage AutoRotate(MagickImage img, MagickGeometry size)
        {
            if (size.Height > size.Width)
            {
                // output must be portrait
                if (img.Height < img.Width)
                    img.Rotate(-90);
            }
            else
            {
                // output must be landscape
                if (img.Height > img.Width)
                    img.Rotate(90);
            }
            return img;
        }

        public static MagickImage ResizeAndFill(MagickImage img, MagickGeometry size, MagickColor fill)
        {
            MagickImage i = (MagickImage)img.Clone();
            i.Resize(size);
            i.Extent(size, Gravity.Center, fill);
            return i;
        }
        public static MagickImage ResizeAndFill(MagickImage img, MagickGeometry size) =>
            ResizeAndFill(img, size, MagickColors.White);

        public static MagickImage RotateResizeAndFill(MagickImage img, MagickGeometry size, MagickColor fill) =>
            ResizeAndFill(AutoRotate(img, size), size, fill);
        public static MagickImage RotateResizeAndFill(MagickImage img, MagickImage size, MagickColor fill) =>
            RotateResizeAndFill(img, new MagickGeometry(size.Width, size.Height), fill);
        public static MagickImage RotateResizeAndFill(MagickImage img, MagickGeometry size) =>
            RotateResizeAndFill(img, size, MagickColors.White);
        public static MagickImage RotateResizeAndFill(MagickImage img, MagickImage size) =>
            RotateResizeAndFill(img, size, MagickColors.White);

        #endregion

        #region text
        public static Drawables CenteredText(string text, int size, int width, int height)
        {
            Drawables draw = new();
            draw.StrokeColor(MagickColors.Black).FontPointSize(size).TextAlignment(TextAlignment.Center);
            draw.Text(width / 2, height / 2, text);
            return draw;
        }

        public static Drawables CenteredText(string text, int size, MagickGeometry fmt) =>
            CenteredText(text, size, fmt.Width, fmt.Height);
        public static Drawables CenteredText(string text, int size, MagickImage fmt) =>
            CenteredText(text, size, fmt.Width, fmt.Height);
        #endregion
    }
}
