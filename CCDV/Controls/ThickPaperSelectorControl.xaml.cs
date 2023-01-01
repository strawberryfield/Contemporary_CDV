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

using System.Windows.Controls;

namespace Casasoft.CCDV.UI;

/// <summary>
/// Interaction logic for ThickPaperSelectorControl.xaml
/// </summary>
public partial class ThickPaperSelectorControl : UserControl
{
    public ThickPaperSelectorControl()
    {
        InitializeComponent();
    }

    public PaperFormats PaperFormat
    {
        get
        {
            PaperFormats ret = PaperFormats.Large;
            if (BaseForm.isChecked(rbMedium)) ret = PaperFormats.Medium;
            if (BaseForm.isChecked(rbA4)) ret = PaperFormats.A4;
            return ret;
        }
        set
        {
            rbMedium.IsChecked = value == PaperFormats.Medium;
            rbLarge.IsChecked = value == PaperFormats.Large;
            rbA4.IsChecked = value == PaperFormats.A4;
        }
    }
}
