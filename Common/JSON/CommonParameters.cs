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

using System.Collections.Generic;

namespace Casasoft.CCDV.JSON;

/// <summary>
/// Common parameters list
/// </summary>
public class CommonParameters : IParameters
{
    /// <summary>
    /// Color to fill images
    /// </summary>
    public string FillColor { get; set; } = "#FFFFFF";
    /// <summary>
    /// Color to use for lines and borders
    /// </summary>
    public string BorderColor { get; set; } = "#000000";
    /// <summary>
    /// Output resolution
    /// </summary>
    public int Dpi { get; set; } = 300;
    /// <summary>
    /// Output file name
    /// </summary>
    public string OutputName { get; set; }
    /// <summary>
    /// c# script for custom processing
    /// </summary>
    public string Script { get; set; }
    /// <summary>
    /// Files to process
    /// </summary>
    public List<string> FilesList { get; set; }

    /// <summary>
    /// Default constructor
    /// </summary>
    public CommonParameters()
    {
        FilesList = new();
    }
}
