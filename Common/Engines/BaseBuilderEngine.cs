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
using System.Text.Json;

namespace Casasoft.CCDV.Engines;

/// <summary>
/// Abstract class for folders and boxes builders
/// </summary>
public class BaseBuilderEngine : BaseEngine, IBaseBuilderEngine
{
    /// <summary>
    /// Constructor
    /// </summary>
    public BaseBuilderEngine() : base()
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="par"></param>
    public BaseBuilderEngine(ICommandLine par) : base(par)
    {
    }

    /// <summary>
    /// Common builder reference
    /// </summary>
    public IBuilder Builder { get; set; }

    #region json
    /// <summary>
    /// Returns the parameters in json format
    /// </summary>
    /// <returns></returns>
    public override string GetJsonParams()
    {
        GetBaseJsonParams();
        BaseBuilderParameters p = (BaseBuilderParameters)parameters;
        BaseBuilder builder = (BaseBuilder)Builder;
        
        p.frontImage = builder.frontImagePath;
        p.backImage = builder.backImagePath;
        p.topImage = builder.topImagePath;
        p.bottomImage = builder.bottomImagePath;
        p.leftImage = builder.leftImagePath;
        p.rightImage = builder.rightImagePath;

        p.borderText = builder.borderText;
        p.font = builder.font;
        p.fontBold = builder.fontBold;
        p.fontItalic = builder.fontItalic;
        p.isHorizontal = builder.isHorizontal;
        p.targetFormat = (int)builder.targetType;

        return JsonSerializer.Serialize(p);
    }

    /// <summary>
    /// Sets the parameters from json formatted string
    /// </summary>
    /// <param name="json"></param>
    public override void SetJsonParams(string json)
    {
        BaseBuilderParameters p = JsonSerializer.Deserialize<BaseBuilderParameters>(json);
        parameters = p;
        SetBaseJsonParams();

        BaseBuilder builder = (BaseBuilder)Builder;
        builder.fillColor = colors.GetColor(p.FillColor);
        builder.borderColor = colors.GetColor(p.BorderColor);
        Dpi = p.Dpi;
        builder.borderText = p.borderText;
        builder.font = p.font;
        builder.fontBold = p.fontBold;
        builder.fontItalic = p.fontItalic;
        builder.isHorizontal = p.isHorizontal;
        builder.targetType = (TargetType)p.targetFormat;

        builder.frontImagePath = p.frontImage;
        builder.backImagePath = p.backImage;
        builder.topImagePath = p.topImage;
        builder.bottomImagePath = p.bottomImage;
        builder.leftImagePath = p.leftImage;
        builder.rightImagePath = p.rightImage;
    }
    #endregion

}
