﻿// copyright (c) 2020-2023 Roberto Ceccarelli - Casasoft
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

using System.Text.Json.Serialization;

namespace Casasoft.CCDV.JSON;

/// <summary>
/// Parameters for box and folder builder
/// </summary>
public class BaseBuilderParameters : CommonParameters
{
    /// <summary>
    /// Image for top border
    /// </summary>
    public string topImage { get; set; } = string.Empty;
    /// <summary>
    /// Image for bottom border
    /// </summary>
    public string bottomImage { get; set; } = string.Empty;
    /// <summary>
    /// Image for left border
    /// </summary>
    public string leftImage { get; set; } = string.Empty;
    /// <summary>
    /// Image for right border
    /// </summary>
    public string rightImage { get; set; } = string.Empty;
    /// <summary>
    /// image for front cover
    /// </summary>
    public string frontImage { get; set; } = string.Empty;
    /// <summary>
    /// image for back cover
    /// </summary>
    public string backImage { get; set; } = string.Empty;

    /// <summary>
    /// Prints text in bold if available
    /// </summary>
    public bool fontBold { get; set; }
    /// <summary>
    /// Prints text in italic if available
    /// </summary>
    public bool fontItalic { get; set; }
    /// <summary>
    /// Font for border text
    /// </summary>
    public string font { get; set; } = "Arial";
    /// <summary>
    /// Text to print on left border
    /// </summary>
    public string borderText { get; set; } = string.Empty;

    /// <summary>
    /// Thickness of the box (mm)
    /// </summary>
    public int spessore { get; set; } = 5;
    /// <summary>
    /// Set if box is landscape
    /// </summary>
    public bool isHorizontal { get; set; }
    /// <summary>
    /// target box format
    /// </summary>
    /// <remarks>
    /// <para>Original data are in enum <see cref="TargetType"/> format</para>
    /// <list type=">">
    /// <item>0 - Carte de viste 64x100mm</item>
    /// <item>1 - Credit card 86x54mm</item>
    /// </list>
    /// </remarks>
    public int targetFormat { get; set; } = (int)TargetType.cdv;

    /// <summary>
    /// Set to generate sample images
    /// </summary>
    public bool useTestImages { get; set; }

    /// <summary>
    /// Output paper size
    /// </summary>
    public string Paper { get; set; }

    /// <summary>
    /// Output paper size
    /// </summary>
    [JsonIgnore]
    public PaperFormats PaperFormat
    {
        get => Utils.GetPaperFormat(Paper);
        set => Paper = value.ToString();
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    public BaseBuilderParameters() : base()
    {
    }
}
