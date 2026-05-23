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
/// Interaction logic for MontaggioDorsiForm.xaml
/// </summary>
public partial class MontaggioDorsiForm : BaseForm
{
    private FileTextBox[] _slots;

    public MontaggioDorsiForm() : base()
    {
        InitializeComponent();
        engine = new MontaggioDorsiEngine();

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
    /// Shows exactly <see cref="ThickPaperSelectorControl.SlotCount"/> file
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
        MontaggioDorsiEngine eng = (MontaggioDorsiEngine)engine;

        int count = paperFormat.SlotCount;
        for (int i = 0; i < count; i++)
        {
            if (!string.IsNullOrWhiteSpace(_slots[i].Value))
                eng.FilesList.Add(_slots[i].Value);
        }

        eng.FillColor = commonOptions.FillColor;
        eng.BorderColor = commonOptions.BorderColor;
        eng.Dpi = (uint)commonOptions.DpiValue;
        eng.Script = commonOptions.Script;
        eng.Tag = commonOptions.ScriptTag;
        eng.PaperFormat = paperFormat.PaperFormat;
        eng.CanvasGravity = txtGravity.gravity;
    }

    /// <summary>
    /// Loads form state from a JSON payload by delegating to the base implementation,
    /// then maps the deserialized engine state into the form controls.
    /// </summary>
    /// <param name="json">JSON string previously produced by the engine or saved state. Parsing is handled by the base implementation.</param>
    /// <remarks>
    /// This method performs the following mappings after calling <c>base.loadJson(json)</c>:
    /// - Casts the form's engine to <c>MontaggioDorsiEngine</c>.
    /// - Copies rendering and script-related settings into <c>commonOptions</c>:
    ///   <c>FillColor</c>, <c>BorderColor</c>, <c>DpiValue</c> (from <c>eng.Dpi</c>), and <c>ScriptTag</c>.
    /// - Applies the engine's paper format to <c>paperFormat.PaperFormat</c> and then calls <c>UpdateSlotVisibility()</c>
    ///   so the UI can show/hide slots based on the new format.
    /// - Restores the canvas gravity into <c>txtGravity.gravity</c>.
    /// - Populates the UI file slots from <c>eng.FilesList</c>, copying at most <c>_slots.Length</c> entries.
    /// </remarks>
    protected override void loadJson(string json)
    {
        base.loadJson(json);
        MontaggioDorsiEngine eng = (MontaggioDorsiEngine)engine;

        // Map common rendering and script settings from the engine into the form controls
        commonOptions.FillColor = eng.FillColor;
        commonOptions.BorderColor = eng.BorderColor;
        commonOptions.DpiValue = (int)eng.Dpi;
        commonOptions.ScriptTag = eng.Tag;

        // Paper format affects available slots/visibility
        paperFormat.PaperFormat = eng.PaperFormat;
        UpdateSlotVisibility();

        // Restore canvas gravity setting
        txtGravity.gravity = eng.CanvasGravity;

        // Populate file slots from the engine's file list, but do not exceed the UI slot count
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
