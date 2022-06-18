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
using System.Windows.Media;

namespace Casasoft.CCDV.UI;

/// <summary>
/// Interaction logic for ColorPickerLabelControl.xaml
/// </summary>
public partial class ColorPickerLabelControl : UserControl
{
    public ColorPickerLabelControl()
    {
        InitializeComponent();
    }

    public MagickColor IMColor
    {
        get => Utils.ColorFromPicker(colorPicker);
        set
        {
            if (value is not null)
            {
                colorPicker.SelectedColor = Utils.ColorFromMagick(value);
            }
        }
    }

    public Color SelectedColor
    {
        get => colorPicker.SelectedColor;
        set => colorPicker.SelectedColor = value;
    }

    public bool ShowAlpha
    {
        get => colorPicker.ShowAlpha;
        set => colorPicker.ShowAlpha = value;
    }

    public string? Caption
    {
        get => label.Content.ToString();
        set => label.Content = value;
    }
}
