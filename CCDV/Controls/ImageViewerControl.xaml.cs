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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Casasoft.CCDV.UI;

/// <summary>
/// Interaction logic for ImageViewerControl.xaml
/// </summary>
public partial class ImageViewerControl : UserControl
{
    public ImageViewerControl()
    {
        InitializeComponent();
    }

    #region properties
    public ImageSource Source
    {
        get => img.Source;
        set => img.Source = value;
    }

    public Image Image => img;
    #endregion

    #region transformations
    private ScaleTransform flipH = new() { ScaleX = -1 };
    private bool isFlipH = false;

    private ScaleTransform flipV = new() { ScaleY = -1 };
    private bool isFlipV = false;

    private int rotation = 0;

    private void ApplyTrans()
    {
        img.RenderTransformOrigin = new Point(0.5, 0.5);
        TransformGroup tg = new();
        if (isFlipH) tg.Children.Add(flipH);
        if (isFlipV) tg.Children.Add(flipV);
        if (rotation != 0) tg.Children.Add(new RotateTransform(rotation));
        img.RenderTransform = tg;
    }
    #endregion

    #region public methods
    public void FlipHorizontal()
    {
        isFlipH = !isFlipH;
        ApplyTrans();
    }

    public void FlipVertical()
    {
        isFlipV = !isFlipV;
        ApplyTrans();
    }

    public void Rotate(int angle)
    {
        rotation += angle;
        if (rotation < 0) rotation += 360;
        if (rotation >= 360) rotation -= 360;
        ApplyTrans();
    }

    public void Reset()
    {
        isFlipH = false;
        isFlipV = false;
        rotation = 0;
        ApplyTrans();
    }
    #endregion

    #region event handlers
    private void MenuItem_FlipH_Click(object sender, System.Windows.RoutedEventArgs e) => FlipHorizontal();
    private void MenuItem_FlipV_Click(object sender, System.Windows.RoutedEventArgs e) => FlipVertical();

    private void MenuItem_R90_Click(object sender, System.Windows.RoutedEventArgs e) => Rotate(90);
    private void MenuItem_R270_Click(object sender, System.Windows.RoutedEventArgs e) => Rotate(-90);
    private void MenuItem_R180_Click(object sender, System.Windows.RoutedEventArgs e) => Rotate(180);

    private void MenuItem_Reset_Click(object sender, System.Windows.RoutedEventArgs e) => Reset();
    #endregion
}
