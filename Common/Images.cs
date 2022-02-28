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

namespace Casasoft.CCDV;

/// <summary>
/// This class generates Imagemagick images
/// based on format defined with <see cref="Formats"/> class
/// </summary>
public class Images
{
    /// <summary>
    /// Options for images generation
    /// </summary>
    protected Formats fmt;

    #region constructors
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dpi">Resolution of images</param>
    public Images(int dpi)
    {
        fmt = new(dpi);
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <remarks>
    /// The resolution is set to the default 300 DPI
    /// </remarks>
    public Images() : this(300) { }

    /// <summary>
    /// Construcot
    /// </summary>
    /// <param name="f">
    /// Instance of <see cref="Formats"/> class used to set the 
    /// resolution of the images
    /// </param>
    public Images(Formats f)
    {
        fmt = f;
    }
    #endregion

    #region commercial formats
    /// <summary>
    /// Photocity Digital print over 27x20cm paper
    /// </summary>
    /// <param name="c">Background color</param>
    /// <returns></returns>
    public MagickImage InCartha20x27_o(MagickColor c) => new(c, fmt.InCartha20x27_o.Width, fmt.InCartha20x27_o.Height);
    /// <summary>
    /// Photocity Digital print over 20x27cm paper
    /// </summary>
    /// <param name="c">Background color</param>
    /// <returns></returns>
    public MagickImage InCartha20x27_v(MagickColor c) => new(c, fmt.InCartha20x27_v.Width, fmt.InCartha20x27_v.Height);
    /// <summary>
    /// Photocity Digital print over 15x20cm paper
    /// </summary>
    /// <param name="c">Background color</param>
    /// <returns></returns>
    public MagickImage InCartha15x20_o(MagickColor c) => new(c, fmt.InCartha15x20_o.Width, fmt.InCartha15x20_o.Height);
    /// <summary>
    /// Photocity Digital print over 20x27cm paper
    /// </summary>
    /// <param name="c">Background color</param>
    /// <returns></returns>
    public MagickImage InCartha15x20_v(MagickColor c) => new(c, fmt.InCartha15x20_v.Width, fmt.InCartha15x20_v.Height);
    /// <summary>
    /// 15x10cm paper
    /// </summary>
    /// <param name="c">Background color</param>
    /// <returns></returns>
    public MagickImage FineArt10x15_o(MagickColor c) => new(c, fmt.FineArt10x15_o.Width, fmt.FineArt10x15_o.Height);
    /// <summary>
    /// 10x15cm paper
    /// </summary>
    /// <param name="c">Background color</param>
    /// <returns></returns>
    public MagickImage FineArt10x15_v(MagickColor c) => new(c, fmt.FineArt10x15_v.Width, fmt.FineArt10x15_v.Height);
    /// <summary>
    /// 18x10cm paper
    /// </summary>
    /// <param name="c">Background color</param>
    /// <returns></returns>
    public MagickImage FineArt10x18_o(MagickColor c) => new(c, fmt.FineArt10x18_o.Width, fmt.FineArt10x18_o.Height);
    /// <summary>
    /// 10x18cm paper
    /// </summary>
    /// <param name="c">Background color</param>
    /// <returns></returns>
    public MagickImage FineArt10x18_v(MagickColor c) => new(c, fmt.FineArt10x18_v.Width, fmt.FineArt10x18_v.Height);

    /// <summary>
    /// Photocity Digital print over 27x20cm paper
    /// </summary>
    /// <remarks>White background</remarks>
    /// <returns></returns>
    public MagickImage InCartha20x27_o() => InCartha20x27_o(MagickColors.White);
    /// <summary>
    /// Photocity Digital print over 20x27cm paper
    /// </summary>
    /// <remarks>White background</remarks>
    /// <returns></returns>
    public MagickImage InCartha20x27_v() => InCartha20x27_v(MagickColors.White);
    /// <summary>
    /// Photocity Digital print over 15x20cm paper
    /// </summary>
    /// <remarks>White background</remarks>
    /// <returns></returns>
    public MagickImage InCartha15x20_o() => InCartha15x20_o(MagickColors.White);
    /// <summary>
    /// Photocity Digital print over 15x20cm paper
    /// </summary>
    /// <remarks>White background</remarks>
    /// <returns></returns>
    public MagickImage InCartha15x20_v() => InCartha15x20_v(MagickColors.White);
    /// <summary>
    /// 15x10cm paper
    /// </summary>
    /// <remarks>White background</remarks>
    /// <returns></returns>
    public MagickImage FineArt10x15_o() => FineArt10x15_o(MagickColors.White);
    /// <summary>
    /// 10x15cm paper
    /// </summary>
    /// <remarks>White background</remarks>
    /// <returns></returns>
    public MagickImage FineArt10x15_v() => FineArt10x15_v(MagickColors.White);
    /// <summary>
    /// 18x10cm paper
    /// </summary>
    /// <remarks>White background</remarks>
    /// <returns></returns>
    public MagickImage FineArt10x18_o() => FineArt10x18_o(MagickColors.White);
    /// <summary>
    /// 10x18cm paper
    /// </summary>
    /// <remarks>White background</remarks>
    /// <returns></returns>
    public MagickImage FineArt10x18_v() => FineArt10x18_v(MagickColors.White);
    #endregion

    #region cdv
    /// <summary>
    /// Horizontal Carte de Visite, 100x64mm
    /// </summary>
    /// <param name="c">Background color</param>
    /// <returns></returns>
    public MagickImage CDV_Full_o(MagickColor c) => new(c, fmt.CDV_Full_o.Width, fmt.CDV_Full_o.Height);
    /// <summary>
    /// Vertical Carte de Visite, 64x100mm
    /// </summary>
    /// <param name="c">Background color</param>
    /// <returns></returns>
    public MagickImage CDV_Full_v(MagickColor c) => new(c, fmt.CDV_Full_v.Width, fmt.CDV_Full_v.Height);
    /// <summary>
    /// Horizontal Carte de Visite reduced to leave a 5 mm border on any side
    /// </summary>
    /// <param name="c">Background color</param>
    /// <returns></returns>
    public MagickImage CDV_Internal_o(MagickColor c) => new(c, fmt.CDV_Internal_o.Width, fmt.CDV_Internal_o.Height);
    /// <summary>
    /// Vertical Carte de Visite reduced to leave a 5 mm border on any side
    /// </summary>
    /// <param name="c">Background color</param>
    /// <returns></returns>
    public MagickImage CDV_Internal_v(MagickColor c) => new(c, fmt.CDV_Internal_v.Width, fmt.CDV_Internal_v.Height);

    /// <summary>
    /// Horizontal Carte de Visite, 100x64mm
    /// </summary>
    /// <remarks>White background</remarks>
    /// <returns></returns>
    public MagickImage CDV_Full_o() => CDV_Full_o(MagickColors.White);
    /// <summary>
    /// Vertical Carte de Visite, 64x100mm
    /// </summary>
    /// <remarks>White background</remarks>
    /// <returns></returns>
    public MagickImage CDV_Full_v() => CDV_Full_v(MagickColors.White);
    /// <summary>
    /// Horizontal Carte de Visite reduced to leave a 5 mm border on any side
    /// </summary>
    /// <remarks>White background</remarks>
    /// <returns></returns>
    public MagickImage CDV_Internal_o() => CDV_Internal_o(MagickColors.White);
    /// <summary>
    /// Vertical Carte de Visite reduced to leave a 5 mm border on any side
    /// </summary>
    /// <remarks>White background</remarks>
    /// <returns></returns>
    public MagickImage CDV_Internal_v() => CDV_Internal_v(MagickColors.White);
    #endregion

    #region credit card
    /// <summary>
    /// Horizontal credit card, 86x54mm
    /// </summary>
    /// <param name="c">Background color</param>
    /// <returns></returns>
    public MagickImage CC_o(MagickColor c) => new(c, fmt.CC_o.Width, fmt.CC_o.Height);
    /// <summary>
    /// Vertical credit card, 54x86mm
    /// </summary>
    /// <param name="c">Background color</param>
    /// <returns></returns>
    public MagickImage CC_v(MagickColor c) => new(c, fmt.CC_v.Width, fmt.CC_v.Height);

    /// <summary>
    /// Horizontal credit card, 86x54mm
    /// </summary>
    /// <remarks>White background</remarks>
    /// <returns></returns>
    public MagickImage CC_o() => CC_o(MagickColors.White);
    /// <summary>
    /// Vertical credit card, 54x86mm
    /// </summary>
    /// <remarks>White background</remarks>
    /// <returns></returns>
    public MagickImage CC_v() => CC_v(MagickColors.White);

    #endregion

    /// <summary>
    /// Draws infos on the image
    /// </summary>
    /// <param name="i">input description</param>
    /// <param name="o">output description</param>
    /// <returns></returns>
    public Drawables Info(string i, string o)
    {
        Drawables d = new Drawables();

        d.FontPointSize(fmt.ToPixels(3))
            .Font("Arial")
            .FillColor(MagickColors.Black)
            .TextAlignment(TextAlignment.Left);

        d.Text(fmt.ToPixels(10), fmt.ToPixels(185), $"{i}Run {DateTime.Now.ToString("R")}")
            .Text(fmt.InCartha20x27_o.Width / 2, fmt.ToPixels(185), $"DPI: {fmt.DPI}\nOutput: {o}");

        return d;
    }

}
