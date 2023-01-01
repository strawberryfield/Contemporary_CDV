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

using System;
using System.Windows;
using System.Windows.Controls;

namespace Casasoft.CCDV.UI;

/// <summary>
/// Interaction logic for CommonCommandsControl.xaml
/// </summary>
public partial class CommonCommandsControl : UserControl
{
    public CommonCommandsControl()
    {
        InitializeComponent();
    }

    public event EventHandler? DoUpdate;
    public event EventHandler? DoSave;
    public event EventHandler? DoSaveJson;
    public event EventHandler? DoOpenJson;
    public event EventHandler? DoPrint;

    private void btnUpdate_Click(object sender, RoutedEventArgs e) => DoUpdate?.Invoke(this, e);

    private void btnSave_Click(object sender, RoutedEventArgs e) => DoSave?.Invoke(this, e);

    private void btnSaveJson_Click(object sender, RoutedEventArgs e) => DoSaveJson?.Invoke(this, e);

    private void btnOpenJson_Click(object sender, RoutedEventArgs e) => DoOpenJson?.Invoke(this, e);

    private void btnPrint_Click(object sender, RoutedEventArgs e) => DoPrint?.Invoke(this, e);
}
