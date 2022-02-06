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

namespace Casasoft.CCDV.JSON;

/// <summary>
/// Parameters for Credit Cards
/// </summary>
public class CreditCardParameters : CommonParameters
{
    /// <summary>
    /// Text to print on the front (like cardholder)
    /// </summary>
    public string FrontText { get; set; } = string.Empty;
    /// <summary>
    /// Front text font
    /// </summary>
    public string FrontTextFont { get; set; } = "Arial";
    /// <summary>
    /// Front text fill color
    /// </summary>
    public string FrontTextColor { get; set; } = "#FFFFFF";
    /// <summary>
    /// front text border color
    /// </summary>
    public string FrontTextBorder { get; set; } = "#000000";
    /// <summary>
    /// use bold weight for front text (if available for font)
    /// </summary>
    public bool fontBold { get; set; }
    /// <summary>
    /// use italic style for front text (if available for font)
    /// </summary>
    public bool fontItalic { get; set; }
    /// <summary>
    /// Pseudo magnetic band color
    /// </summary>
    public string MagneticBandColor { get; set; } = "#000000FF";
    /// <summary>
    /// Pseudo magnetic band image
    /// </summary>
    public string MagneticBandImage { get; set; } = string.Empty;
    /// <summary>
    /// backgroud image
    /// </summary>
    public string BackImage { get; set; } = string.Empty;
    /// <summary>
    /// text to put in the back side
    /// </summary>
    /// <remarks>
    /// The text can be formatted with Pango markup
    /// </remarks>
    public string BackText { get; set; } = string.Empty;

    /// <summary>
    /// Default constructor
    /// </summary>
    public CreditCardParameters() : base()
    {
    }
}
