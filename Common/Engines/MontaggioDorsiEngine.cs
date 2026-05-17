// copyright (c) 2020-2026 Roberto Ceccarelli - Casasoft
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
using Casasoft.CCDV.Scripting;
using ImageMagick;
using System;
using System.Text.Json;

namespace Casasoft.CCDV.Engines;

/// <summary>
/// Montaggio dorsi engine
/// </summary>
public class MontaggioDorsiEngine : BaseMontaggioEngine
{
    #region constructors
    /// <summary>
    /// Constructor
    /// </summary>
    public MontaggioDorsiEngine() : base()
    {
        parameters = new BaseMontaggioParameters();
        ScriptingClass = new MontaggioDorsiScripting();
        OutputName = "dorsi";
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="par"></param>
    public MontaggioDorsiEngine(CommandLine par) : base(par)
    {
        MontaggioDorsiCommandLine p = (MontaggioDorsiCommandLine)par;
        ScriptingClass = new MontaggioDorsiScripting();

        if (string.IsNullOrWhiteSpace(par.JSON))
        {
            parameters = new BaseMontaggioParameters();
            PaperFormat = p.PaperFormat;
            CanvasGravity = p.CanvasGravity;
            Script = p.Script;
        }
    }
    #endregion

    #region json
    /// <summary>
    /// Returns the parameters in json format
    /// </summary>
    /// <returns></returns>
    public override string GetJsonParams()
    {
        BaseMontaggioParameters p = (BaseMontaggioParameters)parameters;
        GetBaseMontaggioJsonParams(p);
        return JsonSerializer.Serialize(p);
    }

    /// <summary>
    /// Sets the parameters from json formatted string
    /// </summary>
    /// <param name="json"></param>
    public override void SetJsonParams(string json) =>
        SetJsonParams(JsonSerializer.Deserialize<BaseMontaggioParameters>(json));

    /// <summary>
    /// Sets the parameters from json deserialized object
    /// </summary>
    /// <param name="json"></param>
    public override void SetJsonParams(IParameters json) =>
        SetJsonParams((BaseMontaggioParameters)json);

    private void SetJsonParams(BaseMontaggioParameters p)
    {
        parameters = p;
        SetBaseMontaggioJsonParams(p);
    }
    #endregion

    #region build
    /// <summary>
    /// Does the dirty work
    /// </summary>
    /// <param name="quiet"></param>
    /// <returns></returns>
    public override MagickImage GetResult(bool quiet)
    {
        _ = base.GetResult(quiet);

        MagickImage final = GetOutputPaper(PaperFormat);

        MagickImageCollection imagesV = new();
        MagickImageCollection imagesO = new();

        // if no file specified use a blank image
        if (FilesList.Count == 0)
        {
            MagickImage dorsoOrig = img.CDV_Full_v();
            switch (PaperFormat)
            {
                case PaperFormats.Medium:
                    for (int i = 0; i < 3; i++) imagesV.Add(dorsoOrig.Clone());
                    break;
                case PaperFormats.Large:
                    for (int i = 0; i < 4; i++) imagesV.Add(dorsoOrig.Clone());
                    dorsoOrig.Rotate(90);
                    for (int i = 0; i < 2; i++) imagesO.Add(dorsoOrig.Clone());
                    break;
                case PaperFormats.A4:
                    for (int i = 0; i < 4; i++) imagesV.Add(dorsoOrig.Clone());
                    for (int i = 0; i < 4; i++) imagesO.Add(dorsoOrig.Clone());
                    break;
            }
        }
        else
        {
            int nImg = 0;
            switch (PaperFormat)
            {
                case PaperFormats.Medium:
                    _ = LoadImages(3, nImg, imagesV, quiet, fmt.CDV_Full_v);
                    break;
                case PaperFormats.Large:
                case PaperFormats.Large20x30:
                    nImg = LoadImages(4, nImg, imagesV, quiet, fmt.CDV_Full_v);
                    _ = LoadImages(2, nImg, imagesO, quiet, fmt.CDV_Full_o);
                    break;
                case PaperFormats.A4:
                    nImg = LoadImages(4, nImg, imagesV, quiet, fmt.CDV_Full_v);
                    _ = LoadImages(4, nImg, imagesO, quiet, fmt.CDV_Full_v);
                    break;
            }
        }

        DrawCutMarks(final, PaperFormat);

        switch (PaperFormat)
        {
            case PaperFormats.Medium:
                final.Composite(imagesV.AppendHorizontally(), Gravity.Center, 0, 0);
                break;
            case PaperFormats.Large:
            case PaperFormats.Large20x30:
                final.Composite(imagesV.AppendHorizontally(), Gravity.North, 0, (int)fmt.ToPixels(10));
                final.Composite(imagesO.AppendHorizontally(), Gravity.North,
                    0, (int)(fmt.ToPixels(10) + fmt.CDV_Full_v.Height - 1));
                break;
            case PaperFormats.A4:
                final.Composite(imagesV.AppendHorizontally(), Gravity.North, 0, (int)fmt.ToPixels(5));
                final.Composite(imagesO.AppendHorizontally(), Gravity.North,
                    0, (int)(fmt.ToPixels(5) + fmt.CDV_Full_v.Height - 1));
                break;
        }

        if (PaperFormat is PaperFormats.Large or PaperFormats.Medium or PaperFormats.Large20x30)
        {
            SetImageInfo(WelcomeBannerText(), $"{OutputName}.{Extension}", final, PaperFormat);
        }
        return final;
    }
    #endregion
}
