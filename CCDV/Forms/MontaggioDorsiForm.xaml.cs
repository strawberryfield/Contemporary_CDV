﻿// copyright (c) 2020-2023 Roberto Ceccarelli - Casasoft
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

namespace Casasoft.CCDV.UI;

/// <summary>
/// Interaction logic for MontaggioDorsiForm.xaml
/// </summary>
public partial class MontaggioDorsiForm : BaseForm
{
    public MontaggioDorsiForm() : base()
    {
        InitializeComponent();
        engine = new MontaggioDorsiEngine();
    }

    protected override void setEngineParameters()
    {
        base.setEngineParameters();
        MontaggioDorsiEngine eng = (MontaggioDorsiEngine)engine;
        addAllFiles();
        eng.FillColor = commonOptions.FillColor;
        eng.BorderColor = commonOptions.BorderColor;
        eng.Dpi = commonOptions.DpiValue;
        eng.Script = commonOptions.Script;
        eng.Tag = commonOptions.ScriptTag;
        eng.PaperFormat = paperFormat.PaperFormat;
        eng.CanvasGravity = txtGravity.gravity;
    }

    protected override void loadJson(string json)
    {
        base.loadJson(json);
        MontaggioDorsiEngine eng = (MontaggioDorsiEngine)engine;
        commonOptions.FillColor = eng.FillColor;
        commonOptions.BorderColor = eng.BorderColor;
        commonOptions.DpiValue = eng.Dpi;
        commonOptions.ScriptTag = eng.Tag;
        paperFormat.PaperFormat = eng.PaperFormat;
        txtGravity.gravity = eng.CanvasGravity;

        addFile(1, filename1);
        addFile(2, filename2);
        addFile(3, filename3);
        addFile(4, filename4);
        addFile(5, filename5);
        addFile(6, filename6);
        addFile(7, filename7);
        addFile(8, filename8);
    }

    protected override void doAnteprima()
    {
        base.doAnteprima();
        AggiornaAnteprima(image.Image);
    }
}
