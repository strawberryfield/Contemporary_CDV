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

using System.Windows;

namespace Casasoft.CCDV.UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void btnDorsi_Click(object sender, RoutedEventArgs e)
    {
        MontaggioDorsiForm? form = new();
        form.ShowDialog();
    }

    private void btnFoto_Click(object sender, RoutedEventArgs e)
    {
        MontaggioFotoForm? form = new();
        form.ShowDialog();
    }

    private void btnScatola_Click(object sender, RoutedEventArgs e)
    {
        BoxBuilderForm form = new(BoxTypes.Box);
        form.ShowDialog();
    }

    private void btnCartellina_Click(object sender, RoutedEventArgs e)
    {
        BoxBuilderForm form = new(BoxTypes.Folder);
        form.ShowDialog();
    }

    private void btnCreditCards_Click(object sender, RoutedEventArgs e)
    {
        CreditCardForm form = new();
        form.ShowDialog();
    }
}
