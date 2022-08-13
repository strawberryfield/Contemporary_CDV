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

namespace Casasoft.CCDV;

/// <summary>
/// Interface for boxes and folders builder
/// </summary>
public interface IBuilder
{
    /// <summary>
    /// Thickness of the box (mm)
    /// </summary>
    int Thickness { get; set; }
    /// <summary>
    /// Reference to formats
    /// </summary>
    Formats fmt { get; set; }
    /// <summary>
    /// Fill color
    /// </summary>
    MagickColor fillColor { get; set; }
    /// <summary>
    /// Border color
    /// </summary>
    MagickColor borderColor { get; set; }
    /// <summary>
    /// target box format
    /// </summary>
    TargetType targetType { get; set; }
    /// <summary>
    /// Set if box is landscape
    /// </summary>
    bool isHorizontal { get; set; }

    /// <summary>
    /// Sets image for the top border
    /// </summary>
    /// <param name="filename"></param>
    void SetTopImage(string filename);

    /// <summary>
    /// Sets image for the bottom border
    /// </summary>
    /// <param name="filename"></param>
    void SetBottomImage(string filename);

    /// <summary>
    /// Sets image for the left border
    /// </summary>
    /// <param name="filename"></param>
    void SetLeftImage(string filename);

    /// <summary>
    /// Sets image for the right border
    /// </summary>
    /// <param name="filename"></param>
    void SetRightImage(string filename);

    /// <summary>
    /// Sets image for the front cover
    /// </summary>
    /// <param name="filename"></param>
    void SetFrontImage(string filename);

    /// <summary>
    /// Sets image for the back cover
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="isHorizontal"></param>
    void SetBackImage(string filename, bool isHorizontal = false);

    /// <summary>
    /// adds lines to help in cutting images
    /// </summary>
    /// <param name="img"></param>
    void AddCuttingLines(MagickImage img);

    /// <summary>
    /// Set of images for testing
    /// </summary>
    void CreateTestImages();

    /// <summary>
    /// Output paper size
    /// </summary>
    PaperFormats PaperFormat { get; set; }
    /// <summary>
    /// Returns empty final image
    /// </summary>
    /// <returns></returns>
    MagickImage GetOutputImage();
}
