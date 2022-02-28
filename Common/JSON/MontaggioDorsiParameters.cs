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

using System.Text.Json.Serialization;

namespace Casasoft.CCDV.JSON;

/// <summary>
/// Parameters for MontaggioDorsi
/// </summary>
public class MontaggioDorsiParameters : CommonParameters
{
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
        get
        {
            PaperFormats ret = PaperFormats.Large;
            if(!string.IsNullOrEmpty(Paper))
            {
                if(Paper.ToUpper() == "MEDIUM")
                {
                    ret = PaperFormats.Medium;
                }
            }
            return ret;
        }
        set => Paper = value.ToString();
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    public MontaggioDorsiParameters()
    {
    }
}
