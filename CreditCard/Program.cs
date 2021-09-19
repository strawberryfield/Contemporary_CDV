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

using Casasoft.CCDV;
using ImageMagick;
using Mono.Options;
using System;

#region command line
CreditCardCommandLine par = new("cc");
par.Usage = "[options]* inputfile";
if (par.Parse(args)) return;

if (par.FilesList.Count <= 0) return;
#endregion

Formats fmt = new(par.Dpi);
Images img = new(fmt);

MagickImage final = img.FineArt10x15_v();
MagickImage front = Get(par.FilesList[0]);
int bordertop = final.Height / 2 - front.Height;
int borderleft = (final.Width - front.Width) / 2;

// Create rear image
MagickImage rear;
if (string.IsNullOrWhiteSpace(par.BackImage))
{
    rear = (MagickImage)front.Clone();
    rear.Blur(0, 10);
    rear.Level(new Percentage(0), new Percentage(100), 3);
}
else
{
    rear = Get(par.BackImage);
    rear.Flop();
}

// Write text in front
Drawables draw;
if (!string.IsNullOrWhiteSpace(par.FrontText))
{
    front.Settings.FontFamily = par.FrontTextFont;
    front.Settings.FontWeight = FontWeight.Bold;
    draw = new();
    draw.StrokeColor(par.FrontTextBorder)
        .FillColor(par.FrontTextColor)
        .StrokeAntialias(true)
        .StrokeWidth(1)
        .FontPointSize(fmt.ToPixels(4))
        .TextKerning(10);
    draw.Text(fmt.ToPixels(4), front.Height - fmt.ToPixels(4), par.FrontText);
    draw.Draw(front);
}

// Magnetic band
draw = new();
draw.FillColor(par.MagneticBandColor);
draw.Rectangle(0, fmt.ToPixels(4), rear.Width, fmt.ToPixels(12 + 4));
draw.Draw(rear);

// Magnetic band overlay
if (!string.IsNullOrWhiteSpace(par.MagneticBandImage))
{
    MagickImage overlay = Utils.RotateResizeAndFill(
        new MagickImage(par.MagneticBandImage),
        new MagickGeometry(rear.Width, fmt.ToPixels(12)),
        MagickColors.Transparent);
    overlay.Flop();
    rear.Composite(overlay, Gravity.North, new PointD(0, fmt.ToPixels(4)), CompositeOperator.Over);
}

// Back text
if (!string.IsNullOrWhiteSpace(par.BackText))
{
    Console.WriteLine("Rendering text");
    MagickImage backText = new(par.BackText);
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
CutLines();

draw = new();
draw.FontPointSize(fmt.ToPixels(2))
    .Font("Arial")
    .FillColor(MagickColors.Black)
    .TextAlignment(TextAlignment.Left)
    .Gravity(Gravity.Northwest)
    .Text(borderleft+fmt.ToPixels(5), final.Height -bordertop + fmt.ToPixels(5), @$"{par.WelcomeBannerText()}
Source: {par.FilesList[0]}
Run {DateTime.Now.ToString("R")}")
    .Draw(final);

draw = new();
draw.FillColor(MagickColors.Black)
    .Font(par.FrontTextFont)
    .FontPointSize(fmt.ToPixels(4))
    .TextKerning(10)
    .Gravity(Gravity.Northwest)
    .Text(borderleft + fmt.ToPixels(4), bordertop - fmt.ToPixels(10), par.FrontText)
    .Draw(final);
fmt.SetImageParameters(final);

final.Write($"{par.OutputName}.jpg");

// ----------------------------------------------------------------
MagickImage Get(string filename)
{
    Console.WriteLine($"Processing: {filename}");
    MagickImage img1 = Utils.RotateResizeAndFill(new(filename),
        fmt.CC_o,
        par.FillColor);

    return img1;
}

void CutLines()
{
    // Margini di taglio
    Drawables draw = new();
    draw.StrokeColor(par.BorderColor).StrokeWidth(1);
    draw.Line(0, bordertop, final.Width, bordertop);
    draw.Line(0, final.Height - bordertop, final.Width, final.Height - bordertop);
    draw.Line(borderleft, 0, borderleft, final.Height);
    draw.Line(final.Width - borderleft, 0, final.Width - borderleft, final.Height);
    draw.Draw(final);
}

#region custom command line parameters
internal class CreditCardCommandLine : CommandLine
{
    public string FrontText { get; set; }
    public string FrontTextFont { get; set; }
    public MagickColor FrontTextColor { get; set; }
    public MagickColor FrontTextBorder { get; set; }
    public MagickColor MagneticBandColor { get; set; }
    public string MagneticBandImage { get; set; }
    public string BackImage { get; set; }
    public string BackText { get; set; }

    private string sFrontTextColor = MagickColors.Black.ToHexString();
    private string sFrontTextBorder = MagickColors.Black.ToHexString();
    private string sMagneticBandColor = MagickColors.SaddleBrown.ToHexString();

    public CreditCardCommandLine(string outputname) :
    this(ExeName(), outputname)
    { }

    public CreditCardCommandLine(string exename, string outputname) :
        base(exename, outputname)
    {
        FrontText = string.Empty;
        FrontTextFont = "Arial";
        FrontTextColor = GetColor(sFrontTextColor);
        FrontTextBorder = GetColor(sFrontTextBorder);
        MagneticBandColor = GetColor(sMagneticBandColor);
        MagneticBandImage = string.Empty;
        BackImage = string.Empty;
        BackText = string.Empty;

        Options = new OptionSet
            {
                { "fronttext=", "text in front (Cardholder name)", o => FrontText = o  },
                { "fronttextfont=", $"front text font (default '{FrontTextFont}')", o => FrontTextFont = o },
                { "fronttextcolor=", $"front text color (default {sFrontTextColor})", o => sFrontTextColor = o },
                { "fronttextborder=", $"front text border color (default {sFrontTextBorder})", o => sFrontTextBorder = o },
                { "mbcolor=", $"magnetic band color (default {sMagneticBandColor})", o => sMagneticBandColor = o },
                { "mbimage=", $"magnetic band overlay image", o => MagneticBandImage = o },
                { "backimage=", "image for back side", o => BackImage = o },
                { "backtext=", "pango markup for text on back side." +
                    "\nText can be stored in a file instead of a string." +
                    "The file must be referenced as '@filename'",
                    o => BackText = GetFileParameter(o) },

            };
        AddBaseOptions();
    }

    public override bool Parse(string[] args)
    {
        if (base.Parse(args)) return true;

        FrontTextColor = GetColor(sFrontTextColor);
        FrontTextBorder = GetColor(sFrontTextBorder);
        MagneticBandColor = GetColor(sMagneticBandColor);
        return false;
    }
}
#endregion
