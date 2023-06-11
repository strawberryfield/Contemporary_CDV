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

namespace Casasoft.CCDV;

/// <summary>
/// The derived classes generate Imagemagick images
/// based on format defined with <see cref="Formats"/> class
/// </summary>
public interface IImages
{
    #region commercial formats
    /// <summary>
    /// Photocity Digital print over 27x20cm paper
    /// </summary>
    /// <param name="c">Background color</param>
    /// <returns></returns>
    public MagickImage InCartha20x27_o(MagickColor c);
    /// <summary>
    /// Photocity Digital print over 20x27cm paper
    /// </summary>
    /// <param name="c">Background color</param>
    /// <returns></returns>
    public MagickImage InCartha20x27_v(MagickColor c);
    /// <summary>
    /// Photocity Digital print over 15x20cm paper
    /// </summary>
    /// <param name="c">Background color</param>
    /// <returns></returns>
    public MagickImage InCartha15x20_o(MagickColor c);
    /// <summary>
    /// Photocity Digital print over 20x27cm paper
    /// </summary>
    /// <param name="c">Background color</param>
    /// <returns></returns>
    public MagickImage InCartha15x20_v(MagickColor c);
    /// <summary>
    /// 15x10cm paper
    /// </summary>
    /// <param name="c">Background color</param>
    /// <returns></returns>
    public MagickImage FineArt10x15_o(MagickColor c);
    /// <summary>
    /// 10x15cm paper
    /// </summary>
    /// <param name="c">Background color</param>
    /// <returns></returns>
    public MagickImage FineArt10x15_v(MagickColor c);
    /// <summary>
    /// 18x10cm paper
    /// </summary>
    /// <param name="c">Background color</param>
    /// <returns></returns>
    public MagickImage FineArt10x18_o(MagickColor c);
    /// <summary>
    /// 10x18cm paper
    /// </summary>
    /// <param name="c">Background color</param>
    /// <returns></returns>
    public MagickImage FineArt10x18_v(MagickColor c);

    /// <summary>
    /// Photocity Digital print over 27x20cm paper
    /// </summary>
    /// <remarks>White background</remarks>
    /// <returns></returns>
    public MagickImage InCartha20x27_o();
    /// <summary>
    /// Photocity Digital print over 20x27cm paper
    /// </summary>
    /// <remarks>White background</remarks>
    /// <returns></returns>
    public MagickImage InCartha20x27_v();
    /// <summary>
    /// Photocity Digital print over 15x20cm paper
    /// </summary>
    /// <remarks>White background</remarks>
    /// <returns></returns>
    public MagickImage InCartha15x20_o();
    /// <summary>
    /// Photocity Digital print over 15x20cm paper
    /// </summary>
    /// <remarks>White background</remarks>
    /// <returns></returns>
    public MagickImage InCartha15x20_v();
    /// <summary>
    /// 15x10cm paper
    /// </summary>
    /// <remarks>White background</remarks>
    /// <returns></returns>
    public MagickImage FineArt10x15_o();
    /// <summary>
    /// 10x15cm paper
    /// </summary>
    /// <remarks>White background</remarks>
    /// <returns></returns>
    public MagickImage FineArt10x15_v();
    /// <summary>
    /// 18x10cm paper
    /// </summary>
    /// <remarks>White background</remarks>
    /// <returns></returns>
    public MagickImage FineArt10x18_o();
    /// <summary>
    /// 10x18cm paper
    /// </summary>
    /// <remarks>White background</remarks>
    /// <returns></returns>
    public MagickImage FineArt10x18_v();
    #endregion

    #region ISO formats
    /// <summary>
    /// ISO A4 297x210 mm
    /// </summary>
    /// <param name="c">Background color</param>
    /// <returns></returns>
    public MagickImage A4_o(MagickColor c);
    /// <summary>
    /// ISO A4 210x297 mm
    /// </summary>
    /// <param name="c">Background color</param>
    /// <returns></returns>
    public MagickImage A4_v(MagickColor c);

    /// <summary>
    /// ISO A4 297x210 mm
    /// </summary>
    /// <remarks>White background</remarks>
    /// <returns></returns>
    public MagickImage A4_o();
    /// <summary>
    /// ISO A4 210x297 mm
    /// </summary>
    /// <remarks>White background</remarks>
    /// <returns></returns>
    public MagickImage A4_v();
    #endregion

    #region cdv
    /// <summary>
    /// Horizontal Carte de Visite, 100x64mm
    /// </summary>
    /// <param name="c">Background color</param>
    /// <returns></returns>
    public MagickImage CDV_Full_o(MagickColor c);
    /// <summary>
    /// Vertical Carte de Visite, 64x100mm
    /// </summary>
    /// <param name="c">Background color</param>
    /// <returns></returns>
    public MagickImage CDV_Full_v(MagickColor c);
    /// <summary>
    /// Horizontal Carte de Visite reduced to leave a 5 mm border on any side
    /// </summary>
    /// <param name="c">Background color</param>
    /// <returns></returns>
    public MagickImage CDV_Internal_o(MagickColor c);
    /// <summary>
    /// Vertical Carte de Visite reduced to leave a 5 mm border on any side
    /// </summary>
    /// <param name="c">Background color</param>
    /// <returns></returns>
    public MagickImage CDV_Internal_v(MagickColor c);

    /// <summary>
    /// Horizontal Carte de Visite, 100x64mm
    /// </summary>
    /// <remarks>White background</remarks>
    /// <returns></returns>
    public MagickImage CDV_Full_o();
    /// <summary>
    /// Vertical Carte de Visite, 64x100mm
    /// </summary>
    /// <remarks>White background</remarks>
    /// <returns></returns>
    public MagickImage CDV_Full_v();
    /// <summary>
    /// Horizontal Carte de Visite reduced to leave a 5 mm border on any side
    /// </summary>
    /// <remarks>White background</remarks>
    /// <returns></returns>
    public MagickImage CDV_Internal_o();
    /// <summary>
    /// Vertical Carte de Visite reduced to leave a 5 mm border on any side
    /// </summary>
    /// <remarks>White background</remarks>
    /// <returns></returns>
    public MagickImage CDV_Internal_v();
    #endregion

    #region credit card
    /// <summary>
    /// Horizontal credit card, 86x54mm
    /// </summary>
    /// <param name="c">Background color</param>
    /// <returns></returns>
    public MagickImage CC_o(MagickColor c);
    /// <summary>
    /// Vertical credit card, 54x86mm
    /// </summary>
    /// <param name="c">Background color</param>
    /// <returns></returns>
    public MagickImage CC_v(MagickColor c);

    /// <summary>
    /// Horizontal credit card, 86x54mm
    /// </summary>
    /// <remarks>White background</remarks>
    /// <returns></returns>
    public MagickImage CC_o();
    /// <summary>
    /// Vertical credit card, 54x86mm
    /// </summary>
    /// <remarks>White background</remarks>
    /// <returns></returns>
    public MagickImage CC_v();

    #endregion

    #region padded images
    /// <summary>
    /// Creates an image with a border around
    /// </summary>
    /// <param name="c">fill color</param>
    /// <param name="baseSize"></param>
    /// <param name="Padding">border size in mm</param>
    /// <returns></returns>
    public MagickImage Padded(MagickColor c, MagickGeometry baseSize, int Padding);

    /// <summary>
    /// Creates an image with a border around
    /// </summary>
    /// <param name="baseSize"></param>
    /// <param name="Padding">border size in mm</param>
    /// <returns></returns>
    public MagickImage Padded(MagickGeometry baseSize, int Padding);
        
    #endregion
}
