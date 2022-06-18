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

using System.Windows.Controls;

namespace Casasoft.CCDV.UI;

/// <summary>
/// Interaction logic for BoxImagesControl.xaml
/// </summary>
public partial class BoxImagesControl : UserControl
{
    public BoxImagesControl()
    {
        InitializeComponent();
    }

    public string FrontImage { get => txtFront.Value; set => txtFront.Value = value; }
    public string BackImage { get => txtBack.Value; set => txtBack.Value = value; }
    public string LeftImage { get => txtLeft.Value; set => txtLeft.Value = value; }
    public string RightImage { get => txtRight.Value; set => txtRight.Value = value; }
    public string TopImage { get => txtTop.Value; set => txtTop.Value = value; }
    public string BottomImage { get => txtBottom.Value; set => txtBottom.Value = value; }

    public bool UseTestImages { get => isChecked(chkTestImages); set => chkTestImages.IsChecked = value; }
    public int Thickness { get => txtThickness.Value; set => txtThickness.Value = value; }
    public string BorderText { get => txtBorderText.Text; set => txtBorderText.Text = value; }

    public string Font { get => ctrlFont.Font; set => ctrlFont.Font = value; }
    public bool FontBold { get => ctrlFont.FontBold; set => ctrlFont.FontBold = value; }
    public bool FontItalic { get => ctrlFont.FontItalic; set => ctrlFont.FontItalic = value; }

    public TargetType TargetType
    {
        get => isChecked(rbCDV) ? TargetType.cdv : TargetType.cc;
        set
        {
            rbCDV.IsChecked = value == TargetType.cdv;
            rbCC.IsChecked = value == TargetType.cc;
        }
    }
    public bool isHorizontal
    {
        get => isChecked(rbHorizontal);
        set
        {
            rbHorizontal.IsChecked = value;
            rbVertical.IsChecked = !value;
        }
    }

    private bool isChecked(RadioButton rb)
    {
        bool ret = false;
        if (rb.IsChecked is not null) ret = (bool)rb.IsChecked;
        return ret;
    }

    private bool isChecked(CheckBox rb)
    {
        bool ret = false;
        if (rb.IsChecked is not null) ret = (bool)rb.IsChecked;
        return ret;
    }
}
