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
/// This class handles all target formats
/// converting sizes in mm to pixel using the specified dpi resolution
/// </summary>
public class Formats
{
    private int _dpi;
    private double _inch = 25.4;

    #region constructors
    /// <summary>
    /// This constructor ser the resolution to 300 DPI
    /// </summary>
    public Formats() : this(300) { }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dpi">Resolution for conversion in pixels</param>
    public Formats(int dpi)
    {
        _dpi = dpi;
    }
    #endregion

    /// <summary>
    /// Converts mm to pixels
    /// </summary>
    /// <param name="mm">size to convert</param>
    /// <returns>pixel at defined resolution</returns>
    public int ToPixels(int mm) => (int)(mm * _dpi / _inch);

    /// <summary>
    /// Returns the resolution
    /// </summary>
    public int DPI => _dpi;

    #region commercial formats
    /// <summary>
    /// Photocity Digital print over 27x20cm paper
    /// </summary>
    /// <remarks>
    /// In printing the service enlarges (an cuts!) the image, so I need to take care of this
    /// </remarks>
    public MagickGeometry InCartha20x27_o => new(ToPixels((int)(270 * 1.04)), ToPixels((int)(200 * 1.04)));
    /// <summary>
    /// Photocity Digital print over 20x27cm paper
    /// </summary>
    /// <remarks>
    /// In printing the service enlarges (an cuts!) the image, so I need to take care of this
    /// </remarks>
    public MagickGeometry InCartha20x27_v => swap(InCartha20x27_o);
    /// <summary>
    /// Photocity Digital print over 15x20cm paper
    /// </summary>
    /// <remarks>
    /// In printing the service enlarges (an cuts!) the image, so I need to take care of this
    /// </remarks>
    public MagickGeometry InCartha15x20_o => new(ToPixels((int)(200 * 1.03)), ToPixels((int)(150 * 1.03)));
    /// <summary>
    /// Photocity Digital print over 15x20cm paper
    /// </summary>
    /// <remarks>
    /// In printing the service enlarges (an cuts!) the image, so I need to take care of this
    /// </remarks>
    public MagickGeometry InCartha15x20_v => swap(InCartha15x20_o);
    /// <summary>
    /// 15x10cm paper
    /// </summary>
    public MagickGeometry FineArt10x15_o => new(ToPixels(152), ToPixels(102));
    /// <summary>
    /// 10x15cm paper
    /// </summary>
    public MagickGeometry FineArt10x15_v => swap(FineArt10x15_o);
    /// <summary>
    /// 18x10cm paper
    /// </summary>
    public MagickGeometry FineArt10x18_o => new(ToPixels(180), ToPixels(102));
    /// <summary>
    /// 10x18cm paper
    /// </summary>
    public MagickGeometry FineArt10x18_v => swap(FineArt10x18_o);
    #endregion

    #region ISO formats
    /// <summary>
    /// 15x10cm paper
    /// </summary>
    public MagickGeometry A4_o => new(ToPixels(297), ToPixels(210));
    /// <summary>
    /// 10x15cm paper
    /// </summary>
    public MagickGeometry A4_v => swap(A4_o);
    #endregion

    #region cdv
    /// <summary>
    /// Horizontal Carte de Visite, 100x64mm
    /// </summary>
    public MagickGeometry CDV_Full_o => new(ToPixels(100), ToPixels(64));
    /// <summary>
    /// Vertical Carte de Visite, 64x100mm
    /// </summary>
    public MagickGeometry CDV_Full_v => swap(CDV_Full_o);
    /// <summary>
    /// Horizontal Carte de Visite reduced to leave a 5 mm border on any side
    /// </summary>
    public MagickGeometry CDV_Internal_o => new(ToPixels(90), ToPixels(54));
    /// <summary>
    /// Vertical Carte de Visite reduced to leave a 5 mm border on any side
    /// </summary>
    public MagickGeometry CDV_Internal_v => swap(CDV_Internal_o);
    #endregion

    #region credit card
    /// <summary>
    /// Horizontal credit card, 86x54mm
    /// </summary>
    public MagickGeometry CC_o => new(ToPixels(86), ToPixels(54));
    /// <summary>
    /// Vertical credit card, 54x86mm
    /// </summary>
    public MagickGeometry CC_v => swap(CC_o);
    #endregion

    private static MagickGeometry swap(MagickGeometry g)
    {
        int tmp = g.Width;
        g.Width = g.Height;
        g.Height = tmp;
        return g;
    }

    /// <summary>
    /// Adds Exif infos to image
    /// </summary>
    /// <param name="img">image to process</param>
    /// <param name="ext">extension (file format)</param>
    public void SetImageParameters(MagickImage img, string ext)
    {
        img.Quality = 95;
        img.Density = new Density(_dpi);
        img.ColorSpace = ColorSpace.sRGB;

        if (ext == "jpg")
        {
            img.Format = MagickFormat.Jpg;

            ExifProfile exif = new();
            exif.SetValue(ExifTag.Make, "Casasoft");
            exif.SetValue(ExifTag.Model, "Contemporary Carte de Visite Tools");
            exif.SetValue(ExifTag.Software, "Casasoft Contemporary Carte de Visite Tools");
            img.SetProfile(exif);
        }
    }

    /// <summary>
    /// Adds Exif infos to jpg image
    /// </summary>
    /// <param name="img">image to process</param>
    public void SetImageParameters(MagickImage img) => SetImageParameters(img, "jpg");
}
