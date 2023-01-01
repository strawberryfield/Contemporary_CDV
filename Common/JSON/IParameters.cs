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

using System.Collections.Generic;

namespace Casasoft.CCDV.JSON;

/// <summary>
/// Common parameters list
/// </summary>
public interface IParameters
{
    /// <summary>
    /// Color to fill images
    /// </summary>
    string FillColor { get; set; }
    /// <summary>
    /// Color to use for lines and borders
    /// </summary>
    string BorderColor { get; set; }
    /// <summary>
    /// Output resolution
    /// </summary>
    int Dpi { get; set; }
    /// <summary>
    /// Output file name
    /// </summary>
    string OutputName { get; set; }
    /// <summary>
    /// Output file name extension
    /// </summary>
    string Extension { get; set; }
    /// <summary>
    /// c# script for custom processing
    /// </summary>
    string Script { get; set; }
    /// <summary>
    /// Extra info for user scripting
    /// </summary>
    string Tag { get; set; }
    /// <summary>
    /// Files to process
    /// </summary>
    List<string> FilesList { get; set; }
}
