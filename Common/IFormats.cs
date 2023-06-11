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
/// The classes that inherit this interface handle all target formats
/// converting sizes in mm to pixel using the specified dpi resolution
/// </summary>
public interface IFormats
{
    /// <summary>
    /// Converts mm to pixels
    /// </summary>
    /// <param name="mm">size to convert</param>
    /// <returns>pixel at defined resolution</returns>
    public int ToPixels(int mm);

    /// <summary>
    /// Returns the resolution
    /// </summary>
    public int DPI { get; }

    #region commercial formats
    /// <summary>
    /// Photocity Digital print over 27x20cm paper
    /// </summary>
    /// <remarks>
    /// In printing the service enlarges (an cuts!) the image, so I need to take care of this
    /// </remarks>
    public MagickGeometry InCartha20x27_o { get; }
    /// <summary>
    /// Photocity Digital print over 20x27cm paper
    /// </summary>
    /// <remarks>
    /// In printing the service enlarges (an cuts!) the image, so I need to take care of this
    /// </remarks>
    public MagickGeometry InCartha20x27_v { get; }
    /// <summary>
    /// Photocity Digital print over 15x20cm paper
    /// </summary>
    /// <remarks>
    /// In printing the service enlarges (an cuts!) the image, so I need to take care of this
    /// </remarks>
    public MagickGeometry InCartha15x20_o { get; }
    /// <summary>
    /// Photocity Digital print over 15x20cm paper
    /// </summary>
    /// <remarks>
    /// In printing the service enlarges (an cuts!) the image, so I need to take care of this
    /// </remarks>
    public MagickGeometry InCartha15x20_v { get; }
    /// <summary>
    /// 15x10cm paper
    /// </summary>
    public MagickGeometry FineArt10x15_o { get; }
    /// <summary>
    /// 10x15cm paper
    /// </summary>
    public MagickGeometry FineArt10x15_v { get; }
    /// <summary>
    /// 18x10cm paper
    /// </summary>
    public MagickGeometry FineArt10x18_o { get; }
    /// <summary>
    /// 10x18cm paper
    /// </summary>
    public MagickGeometry FineArt10x18_v { get; }
    #endregion

    #region ISO formats
    /// <summary>
    /// 15x10cm paper
    /// </summary>
    public MagickGeometry A4_o { get; }
    /// <summary>
    /// 10x15cm paper
    /// </summary>
    public MagickGeometry A4_v { get; }
    #endregion

    #region cdv
    /// <summary>
    /// Horizontal Carte de Visite, 100x64mm
    /// </summary>
    public MagickGeometry CDV_Full_o { get; }
    /// <summary>
    /// Vertical Carte de Visite, 64x100mm
    /// </summary>
    public MagickGeometry CDV_Full_v { get; }
    /// <summary>
    /// Horizontal Carte de Visite reduced to leave a 5 mm border on any side
    /// </summary>
    public MagickGeometry CDV_Internal_o { get; }
    /// <summary>
    /// Vertical Carte de Visite reduced to leave a 5 mm border on any side
    /// </summary>
    public MagickGeometry CDV_Internal_v { get; }
    #endregion

    #region credit card
    /// <summary>
    /// Horizontal credit card, 86x54mm
    /// </summary>
    public MagickGeometry CC_o { get; }
    /// <summary>
    /// Vertical credit card, 54x86mm
    /// </summary>
    public MagickGeometry CC_v { get; }
    #endregion


    /// <summary>
    /// Adds Exif infos to image
    /// </summary>
    /// <param name="img">image to process</param>
    /// <param name="ext">extension (file format)</param>
    public void SetImageParameters(MagickImage img, string ext);

    /// <summary>
    /// Adds Exif infos to jpg image
    /// </summary>
    /// <param name="img">image to process</param>
    public void SetImageParameters(MagickImage img);
}
