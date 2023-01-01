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
using System.IO;
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

    public int DpiValue
    {
        get => txtDPI.Value; 
        set => txtDPI.Value = value;
    }

    public MagickColor BorderColor
    {
        get => cpBorder.IMColor;
        set => cpBorder.IMColor = value;
    }
    public MagickColor FillColor
    {
        get => cpFill.IMColor;
        set => cpFill.IMColor = value;
    }

    public string ScriptTag
    {
        get => txtTag.Text;
        set => txtTag.Text = value;
    }

    public string Script
    {
        get
        {
            string ret = string.Empty;
            if(!string.IsNullOrWhiteSpace(txtScript.Value))
            {
                ret = File.ReadAllText(txtScript.Value);
            }
            return ret;
        }
    }

    private void MenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        MenuItem mi = (MenuItem)sender;
        txtDPI.Value = Convert.ToInt16(mi.Header);
    }
}
