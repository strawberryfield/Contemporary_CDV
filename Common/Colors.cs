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
using System;
using System.Collections.Generic;

namespace Casasoft.CCDV;

/// <summary>
/// Colors conversion utilities
/// </summary>
public class Colors
{
    /// <summary>
    /// List of colors names
    /// </summary>
    public Dictionary<string, MagickColor> colorDictionary;
    
    /// <summary>
    /// Constructor
    /// </summary>
    public Colors()
    {
        fillColorDictionary();
    }
    
    /// <summary>
    /// Fills the list of colors names
    /// </summary>
    private void fillColorDictionary()
    {
        colorDictionary = new(StringComparer.OrdinalIgnoreCase);
        Type cl = typeof(MagickColors);
        foreach (var color in cl.GetProperties())
        {
            colorDictionary.Add(color.Name, (MagickColor)color.GetValue(null));
        }
    }

    /// <summary>
    /// Gets the color by a string
    /// </summary>
    /// <param name="color">name or components values</param>
    /// <returns><see cref="MagickColor"/></returns>
    public MagickColor GetColor(string color)
    {
        if (!string.IsNullOrWhiteSpace(color))
        {
            if (color[0] == '#')
            {
                return new MagickColor(color);
            }
            else
            {
                MagickColor ret;
                if (colorDictionary.TryGetValue(color, out ret))
                {
                    return ret;
                }
                else
                {
                    return null;
                }

            }
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Returns the string rappresentation of the given color
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public string GetColorString(MagickColor color) => color.ToHexString();
 
}
