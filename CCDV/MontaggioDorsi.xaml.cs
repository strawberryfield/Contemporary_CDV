﻿// copyright (c) 2020-2021 Roberto Ceccarelli - Casasoft
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
using ImageMagick;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Casasoft.CCDV.UI;

/// <summary>
/// Interaction logic for MontaggioDorsi.xaml
/// </summary>
public partial class MontaggioDorsi : Window
{
    private MontaggioDorsiEngine engine;

    public MontaggioDorsi()
    {
        InitializeComponent();
        engine = new MontaggioDorsiEngine();
    }

    private void filename1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        TextBox tb = (TextBox)sender;
        OpenFileDialog openFileDialog = new();
        openFileDialog.Filter = "Image files (*.jpg;*.jpeg;*.png;*.psd)|*.jpg;*.jpeg;*.png;*.psd|All files (*.*)|*.*";
        openFileDialog.Title = "Immagine dorso";
        if (openFileDialog.ShowDialog() == true)
            tb.Text = openFileDialog.FileName;
    }

    private void btnUpdate_Click(object sender, RoutedEventArgs e)
    {
        engine.Dpi = 72;
        engine.FilesList.Clear();
        engine.FilesList.Add(filename1.Text);
        image.Source = engine.GetResult(true).ToBitmapSource();
    }
}
