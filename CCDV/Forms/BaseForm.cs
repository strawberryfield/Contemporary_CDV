// copyright (c) 2020-2022 Roberto Ceccarelli - Casasoft
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
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Casasoft.CCDV.UI;

/// <summary>
/// Common interaction logic
/// </summary>
public partial class BaseForm : Window
{
    protected IEngine engine;

    protected BackgroundWorker bwAnteprima;
    protected BackgroundWorker bwRender;
    protected BackgroundWorker bwPrint;
    protected WaitForm waitForm;
    private Image image;

    public BaseForm()
    {
        engine = new BaseEngine();
        waitForm = new WaitForm();
        image = new();

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

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        doAnteprima();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        setEngineParameters();
        bwPrint.RunWorkerAsync();
        waitForm = new WaitForm();
        waitForm.Owner = this;
        waitForm.ShowDialog();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        setEngineParameters();
        bwRender.RunWorkerAsync();
        waitForm = new WaitForm();
        waitForm.Owner = this;
        waitForm.ShowDialog();
    }

    protected void btnSaveJson_Click(object sender, EventArgs e)
    {
        setEngineParameters();
        SaveFileDialog sd = new();
        sd.Filter = "json data file (*.json)|*.json|All files (*.*)|*.*";
        sd.Title = "Salvataggio parametri immagine";
        sd.DefaultExt = "json";
        sd.AddExtension = true;
        sd.OverwritePrompt = true;
        sd.ShowDialog();
        if (!string.IsNullOrWhiteSpace(sd.FileName))
        {
            File.WriteAllText(sd.FileName, engine.GetJsonParams());
        }
    }

    protected void btnOpenJson_Click(object sender, EventArgs e)
    {
        OpenFileDialog sd = new();
        sd.Filter = "json data file (*.json)|*.json|All files (*.*)|*.*";
        sd.Title = "Apertura parametri immagine";
        sd.DefaultExt = "json";
        sd.AddExtension = true;
        sd.ShowDialog();
        if (!string.IsNullOrWhiteSpace(sd.FileName))
        {
            loadJson(File.ReadAllText(sd.FileName));
        }
    }

    protected virtual void loadJson(string json)
    {
        engine.SetJsonParams(json);
    }

    protected void addFile(int num, FileTextBox ftb)
    {
        if (engine.FilesList.Count >= num)
        {
            ftb.Value = engine.FilesList[num - 1];
        }
    }

    protected virtual void setEngineParameters()
    {
        engine.FilesList.Clear();
    }

    protected virtual void doAnteprima()
    {
        setEngineParameters();
        //        engine.Dpi = 120;
    }

    protected void AggiornaAnteprima(Image img)
    {
        image = img;
        image.Source = null;
        bwAnteprima.RunWorkerAsync();
        waitForm = new WaitForm();
        waitForm.Owner = this;
        waitForm.ShowDialog();
    }

    private void bwAnteprima_DoWork(object? sender, DoWorkEventArgs e)
    {
        e.Result = engine.GetResult(true);
    }

    private void bwAnteprima_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
    {
        MagickImage? bm = (MagickImage?)e.Result;
        if (bm is not null) image.Source = bm.ToBitmapSource();
        waitForm.Close();
    }

    private void bwRender_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
    {
        MagickImage? bm = (MagickImage?)e.Result;
        waitForm.Close();

        if (bm is not null)
        {
            SaveFileDialog sd = new();
            sd.Filter = "jpeg Image (*.jpg;*.jpeg)|*.jpg;*.jpeg|All files (*.*)|*.*";
            sd.Title = "Salvataggio immagine";
            sd.DefaultExt = "jpg";
            sd.AddExtension = true;
            sd.OverwritePrompt = true;
            sd.ShowDialog();
            if (!string.IsNullOrWhiteSpace(sd.FileName))
            {
                engine.SetImageInfo(sd.FileName, bm);
                engine.SetImageParameters(bm);
                bm.Write(sd.FileName);
            }
        }
    }

    private void bwPrint_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
    {
        MagickImage? bm = (MagickImage?)e.Result;
        waitForm.Close();

        if (bm is not null)
        {
            DrawingVisual vis = new DrawingVisual();
            using (DrawingContext dc = vis.RenderOpen())
            {
                dc.DrawImage(bm.ToBitmapSource(), new Rect
                {
                    Width = bm.Width / engine.Dpi * 96,
                    Height = bm.Height / engine.Dpi * 96
                });
            }

            PrintDialog pd = new();
            if (pd.ShowDialog() == true)
            {
                pd.PrintVisual(vis, "Print Image");
            }
        }
    }

    public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
    {
        if (depObj is null)
            yield return null;

        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
        {
            var child = VisualTreeHelper.GetChild(depObj, i);

            if (child is not null && child is T)
                yield return (T)child;

            foreach (T childOfChild in FindVisualChildren<T>(child))
                yield return childOfChild;
        }
    }

    protected void addAllFiles()
    {
        foreach (FileTextBox? tb in FindVisualChildren<FileTextBox>(this))
        {
            if (!string.IsNullOrWhiteSpace(tb.Value))
            {
                if (tb.OpenFileDialogTitle != "Script")
                {
                    engine.FilesList.Add(tb.Value);
                }
            }
        }
    }

    public static bool isChecked(RadioButton rb)
    {
        bool ret = false;
        if (rb.IsChecked is not null) ret = (bool)rb.IsChecked;
        return ret;
    }

    public static bool isChecked(CheckBox rb)
    {
        bool ret = false;
        if (rb.IsChecked is not null) ret = (bool)rb.IsChecked;
        return ret;
    }
}
