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

using ImageMagick;
using System.Windows.Controls;

namespace Casasoft.CCDV.UI;

/// <summary>
/// Interaction logic for CommonOptionsControl.xaml
/// </summary>
public partial class CommonOptionsControl : UserControl
{
    public CommonOptionsControl()
    {
        InitializeComponent();
    }

    public int DpiValue { get => txtDPI.Value; set => txtDPI.Value = value; }

    public MagickColor BorderColor
    {
        get => Utils.ColorFromPicker(cpBorder);
        set
        {
            if (value != null)
            {
                cpBorder.SelectedColor = Utils.ColorFromMagick(value);
            }
        }
    }
    public MagickColor FillColor
    {
        get => Utils.ColorFromPicker(cpFill);
        set
        {
            if (value != null)
            {
                cpFill.SelectedColor = Utils.ColorFromMagick(value);
            }
        }
    }
}
