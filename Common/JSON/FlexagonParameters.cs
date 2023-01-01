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

namespace Casasoft.CCDV.JSON;

/// <summary>
/// Json structure for Flexagon
/// </summary>
public class FlexagonParameters : CommonParameters
{
    /// <summary>
    /// Number of faces of the flexagons
    /// </summary>
    /// <remarks>
    /// Valid numbers are 3, 4 or 6
    /// </remarks>
    public int Faces { get; set; } = 3;

    /// <summary>
    /// True if samples images will be created
    /// </summary>
    public bool useSampleImages { get; set; }

    /// <summary>
    /// Default constructor
    /// </summary>
    public FlexagonParameters() : base()
    {
    }
}
