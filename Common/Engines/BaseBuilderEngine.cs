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
using ImageMagick;
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

    /// <summary>
    /// <see cref="Builder"/> viene creato nel corpo del costruttore della
    /// sottoclasse concreta (<see cref="FolderEngine"/>/<see cref="ScatolaEngine"/>),
    /// che gira DOPO che <see cref="BaseEngine(ICommandLine)"/> ha finito.
    /// L'applicazione del JSON va quindi rimandata fino a quel momento.
    /// </summary>
    protected override bool DeferJsonParams => true;

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
        p.PaperFormat = builder.PaperFormat;
        p.spessore = builder.Thickness;

        return JsonSerializer.Serialize(p);
    }

    /// <summary>
    /// Sets the parameters from json formatted string
    /// </summary>
    /// <param name="json"></param>
    public override void SetJsonParams(string json) =>
        SetJsonParams(JsonSerializer.Deserialize<BaseBuilderParameters>(json));

    /// <summary>
    /// Sets the parameters from json deserialized object
    /// </summary>
    /// <param name="json"></param>
    public override void SetJsonParams(IParameters json) =>
        SetJsonParams((BaseBuilderParameters)json);

    private void SetJsonParams(BaseBuilderParameters p)
    {
        parameters = p;
        SetBaseJsonParams();      // imposta anche Dpi → ricrea fmt/img sull'Engine
        Script = p.Script;

        BaseBuilder builder = (BaseBuilder)Builder;
        builder.fillColor = colors.GetColor(p.FillColor);
        builder.borderColor = colors.GetColor(p.BorderColor);
        builder.fmt = fmt; // tiene la geometria del Builder sincronizzata col Dpi del JSON

        builder.font = p.font;
        builder.fontBold = p.fontBold;
        builder.fontItalic = p.fontItalic;
        builder.borderText = p.borderText;
        builder.isHorizontal = p.isHorizontal;
        builder.targetType = (TargetType)p.targetFormat;
        builder.Thickness = p.spessore;
        builder.PaperFormat = p.PaperFormat;

        // targetType / isHorizontal / Thickness / Dpi possono differire da
        // quelli usati quando Builder è stato creato dalla command line:
        // ricostruisce i canvas vuoti con la geometria corretta, esattamente
        // come fa il costruttore BaseBuilder(BaseBuilderCommandLine, IFormats).
        builder.makeEmptyImages();

        if (p.useTestImages)
            builder.CreateTestImages();

        // Carica davvero le immagini (non solo il path), così il JSON si
        // comporta come le rispettive opzioni da riga di comando.
        builder.SetFrontImage(p.frontImage);
        builder.SetBackImage(p.backImage, p.isHorizontal);
        builder.SetTopImage(p.topImage);
        builder.SetBottomImage(p.bottomImage);
        builder.SetLeftImage(p.leftImage);
        builder.SetRightImage(p.rightImage);
    }
    #endregion

    /// <summary>
    /// Does the dirty work
    /// </summary>
    /// <returns>Image to print</returns>
    public override MagickImage GetResult(bool quiet)
    {
        MagickImage ret = base.GetResult(quiet);
        Builder.ScriptInstance = ScriptInstance;
        return ret;
    }

    /// <summary>
    /// True when the output paper requires the box/folder layout to be
    /// anchored to the left edge instead of centred, so any part of the
    /// layout that does not fit is cut off on the right side rather than
    /// symmetrically on both sides. Currently only
    /// <see cref="PaperFormats.Medium13x17"/> uses this.
    /// </summary>
    /// <param name="format">Output paper format</param>
    protected static bool IsLeftAlignedLayout(PaperFormats format) => format == PaperFormats.Medium13x17;
}
