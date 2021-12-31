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
using Mono.Options;
using System.Collections.Generic;

namespace Casasoft.CCDV;

/// <summary>
/// Interface for command line handling
/// </summary>
public interface ICommandLine
{
    /// <summary>
    /// Output file name
    /// </summary>
    string OutputName { get; set; }
    /// <summary>
    /// Output resolution
    /// </summary>
    int Dpi { get; set; }
    /// <summary>
    /// Input files list
    /// </summary>
    List<string> FilesList { get; set; }
    /// <summary>
    /// MonoOptions options list
    /// </summary>
    OptionSet Options { get; set; }
    /// <summary>
    /// Usage example
    /// </summary>
    string Usage { get; set; }
    /// <summary>
    /// Color to fill images
    /// </summary>
    MagickColor FillColor { get; set; }
    /// <summary>
    /// Color to use for lines and borders
    /// </summary>
    MagickColor BorderColor { get; set; }
    /// <summary>
    /// Long description for man pages
    /// </summary>
    string LongDesc { get; set; }

    /// <summary>
    /// Prints the welcome banner
    /// </summary>
    void WelcomeBanner();
    /// <summary>
    /// Text for welcome banner
    /// </summary>
    /// <returns></returns>
    string WelcomeBannerText();
    /// <summary>
    /// Sets base options in derived classes
    /// </summary>
    void AddBaseOptions();
    /// <summary>
    /// Does the dirty work
    /// </summary>
    /// <param name="args">command line arguments</param>
    /// <returns>true if nothing to (ie. help)</returns>
    bool Parse(string[] args);
    /// <summary>
    /// Windows shell does not expand wildcards
    /// </summary>
    void ExpandWildcards();
}
