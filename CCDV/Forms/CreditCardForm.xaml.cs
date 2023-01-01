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

using Casasoft.CCDV.Engines;

namespace Casasoft.CCDV.UI;

/// <summary>
/// Interaction logic for CreditCardForm.xaml
/// </summary>
public partial class CreditCardForm : BaseForm
{
    public CreditCardForm()
    {
        InitializeComponent();
        engine = new CreditCardEngine();
    }

    protected override void setEngineParameters()
    {
        base.setEngineParameters();
        engine.FillColor = commonOptions.FillColor;
        engine.BorderColor = commonOptions.BorderColor;
        engine.Dpi = commonOptions.DpiValue;
        engine.Script = commonOptions.Script;
        engine.Tag = commonOptions.ScriptTag;

        CreditCardEngine eng = (CreditCardEngine)engine;
        eng.FilesList.Clear();
        eng.FilesList.Add(frontImage.Value);
        eng.BackImage = backImage.Value;
        eng.FrontText = frontText.Text;
        eng.BackText = backText.Value;
        eng.FrontTextFont = fontFront.Font;
        eng.FrontTextColor = cpFill.IMColor;
        eng.FrontTextBorder = cpBorder.IMColor;
        eng.FrontTextBackground = cpBackground.IMColor;
        eng.fontBold = fontFront.FontBold;
        eng.fontItalic = fontFront.FontItalic;
        eng.MagneticBandColor = cpMB.IMColor;
        eng.MagneticBandImage = mbImage.Value;
    }

    protected override void loadJson(string json)
    {
        base.loadJson(json);
        CreditCardEngine eng = (CreditCardEngine)engine;
        frontImage.Value = eng.FilesList.Count > 0 ? eng.FilesList[0] : string.Empty;
        backImage.Value = eng.BackImage;
        frontText.Text = eng.FrontText;
        backText.Value = eng.BackText;
        fontFront.Font = eng.FrontTextFont;
        cpFill.IMColor = eng.FrontTextColor;
        cpBorder.IMColor = eng.FrontTextBorder;
        cpBackground.IMColor = eng.FrontTextBackground;
        fontFront.FontBold = eng.fontBold;
        fontFront.FontItalic = eng.fontItalic;
        cpMB.IMColor = eng.MagneticBandColor;
        mbImage.Value = eng.MagneticBandImage;
        commonOptions.FillColor = eng.FillColor;
        commonOptions.BorderColor = eng.BorderColor;
        commonOptions.DpiValue = eng.Dpi;
        commonOptions.ScriptTag = eng.Tag;
    }

    protected override void doAnteprima()
    {
        setEngineParameters();
        AggiornaAnteprima(image.Image);
    }
}
