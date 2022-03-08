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

using Casasoft.CCDV.JSON;
using System.Reflection;

namespace Casasoft.CCDV.Scripting;

/// <summary>
/// Interface for scripting handler classes
/// </summary>
public interface IScripting
{
    /// <summary>
    /// json unserialized parameters
    /// </summary>
    IParameters Parameters { get; set; }

    /// <summary>
    /// Adds namespaces and class declaration to script
    /// </summary>
    /// <param name="script"></param>
    /// <returns></returns>
    string WrapScript(string script);
    /// <summary>
    /// Builds in-memory assembly
    /// </summary>
    /// <param name="script"></param>
    /// <returns></returns>
    Assembly Compile(string script);
}
