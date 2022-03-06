// copyright (c) 2020-2022 Roberto Ceccarelli - Casasoft
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
using System;
using System.IO;
using System.Reflection;

namespace Casasoft.CCDV;

/// <summary>
/// Collection of Image Magick utilities
/// </summary>
public static class Utils
{
    #region image resize
    /// <summary>
    /// Rotates an image according to the given geometry
    /// </summary>
    /// <param name="img">image to rotate</param>
    /// <param name="size">reference geometry</param>
    /// <returns>rotated image</returns>
    public static MagickImage AutoRotate(MagickImage img, MagickGeometry size)
    {
        MagickImage i = (MagickImage)img.Clone();
        if (size.Height > size.Width)
        {
            // output must be portrait
            if (img.Height < img.Width) 
                i.Rotate(-90);
        }
        else
        {
            // output must be landscape
            if (img.Height > img.Width)
                i.Rotate(-90);
        }
        return i;
    }

    /// <summary>
    /// Resizes an image to the given geometry.<br/>
    /// Empty space is filled with the given color
    /// </summary>
    /// <param name="img">Image to resize</param>
    /// <param name="size">target size</param>
    /// <param name="fill">color to fill the empty space</param>
    /// <returns>Image resized and filled</returns>
    public static MagickImage ResizeAndFill(MagickImage img, MagickGeometry size, MagickColor fill)
    {
        MagickImage i = (MagickImage)img.Clone();
        i.Resize(size);
        i.Extent(size, Gravity.Center, fill);
        return i;
    }
    /// <summary>
    /// Resizes an image to the given geometry.<br/>
    /// Empty space is filled with white
    /// </summary>
    /// <param name="img">Image to resize</param>
    /// <param name="size">target size</param>
    /// <returns>Image resized and filled</returns>
    public static MagickImage ResizeAndFill(MagickImage img, MagickGeometry size) =>
        ResizeAndFill(img, size, MagickColors.White);

    /// <summary>
    /// Resizes an image to the given geometry.<br/>
    /// Before resizing the image is rotated with <see cref="AutoRotate"/>.<br/>
    /// Empty space is filled with the given color.
    /// </summary>
    /// <param name="img">Image to process</param>
    /// <param name="size">reference size and orientation</param>
    /// <param name="fill">fill color</param>
    /// <returns>processed image</returns>
    public static MagickImage RotateResizeAndFill(MagickImage img, MagickGeometry size, MagickColor fill) =>
        ResizeAndFill(AutoRotate(img, size), size, fill);
    /// <summary>
    /// Resizes an image to the size of another image.<br/>
    /// Before resizing the image is rotated with <see cref="AutoRotate"/>.<br/>
    /// Empty space is filled with the given color.
    /// </summary>
    /// <param name="img">Image to process</param>
    /// <param name="size">reference size and orientation</param>
    /// <param name="fill">fill color</param>
    /// <returns>processed image</returns>
    public static MagickImage RotateResizeAndFill(MagickImage img, MagickImage size, MagickColor fill) =>
        RotateResizeAndFill(img, new MagickGeometry(size.Width, size.Height), fill);
    /// <summary>
    /// Resizes an image to the given geometry.
    /// Before resizing the image is rotated with <see cref="AutoRotate"/>
    /// Empty space is filled with white color.
    /// </summary>
    /// <param name="img">Image to process</param>
    /// <param name="size">reference size and orientation</param>
    /// <returns>processed image</returns>
    public static MagickImage RotateResizeAndFill(MagickImage img, MagickGeometry size) =>
        RotateResizeAndFill(img, size, MagickColors.White);
    /// <summary>
    /// Resizes an image to the size of another image.<br/>
    /// Before resizing the image is rotated with <see cref="AutoRotate"/>.<br/>
    /// Empty space is filled with white color.
    /// </summary>
    /// <param name="img">Image to process</param>
    /// <param name="size">reference size and orientation</param>
    /// <returns>processed image</returns>
    public static MagickImage RotateResizeAndFill(MagickImage img, MagickImage size) =>
        RotateResizeAndFill(img, size, MagickColors.White);

    #endregion

    #region text
    /// <summary>
    /// Renders text centered to the given size
    /// </summary>
    /// <param name="text">text to render</param>
    /// <param name="size">font size</param>
    /// <param name="width">render area width</param>
    /// <param name="height">render area height</param>
    /// <param name="font">font</param>
    /// <param name="rotation">text rotation</param>
    /// <param name="fontBold">use bold font (if available)</param>
    /// <param name="fontItalic">use bold font (if available)</param>
    /// <returns></returns>
    public static Drawables CenteredText(string text, int size, int width, int height,
        string font = "", int rotation = 0, bool fontBold = false, bool fontItalic = false)
    {
        Drawables draw = new();
        draw.Rotation(rotation);
        if (!string.IsNullOrWhiteSpace(font)) draw.Font(font,
             fontItalic ? FontStyleType.Italic : FontStyleType.Normal,
             fontBold ? FontWeight.Bold : FontWeight.Normal,
             FontStretch.Normal);
        draw.StrokeColor(MagickColors.Black).FontPointSize(size).TextAlignment(TextAlignment.Center);
        if (Math.Abs(rotation) != 90)
            draw.Text(width / 2, height / 2, text);
        else
            draw.Text(Math.Sign(rotation) * height / 2, width / 2, text);

        return draw;
    }

    /// <summary>
    /// Renders text centered to the given geometry
    /// </summary>
    /// <param name="text">text to render</param>
    /// <param name="size">font size</param>
    /// <param name="fmt">reference geometry</param>
    /// <param name="font">font</param>
    /// <param name="rotation">text rotation</param>
    /// <param name="fontBold">use bold font (if available)</param>
    /// <param name="fontItalic">use bold font (if available)</param>
    /// <returns></returns>
    public static Drawables CenteredText(string text, int size, MagickGeometry fmt,
        string font = "", int rotation = 0, bool fontBold = false, bool fontItalic = false) =>
        CenteredText(text, size, fmt.Width, fmt.Height, font, rotation, fontBold, fontItalic);
    /// <summary>
    /// Renders text centered to the given reference image
    /// </summary>
    /// <param name="text">text to render</param>
    /// <param name="size">font size</param>
    /// <param name="fmt">reference image</param>
    /// <param name="font">font</param>
    /// <param name="rotation">text rotation</param>
    /// <param name="fontBold">use bold font (if available)</param>
    /// <param name="fontItalic">use bold font (if available)</param>
    /// <returns></returns>
    public static Drawables CenteredText(string text, int size, MagickImage fmt,
        string font = "", int rotation = 0, bool fontBold = false, bool fontItalic = false) =>
        CenteredText(text, size, fmt.Width, fmt.Height, font, rotation, fontBold, fontItalic);
    #endregion

    #region lines
    /// <summary>
    /// Draws an horizontal line from 0 to width
    /// </summary>
    /// <param name="draw"></param>
    /// <param name="h">Distance from top border</param>
    /// <param name="width"></param>
    public static void HLine(Drawables draw, int h, int width) => draw.Line(0, h, width, h);
    /// <summary>
    /// Draws a vertical line from 0 to height
    /// </summary>
    /// <param name="draw"></param>
    /// <param name="l">Distance from left border</param>
    /// <param name="height"></param>
    public static void VLine(Drawables draw, int l, int height) => draw.Line(l, 0, l, height);
    #endregion

    #region paper formats
    public static PaperFormats GetThickFormat(string Paper)
    {
        PaperFormats ret = PaperFormats.Large;
        if (!string.IsNullOrEmpty(Paper))
        {
            if (Paper.ToUpper() == "MEDIUM")
            {
                ret = PaperFormats.Medium;
            }
        }
        return ret;
    }
    #endregion

    /// <summary>
    /// Returns the text of the license
    /// </summary>
    /// <returns></returns>
    public static string GetLicense()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        using (Stream stream = assembly.GetManifestResourceStream("Casasoft.CCDV.LICENSE"))
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
           }
        }
    }
}
