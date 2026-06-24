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

using System;
using System.Windows;
using System.Windows.Controls;

namespace Casasoft.CCDV.UI;

/// <summary>
/// Interaction logic for BoxPaperSelectorControl.xaml
/// </summary>
/// <remarks>
/// Dedicated paper-format selector for <see cref="BoxBuilderForm"/> (Scatola/Cartella).
/// Kept separate from <see cref="ThickPaperSelectorControl"/> because the latter is
/// also shared by <see cref="MontaggioDorsiForm"/> and <see cref="CubettiForm"/>, which
/// must NOT expose <see cref="PaperFormats.Medium13x17"/>: <c>MontaggioDorsiEngine</c>
/// intentionally has no specific handling for it, and <c>CubettiCommandLine</c> never
/// supported it. Only <c>BaseBuilderCommandLine</c> (Scatola/Cartella) and
/// <c>MontaggioFotoCommandLine</c> advertise <c>--paper=Medium13x17</c>.
/// </remarks>
public partial class BoxPaperSelectorControl : UserControl
{
    public BoxPaperSelectorControl()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Raised when the selected paper format changes.
    /// </summary>
    public event EventHandler? FormatChanged;

    private void Format_Checked(object sender, RoutedEventArgs e)
        => FormatChanged?.Invoke(this, EventArgs.Empty);

    public PaperFormats PaperFormat
    {
        get
        {
            if (BaseForm.isChecked(rbMedium)) return PaperFormats.Medium;
            if (BaseForm.isChecked(rbMedium13x17)) return PaperFormats.Medium13x17;
            if (BaseForm.isChecked(rb20x30)) return PaperFormats.Large20x30;
            if (BaseForm.isChecked(rbA4)) return PaperFormats.A4;
            return PaperFormats.Large;
        }
        set
        {
            rbLarge.IsChecked = value == PaperFormats.Large;
            rbMedium.IsChecked = value == PaperFormats.Medium;
            rbMedium13x17.IsChecked = value == PaperFormats.Medium13x17;
            rb20x30.IsChecked = value == PaperFormats.Large20x30;
            rbA4.IsChecked = value == PaperFormats.A4;
        }
    }
}