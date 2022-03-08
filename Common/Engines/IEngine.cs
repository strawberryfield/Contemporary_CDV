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

using Casasoft.CCDV.Scripting;
using ImageMagick;
using System.Collections.Generic;
using System.Reflection;

namespace Casasoft.CCDV.Engines;

/// <summary>
/// Interface for various images managers
/// </summary>
public interface IEngine
{
    /// <summary>
    /// Output resolution
    /// </summary>
    int Dpi { get; set; }
    /// <summary>
    /// List of files to process
    /// </summary>
    List<string> FilesList { get; set; }
    /// <summary>
    /// Color to fill empty spaces
    /// </summary>
    MagickColor FillColor { get; set; }
    /// <summary>
    /// Color for lines and borders
    /// </summary>
    MagickColor BorderColor { get; set; }
    /// <summary>
    /// c# script for custom processing
    /// </summary>
    string Script { get; set; }
    /// <summary>
    /// compiled script for custom processing
    /// </summary>
    Assembly CustomCode { get; set; }
    /// <summary>
    /// Class that handles user scripts
    /// </summary>
    IScripting ScriptingClass { get; set; }

    /// <summary>
    /// Does the dirty work
    /// </summary>
    /// <returns>Image to print</returns>
    MagickImage GetResult();
    /// <summary>
    /// Does the dirty work
    /// </summary>
    /// <param name="quiet">suppress messages when running</param>
    /// <returns>Image to print</returns>
    MagickImage GetResult(bool quiet);
    /// <summary>
    /// Writes exif infos on image
    /// </summary>
    /// <param name="image">image to process</param>
    void SetImageParameters(MagickImage image);
    /// <summary>
    /// Writes info text on images
    /// </summary>
    /// <param name="o">output related infos</param>
    /// <param name="image">image to process</param>
    void SetImageInfo(string o, MagickImage image);
    /// <summary>
    /// Writes info text on images
    /// </summary>
    /// <param name="i">input related infos</param>
    /// <param name="o">output related infos</param>
    /// <param name="image">image to process</param>
    void SetImageInfo(string i, string o, MagickImage image);
    /// <summary>
    /// gets the program banner
    /// </summary>
    /// <returns></returns>
    string WelcomeBannerText();

    /// <summary>
    /// Returns the parameters in json format
    /// </summary>
    /// <returns></returns>
    string GetJsonParams();
    /// <summary>
    /// Sets the parameters from json formatted string
    /// </summary>
    /// <param name="json"></param>
     void SetJsonParams(string json);

}
