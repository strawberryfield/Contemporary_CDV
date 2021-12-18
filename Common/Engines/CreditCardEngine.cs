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

using ImageMagick;
using System;

namespace Casasoft.CCDV.Engines;

public class CreditCardEngine : BaseEngine
{
    public string BackImage { get; set; }  
    public string FrontText { get; set; }
    public string FrontTextFont { get; set; }
    public MagickColor FrontTextBorder { get; set; }
    public MagickColor FrontTextColor { get; set; }
    public MagickColor MagneticBandColor { get; set; }
    public string MagneticBandImage { get; set; }
    public string BackText { get; set; }

    public CreditCardEngine() : base()  
    {
    }

    public CreditCardEngine(ICommandLine par) : base(par)
    {
        CreditCardCommandLine p = (CreditCardCommandLine)par;
        BackImage = p.BackImage;
        FrontText = p.FrontText;
        FrontTextFont = p.FrontTextFont;
        FrontTextBorder = p.FrontTextBorder;
        FrontTextColor = p.FrontTextColor;
        MagneticBandColor = p.MagneticBandColor;
        MagneticBandImage = p.MagneticBandImage;
        BackText = p.BackText;
    }

    public override MagickImage GetResult(bool quiet)
    {
        img = new(fmt);

        MagickImage final = img.FineArt10x15_v();
        MagickImage front = Get(FilesList[0], quiet);
        int bordertop = final.Height / 2 - front.Height;
        int borderleft = (final.Width - front.Width) / 2;

        // Create rear image
        MagickImage rear;
        if (string.IsNullOrWhiteSpace(BackImage))
        {
            rear = (MagickImage)front.Clone();
            rear.Blur(0, 10);
            rear.Level(new Percentage(0), new Percentage(100), 3);
        }
        else
        {
            rear = Get(BackImage, quiet);
            rear.Flop();
        }

        // Write text in front
        Drawables draw;
        if (!string.IsNullOrWhiteSpace(FrontText))
        {
            front.Settings.FontFamily = FrontTextFont;
            front.Settings.FontWeight = FontWeight.Bold;
            draw = new();
            draw.StrokeColor(FrontTextBorder)
                .FillColor(FrontTextColor)
                .StrokeAntialias(true)
                .StrokeWidth(1)
                .FontPointSize(fmt.ToPixels(4))
                .TextKerning(10);
            draw.Text(fmt.ToPixels(4), front.Height - fmt.ToPixels(4), FrontText);
            draw.Draw(front);
        }

        // Magnetic band
        draw = new();
        draw.FillColor(MagneticBandColor);
        draw.Rectangle(0, fmt.ToPixels(4), rear.Width, fmt.ToPixels(12 + 4));
        draw.Draw(rear);

        // Magnetic band overlay
        if (!string.IsNullOrWhiteSpace(MagneticBandImage))
        {
            MagickImage overlay = Utils.RotateResizeAndFill(
                new MagickImage(MagneticBandImage),
                new MagickGeometry(rear.Width, fmt.ToPixels(12)),
                MagickColors.Transparent);
            overlay.Flop();
            rear.Composite(overlay, Gravity.North, new PointD(0, fmt.ToPixels(4)), CompositeOperator.Over);
        }

        // Back text
        if (!string.IsNullOrWhiteSpace(BackText))
        {
            if (!quiet) Console.WriteLine("Rendering text");
            MagickImage backText = new(BackText);
            backText.ColorFuzz = new Percentage(20);
            backText.Transparent(MagickColors.White);
            backText.Flop();
            rear.Composite(backText, Gravity.South, new PointD(0, fmt.ToPixels(4)), CompositeOperator.Over);
        }

        // Final assembly
        MagickImageCollection images = new();
        images.Add(rear);
        rear.Flip();
        images.Add(front);

        final.Composite(images.AppendVertically(), Gravity.Center);
        CutLines(final, bordertop, borderleft);

        draw = new();
        draw.FontPointSize(fmt.ToPixels(2))
            .Font("Arial")
            .FillColor(MagickColors.Black)
            .TextAlignment(TextAlignment.Left)
            .Gravity(Gravity.Northwest)
            .Text(borderleft + fmt.ToPixels(5), final.Height - bordertop + fmt.ToPixels(5), @$"{WelcomeBannerText()}
Source: {FilesList[0]}
Run {DateTime.Now.ToString("R")}")
            .Draw(final);

        if (!string.IsNullOrWhiteSpace(FrontText))
        {
            draw = new();
            draw.FillColor(MagickColors.Black)
                .Font(FrontTextFont)
                .FontPointSize(fmt.ToPixels(4))
                .TextKerning(10)
                .Gravity(Gravity.Northwest)
                .Text(borderleft + fmt.ToPixels(4), bordertop - fmt.ToPixels(10), FrontText)
                .Draw(final);
        }
        fmt.SetImageParameters(final);

        return final;
    }

    // ----------------------------------------------------------------
    private MagickImage Get(string filename, bool quiet)
    {
        if (!quiet) Console.WriteLine($"Processing: {filename}");
        MagickImage img1 = Utils.RotateResizeAndFill(new(filename),
            fmt.CC_o,
            FillColor);

        return img1;
    }

    private void CutLines(MagickImage final, int bordertop, int borderleft)
    {
        // Margini di taglio
        Drawables draw = new();
        draw.StrokeColor(BorderColor).StrokeWidth(1);
        draw.Line(0, bordertop, final.Width, bordertop);
        draw.Line(0, final.Height - bordertop, final.Width, final.Height - bordertop);
        draw.Line(borderleft, 0, borderleft, final.Height);
        draw.Line(final.Width - borderleft, 0, final.Width - borderleft, final.Height);
        draw.Draw(final);
    }

}
