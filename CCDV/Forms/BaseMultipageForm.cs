﻿// copyright (c) 2020-2022 Roberto Ceccarelli - Casasoft
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
using Casasoft.Xaml.Controls;
using ImageMagick;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Casasoft.CCDV.UI;

public partial class BaseMultipageForm : BaseForm
{
    protected List<MagickImage>? bm;

    public BaseMultipageForm()
    {
        engine = new BaseEngine();
        waitForm = new WaitForm();

        bwAnteprima = new BackgroundWorker();
        bwAnteprima.DoWork += new System.ComponentModel.DoWorkEventHandler(bwAnteprima_DoWork);
        bwAnteprima.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bwAnteprima_RunWorkerCompleted);

        bwRender = new BackgroundWorker();
        bwRender.DoWork += new System.ComponentModel.DoWorkEventHandler(bwAnteprima_DoWork);
        bwRender.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bwRender_RunWorkerCompleted);

        bwPrint = new BackgroundWorker();
        bwPrint.DoWork += new System.ComponentModel.DoWorkEventHandler(bwAnteprima_DoWork);
        bwPrint.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bwPrint_RunWorkerCompleted);
    }

    #region backgroudworkers
    private void bwAnteprima_DoWork(object? sender, DoWorkEventArgs e)
    {
        e.Result = engine.GetResults(true);
    }

    private void bwAnteprima_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
    {
        bm = (List<MagickImage>?)e.Result;
        if (bm is not null)
        {
            image.Source = bm[0].ToBitmapSource();
        }
        waitForm.Close();
    }

    private void bwRender_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
    {
        bm = (List<MagickImage>?)e.Result;
        waitForm.Close();

        if (bm is not null)
        {
            SaveFileDialog sd = SaveDialog();
            if (!string.IsNullOrWhiteSpace(sd.FileName))
            {
                string ext = Path.GetExtension(sd.FileName);
                string basename = Path.Combine(
                    Path.GetDirectoryName(sd.FileName),
                    Path.GetFileNameWithoutExtension(sd.FileName));

                int max = bm.Count;
                int i = 0;
                foreach (MagickImage img in bm)
                {
                    i++;
                    string filename = $"{basename}-{i}of{max}{ext}";
                    engine.SetImageInfo(filename, img);
                    engine.SetImageParameters(img);
                    img.Write(filename);
                }
            }
        }
    }

    private void bwPrint_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
    {
        bm = (List<MagickImage>?)e.Result;
        waitForm.Close();

        if (bm is not null)
        {
            PrintDialog pd = new();
            if (pd.ShowDialog() == true)
            {
                int max = bm.Count;
                int i = 0;

                foreach (MagickImage img in bm)
                {
                    DrawingVisual vis = new();
                    using (DrawingContext dc = vis.RenderOpen())
                    {
                        dc.DrawImage(img.ToBitmapSource(), new Rect
                        {
                            Width = img.Width / engine.Dpi * 96,
                            Height = img.Height / engine.Dpi * 96
                        });
                    }
                    i++;
                    pd.PrintVisual(vis, $"Print Image {i} of {max}");
                }
            }
        }
    }
    #endregion
}
