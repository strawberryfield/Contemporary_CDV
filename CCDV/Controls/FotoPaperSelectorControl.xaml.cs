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
/// Interaction logic for FotoPaperSelectorControl.xaml
/// </summary>
public partial class FotoPaperSelectorControl : UserControl
{
    public FotoPaperSelectorControl()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Raised when the selected format changes.
    /// </summary>
    public event EventHandler? FormatChanged;

    private void Format_Checked(object sender, RoutedEventArgs e)
        => FormatChanged?.Invoke(this, EventArgs.Empty);

    /// <summary>
    /// Gets or sets the currently selected paper format based on the state of the radio buttons in the control.
    /// </summary>
    /// <remarks>
    /// This property provides a synchronized interface between the visual radio button controls and the underlying 
    /// <see cref="PaperFormats"/> enum value.
    /// 
    /// The getter examines each radio button in sequence and returns the corresponding paper format.
    /// If no radio button is checked, the default value <see cref="PaperFormats.Small"/> is returned.
    /// 
    /// The setter checks the appropriate radio button that corresponds to the specified paper format value,
    /// automatically unchecking any previously selected radio button due to mutual exclusivity.
    /// </remarks>
    /// <value>
    /// A <see cref="PaperFormats"/> enum value representing the selected paper format:
    /// <list type="bullet">
    /// <item><see cref="PaperFormats.Panorama"/> - when rbPanorama is checked</item>
    /// <item><see cref="PaperFormats.Medium"/> - when rbMedium is checked</item>
    /// <item><see cref="PaperFormats.Medium13x17"/> - when rbMedium13x17 is checked</item>
    /// <item><see cref="PaperFormats.Large"/> - when rbLarge is checked</item>
    /// <item><see cref="PaperFormats.Large20x30"/> - when rb20x30 is checked</item>
    /// <item><see cref="PaperFormats.A4"/> - when rbA4 is checked</item>
    /// <item><see cref="PaperFormats.Small"/> - when rbSmall is checked or no button is selected (default)</item>
    /// </list>
    /// </value>
    public PaperFormats PaperFormat
    {
        get
        {
            if (BaseForm.isChecked(rbPanorama)) return PaperFormats.Panorama;
            if (BaseForm.isChecked(rbMedium)) return PaperFormats.Medium;
            if (BaseForm.isChecked(rbMedium13x17)) return PaperFormats.Medium13x17;
            if (BaseForm.isChecked(rbLarge)) return PaperFormats.Large;
            if (BaseForm.isChecked(rb20x30)) return PaperFormats.Large20x30;
            if (BaseForm.isChecked(rbA4)) return PaperFormats.A4;
            return PaperFormats.Small;
        }
        set
        {
            rbSmall.IsChecked = value == PaperFormats.Small;
            rbPanorama.IsChecked = value == PaperFormats.Panorama;
            rbMedium.IsChecked = value == PaperFormats.Medium;
            rbMedium13x17.IsChecked = value == PaperFormats.Medium13x17;
            rbLarge.IsChecked = value == PaperFormats.Large;
            rb20x30.IsChecked = value == PaperFormats.Large20x30;
            rbA4.IsChecked = value == PaperFormats.A4;
        }
    }

    /// <summary>
    /// Number of image slots for the currently selected format.
    /// Small/Panorama/Medium13x17 = 2, Medium = 3, Large/Large20x30 = 6, A4 = 8.
    /// </summary>
    public int SlotCount => PaperFormat switch
    {
        PaperFormats.Medium => 3,
        PaperFormats.Large => 6,
        PaperFormats.Large20x30 => 6,
        PaperFormats.A4 => 8,
        _ => 2   // Small, Panorama, Medium13x17
    };
}
