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

namespace Casasoft.CCDV.JSON;

/// <summary>
/// Parameters for MontaggioFoto
/// </summary>
public class MontaggioFotoParameters : CommonParameters
{
    /// <summary>
    /// Set if image has full CDV size (100x64mm)
    /// </summary>
    public bool FullSize { get; set; }
    /// <summary>
    /// Set if white border is removed
    /// </summary>
    public bool Trim { get; set; }
    /// <summary>
    /// Set if a border to full CDV size (100x64mm) is added
    /// </summary>
    public bool WithBorder { get; set; }
    /// <summary>
    /// Blank border around the image
    /// </summary>
    public int Padding { get; set; } = 0;
    /// <summary>
    /// Canvas gravity
    /// </summary>
    public Gravity CanvasGravity { get; set; }

    /// <summary>
    /// Default constructor
    /// </summary>
    public MontaggioFotoParameters() : base()
    {
    }
}
