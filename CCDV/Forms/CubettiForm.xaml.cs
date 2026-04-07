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

using Casasoft.CCDV.Engines;

namespace Casasoft.CCDV.UI;

/// <summary>
/// Interaction logic for CubettiForm.xaml
/// </summary>
public partial class CubettiForm : BaseMultipageForm
{
    public CubettiForm() : base()
    {
        InitializeComponent();
        engine = new CubettiEngine();
    }

    protected override void setEngineParameters()
    {
        base.setEngineParameters();
        CubettiEngine eng = (CubettiEngine)engine;
        eng.useSampleImages = isChecked(chkTestImages);
        addAllFiles();
        eng.FillColor = commonOptions.FillColor;
        eng.BorderColor = commonOptions.BorderColor;
        eng.Dpi = (uint)commonOptions.DpiValue;
        eng.Script = commonOptions.Script;
        eng.Tag = commonOptions.ScriptTag;
        eng.PaperFormat = paperFormat.PaperFormat;
        eng.Rows = (uint)txtRows.Value;
        eng.Columns = (uint)txtCols.Value;
        eng.Size = (uint)txtSize.Value;
    }

    protected override void loadJson(string json)
    {
        base.loadJson(json);
        CubettiEngine eng = (CubettiEngine)engine;
        commonOptions.FillColor = eng.FillColor;
        commonOptions.BorderColor = eng.BorderColor;
        commonOptions.DpiValue = (int)eng.Dpi;
        commonOptions.ScriptTag = eng.Tag;
        paperFormat.PaperFormat = eng.PaperFormat;
        txtRows.Value = (int)eng.Rows;
        txtCols.Value = (int)eng.Columns;
        txtSize.Value = (int)eng.Size;
        chkTestImages.IsChecked = eng.useSampleImages;

        addFile(1, filename1);
        addFile(2, filename2);
        addFile(3, filename3);
        addFile(4, filename4);
        addFile(5, filename5);
        addFile(6, filename6);
    }

    protected override void doAnteprima()
    {
        base.doAnteprima();
        AggiornaAnteprima(image.Image);
    }

}
