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

using ImageMagick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Casasoft.CCDV.UI;

/// <summary>
/// Interaction logic for GravityControl.xaml
/// </summary>
public partial class GravityControl : UserControl
{
    public record ComboBoxPairs(string _Key, Gravity _Value);

    public GravityControl()
    {
        InitializeComponent();
    }

    private void UserControl_Initialized(object sender, EventArgs e)
    {
        txtGravity.Items.Clear();
        foreach (Gravity s in Enum.GetValues(typeof(Gravity)).Cast<Gravity>())
        {
            if (s != 0)
                txtGravity.Items.Add(new ComboBoxPairs(s.ToString(), s));
        }
        txtGravity.SelectedIndex = (int)Gravity.Center - 1;
    }

    public Gravity gravity
    {
        get => (Gravity)txtGravity.SelectedValue;
        set => txtGravity.SelectedIndex = (int)value - 1;
    }

}
