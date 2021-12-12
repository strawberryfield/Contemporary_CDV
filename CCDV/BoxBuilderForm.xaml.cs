// copyright (c) 2020-2021 Roberto Ceccarelli - Casasoft
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
/// Interaction logic for BoxBuilderForm.xaml
/// </summary>
public partial class BoxBuilderForm : BaseForm
{
    public BoxBuilderForm() : base()
    {
        InitializeComponent();
        engine = new ScatolaEngine();
    }

    protected override void setEngineParameters()
    {
        base.setEngineParameters();
        ScatolaEngine eng = (ScatolaEngine)engine;
        ScatolaBuilder builder = (ScatolaBuilder)eng.Builder;
        builder.fillColor = commonOptions.FillColor;
        builder.borderColor = commonOptions.BorderColor;
        eng.Dpi = commonOptions.DpiValue;
        builder.borderText = boxImages.BorderText;
        builder.font = boxImages.Font;
        builder.fontBold = boxImages.FontBold;

        builder.makeEmptyImages();
        if (boxImages.UseTestImages) builder.CreateTestImages();
        builder.SetFrontImage(boxImages.FrontImage);
        builder.SetBackImage(boxImages.BackImage);
        builder.SetTopImage(boxImages.TopImage);
        builder.SetBottomImage(boxImages.BottomImage);
        builder.SetLeftImage(boxImages.LeftImage);
        builder.SetRightImage(boxImages.RightImage);
    }

    protected override void doAnteprima()
    {
        base.doAnteprima();
        AggiornaAnteprima(image);
    }

}
