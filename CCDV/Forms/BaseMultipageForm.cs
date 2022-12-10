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
using ImageMagick;
using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Printing;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Casasoft.CCDV.UI;

public partial class BaseMultipageForm : BaseForm
{
    protected List<MagickImage>? bm;
    protected int CurrentPreview = 0;

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

    #region events handlers
    protected void MultipagePreviewBarControl_GoStart(object sender, System.EventArgs e)
    {
        if (bm is not null)
        {
            CurrentPreview = 0;
            image.Source = bm[CurrentPreview].ToBitmapSource();
        }
    }

    protected void MultipagePreviewBarControl_GoBack(object sender, System.EventArgs e)
    {
        if (bm is not null && CurrentPreview > 0)
        {
            CurrentPreview--;
            image.Source = bm[CurrentPreview].ToBitmapSource();
        }
    }

    protected void MultipagePreviewBarControl_GoNext(object sender, System.EventArgs e)
    {
        if (bm is not null && CurrentPreview < bm.Count - 1)
        {
            CurrentPreview++;
            image.Source = bm[CurrentPreview].ToBitmapSource();
        }
    }

    protected void MultipagePreviewBarControl_GoEnd(object sender, System.EventArgs e)
    {
        if (bm is not null)
        {
            CurrentPreview = bm.Count - 1;
            image.Source = bm[CurrentPreview].ToBitmapSource();
        }
    }
    #endregion

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
            CurrentPreview = 0;
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
                pd.PrintTicket.PageMediaSize = new(PageMediaSizeName.ISOA4);
                pd.PrintTicket.PageOrientation = bm[0].Width > bm[0].Height ? PageOrientation.Landscape : PageOrientation.Portrait;
                pd.PrintTicket.PageBorderless = PageBorderless.Borderless;
                FixedDocument doc = NewFixedDocument(pd);

                foreach (MagickImage im in bm)
                {
                    FixedPage page = NewFixedPage(doc);
                    AddImage(page, im);
                    AddPage(doc, page);
                }
                pd.PrintDocument(doc.DocumentPaginator, "Print Images");
            }
        }
    }
    #endregion
}
