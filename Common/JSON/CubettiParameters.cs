﻿// copyright (c) 2020-2022 Roberto Ceccarelli - Casasoft
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casasoft.CCDV.JSON;

/// <summary>
/// Json structure for Cubetti
/// </summary>
public class CubettiParameters : CommonParameters
{
    /// <summary>
    /// Number of rows to generate
    /// </summary>
    public int Rows { get; set; } = 2;
    /// <summary>
    /// Number of Columns to generate
    /// </summary>
    public int Columns { get; set; } = 3;
    /// <summary>
    /// Size of any cube (mm)
    /// </summary>
    public int Size { get; set; } = 50;

    /// <summary>
    /// Default constructor
    /// </summary>
    public CubettiParameters() : base()
    {
    }
}