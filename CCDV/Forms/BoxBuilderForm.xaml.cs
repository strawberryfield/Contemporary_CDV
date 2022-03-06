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

using Casasoft.CCDV.Engines;
using Casasoft.CCDV.JSON;

namespace Casasoft.CCDV.UI;

public enum BoxTypes { Box, Folder }

/// <summary>
/// Interaction logic for BoxBuilderForm.xaml
/// </summary>
public partial class BoxBuilderForm : BaseForm
{
    private BoxTypes boxType;

    public BoxBuilderForm() : this(BoxTypes.Box) { }
    public BoxBuilderForm(BoxTypes type) : base()
    {
        InitializeComponent();
        boxType = type;
        switch (type)
        {
            case BoxTypes.Box:
                this.Title = "Creazione scatola";
                engine = new ScatolaEngine();
                break;
            case BoxTypes.Folder:
                this.Title = "Creazione cartellina";
                engine = new FolderEngine();
                break;
            default:
                break;
        }
    }

    protected override void setEngineParameters()
    {
        base.setEngineParameters();
        IBaseBuilderEngine eng = (IBaseBuilderEngine)engine;
        BaseBuilder builder = (BaseBuilder)eng.Builder;
        builder.fillColor = commonOptions.FillColor;
        builder.borderColor = commonOptions.BorderColor;
        eng.Dpi = commonOptions.DpiValue;
        builder.borderText = boxImages.BorderText;
        builder.font = boxImages.Font;
        builder.fontBold = boxImages.FontBold;
        builder.fontItalic = boxImages.FontItalic;
        builder.isHorizontal = boxImages.isHorizontal;
        builder.targetType = boxImages.TargetType;
        builder.PaperFormat = paperFormat.PaperFormat;

        builder.makeEmptyImages();
        if (boxImages.UseTestImages) builder.CreateTestImages();
        builder.SetFrontImage(boxImages.FrontImage);
        builder.SetBackImage(boxImages.BackImage);
        builder.SetTopImage(boxImages.TopImage);
        builder.SetBottomImage(boxImages.BottomImage);
        builder.SetLeftImage(boxImages.LeftImage);
        builder.SetRightImage(boxImages.RightImage);
    }

    protected override void loadJson(string json)
    {
        base.loadJson(json);
        BaseBuilderEngine eng = (BaseBuilderEngine)engine;
        BaseBuilder builder = (BaseBuilder)eng.Builder;
        commonOptions.FillColor = eng.FillColor;
        commonOptions.BorderColor = eng.BorderColor;
        commonOptions.DpiValue = eng.Dpi;

        boxImages.BorderText = builder.borderText;
        boxImages.Font = builder.font;
        boxImages.FontBold = builder.fontBold;
        boxImages.FontItalic = builder.fontItalic;
        boxImages.isHorizontal = builder.isHorizontal;
        boxImages.TargetType = builder.targetType;
        paperFormat.PaperFormat = builder.PaperFormat;

        boxImages.FrontImage = builder.frontImagePath;
        boxImages.BackImage = builder.backImagePath;
        boxImages.TopImage = builder.topImagePath;
        boxImages.BottomImage = builder.bottomImagePath;
        boxImages.LeftImage = builder.leftImagePath;
        boxImages.RightImage = builder.rightImagePath;
    }

    protected override void doAnteprima()
    {
        base.doAnteprima();
        AggiornaAnteprima(image.Image);
    }

}
