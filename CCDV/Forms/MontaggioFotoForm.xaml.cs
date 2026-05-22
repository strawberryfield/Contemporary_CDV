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

using Casasoft.Xaml.Controls;
using Casasoft.CCDV.Engines;
using System;
using System.Windows;

namespace Casasoft.CCDV.UI;

/// <summary>
/// Interaction logic for MontaggioFotoForm.xaml
/// </summary>
public partial class MontaggioFotoForm : BaseForm
{
    // All file slots in order — makes the show/hide loop trivial.
    private FileTextBox[] _slots;

    public MontaggioFotoForm() : base()
    {
        InitializeComponent();
        engine = new MontaggioFotoEngine();

        _slots = new[]
        {
            filename1, filename2, filename3, filename4,
            filename5, filename6, filename7, filename8
        };

        UpdateSlotVisibility();
    }

    // -----------------------------------------------------------------------
    // Format selector
    // -----------------------------------------------------------------------

    private void paperFormat_FormatChanged(object sender, EventArgs e)
        => UpdateSlotVisibility();

    /// <summary>
    /// Shows exactly <see cref="FotoPaperSelectorControl.SlotCount"/> file
    /// boxes and collapses the rest.
    /// </summary>
    private void UpdateSlotVisibility()
    {
        int count = paperFormat.SlotCount;
        for (int i = 0; i < _slots.Length; i++)
            _slots[i].Visibility = i < count ? Visibility.Visible : Visibility.Collapsed;
    }

    // -----------------------------------------------------------------------
    // Engine ↔ UI
    // -----------------------------------------------------------------------

    protected override void setEngineParameters()
    {
        base.setEngineParameters();
        MontaggioFotoEngine eng = (MontaggioFotoEngine)engine;

        // Add only the visible (active) slots.
        int count = paperFormat.SlotCount;
        for (int i = 0; i < count; i++)
        {
            if (!string.IsNullOrWhiteSpace(_slots[i].Value))
                eng.FilesList.Add(_slots[i].Value);
        }

        eng.FillColor      = commonOptions.FillColor;
        eng.BorderColor    = commonOptions.BorderColor;
        eng.Dpi            = (uint)commonOptions.DpiValue;
        eng.Script         = commonOptions.Script;
        eng.Tag            = commonOptions.ScriptTag;
        eng.PaperFormat    = paperFormat.PaperFormat;
        eng.WithBorder     = isChecked(chkWithBorders);
        eng.FullSize       = isChecked(chkFullSize);
        eng.Trim           = isChecked(chkTrim);
        eng.Padding        = (uint)txtPadding.Value;
        eng.CanvasGravity  = txtGravity.gravity;
    }

    protected override void loadJson(string json)
    {
        base.loadJson(json);
        MontaggioFotoEngine eng = (MontaggioFotoEngine)engine;

        commonOptions.FillColor   = eng.FillColor;
        commonOptions.BorderColor = eng.BorderColor;
        commonOptions.DpiValue    = (int)eng.Dpi;
        commonOptions.ScriptTag   = eng.Tag;

        paperFormat.PaperFormat   = eng.PaperFormat;
        UpdateSlotVisibility();

        chkWithBorders.IsChecked  = eng.WithBorder;
        chkFullSize.IsChecked     = eng.FullSize;
        chkTrim.IsChecked         = eng.Trim;
        txtPadding.Value          = (int)eng.Padding;
        txtGravity.gravity        = eng.CanvasGravity;

        // Populate visible slots from FilesList.
        int count = Math.Min(eng.FilesList.Count, _slots.Length);
        for (int i = 0; i < count; i++)
            _slots[i].Value = eng.FilesList[i];
    }

    protected override void doAnteprima()
    {
        base.doAnteprima();
        AggiornaAnteprima(image.Image);
    }
}
