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
/// Interaction logic for ThickPaperSelectorControl.xaml
/// </summary>
public partial class ThickPaperSelectorControl : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ThickPaperSelectorControl"/> class.
    /// </summary>
    /// <remarks>
    /// Initializes component and wires up UI defined in XAML. The control exposes the
    /// <see cref="PaperFormat"/> property to get/set the currently selected format and
    /// raises <see cref="FormatChanged"/> when the selection changes.
    /// </remarks>
    public ThickPaperSelectorControl()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Raised when the selected paper format changes.
    /// </summary>
    /// <remarks>
    /// This event is raised from the internal radio-button checked handler
    /// (<see cref="Format_Checked"/>) whenever the user selects a different format.
    /// Subscribers should treat this as a UI notification; the control's <see cref="PaperFormat"/>
    /// property already reflects the new selection when the event is raised.
    /// </remarks>
    public event EventHandler? FormatChanged;

    /// <summary>
    /// Internal radio-button checked handler that forwards the change via <see cref="FormatChanged"/>.
    /// </summary>
    /// <param name="sender">The radio button that raised the event.</param>
    /// <param name="e">Event arguments provided by WPF.</param>
    private void Format_Checked(object sender, RoutedEventArgs e)
        => FormatChanged?.Invoke(this, EventArgs.Empty);

    /// <summary>
    /// Gets or sets the currently selected <see cref="PaperFormats"/> for this control.
    /// </summary>
    /// <remarks>
    /// The getter inspects the control's radio buttons to determine the active format.
    /// If none of the known radio buttons are checked the getter falls back to <see cref="PaperFormats.Large"/>.
    /// The setter updates the radio buttons to reflect the specified value.
    /// </remarks>
    public PaperFormats PaperFormat
    {
        get
        {
            if (BaseForm.isChecked(rbMedium)) return PaperFormats.Medium;
            if (BaseForm.isChecked(rbA4)) return PaperFormats.A4;
            if (BaseForm.isChecked(rb20x30)) return PaperFormats.Large20x30;
            return PaperFormats.Large;
        }
        set
        {
            rbMedium.IsChecked = value == PaperFormats.Medium;
            rbLarge.IsChecked = value == PaperFormats.Large;
            rbA4.IsChecked = value == PaperFormats.A4;
            rb20x30.IsChecked = value == PaperFormats.Large20x30;
        }
    }

    /// <summary>
    /// Number of image slots for the currently selected format.
    /// Medium = 3, Large / Large20x30 = 6 (4 portrait + 2 landscape),
    /// A4 = 8 (4 portrait + 4 landscape).
    /// </summary>
    public int SlotCount => PaperFormat switch
    {
        PaperFormats.Medium => 3,
        PaperFormats.Large => 6,
        PaperFormats.Large20x30 => 6,
        PaperFormats.A4 => 8,
        _ => 6
    };
}
