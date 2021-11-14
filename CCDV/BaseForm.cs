// copyright (c) 2020-2021 Roberto Ceccarelli - Casasoft
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
using ImageMagick;
using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Casasoft.CCDV.UI;

/// <summary>
/// Interaction logic for MontaggioDorsi.xaml
/// </summary>
public partial class BaseForm : Window
{
    protected IEngine engine;

    public BaseForm()
    {
        engine = new BaseEngine();
        bwAnteprima = new BackgroundWorker();
        bwAnteprima.DoWork += new System.ComponentModel.DoWorkEventHandler(bwAnteprima_DoWork);
        bwAnteprima.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bwAnteprima_RunWorkerCompleted);
    }

    protected void filename_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        TextBox tb = (TextBox)sender;
        OpenFileDialog openFileDialog = new();
        openFileDialog.Filter = "Image files (*.jpg;*.jpeg;*.png;*.psd)|*.jpg;*.jpeg;*.png;*.psd|All files (*.*)|*.*";
        openFileDialog.Title = "Immagine";
        if (openFileDialog.ShowDialog() == true)
            tb.Text = openFileDialog.FileName;
    }

    protected void btnUpdate_Click(object sender, RoutedEventArgs e)
    {
        engine.Dpi = 72;
        engine.FilesList.Clear();
        makePreview();
    }

    protected virtual void makePreview()
    {

    }

    private BackgroundWorker bwAnteprima;
    private WaitForm waitForm;
    private Image image;

    protected void AggiornaAnteprima(Image img)
    {
        image = img;
        bwAnteprima.RunWorkerAsync();
        waitForm = new WaitForm();
        waitForm.Owner = this;
        waitForm.ShowDialog();
    }

    private void bwAnteprima_DoWork(object sender, DoWorkEventArgs e)
    {
        e.Result = engine.GetResult(true);
    }

    private void bwAnteprima_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        MagickImage bm = (MagickImage)e.Result;
        image.Source = bm.ToBitmapSource();
        waitForm.Close();
    }


    public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
    {
        if (depObj == null)
            yield return null;

        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
        {
            var child = VisualTreeHelper.GetChild(depObj, i);

            if (child != null && child is T)
                yield return (T)child;

            foreach (T childOfChild in FindVisualChildren<T>(child))
                yield return childOfChild;
        }
    }

    protected void addAllFiles()
    {
        foreach (var tb in FindVisualChildren<TextBox>(this))
        {
            if (!string.IsNullOrWhiteSpace(tb.Text))
                engine.FilesList.Add(tb.Text);
        }
    }
}
