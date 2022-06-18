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
    int Thickness { get; set; }
    Formats fmt { get; set; }
    MagickColor fillColor { get; set; }
    MagickColor borderColor { get; set; }
    TargetType targetType { get; set; }
    bool isHorizontal { get; set; }

    void SetTopImage(string filename);
    void SetBottomImage(string filename);
    void SetLeftImage(string filename);
    void SetRightImage(string filename);
    void SetFrontImage(string filename);
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
