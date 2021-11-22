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
using System.Windows;

namespace Casasoft.CCDV.UI;

/// <summary>
/// Interaction logic for MontaggioFotoForm.xaml
/// </summary>
public partial class MontaggioFotoForm : BaseForm
{
    public MontaggioFotoForm() : base()
    {
        InitializeComponent();
        engine = new MontaggioFotoEngine();
    }

    protected override void setEngineParameters()
    {
        base.setEngineParameters();
        addAllFiles();
        MontaggioFotoEngine eng = (MontaggioFotoEngine)engine;
        eng.FillColor = commonOptions.FillColor;
        eng.BorderColor = commonOptions.BorderColor;
        eng.Dpi = commonOptions.DpiValue;
        eng.WithBorder = (bool)chkWithBorders.IsChecked;
        eng.FullSize = (bool)chkFullSize.IsChecked; 
        eng.Trim = (bool)chkTrim.IsChecked;       
    }

    protected override void doAnteprima()
    {
        base.doAnteprima();
        AggiornaAnteprima(image);
    }

}