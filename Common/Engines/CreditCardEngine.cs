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

using Casasoft.CCDV.JSON;
using ImageMagick;
using System;
using System.Text.Json;

namespace Casasoft.CCDV.Engines;

/// <summary>
/// Creates a credit card
/// </summary>
public class CreditCardEngine : BaseEngine
{
    #region properties
    /// <summary>
    /// Image to put on the back side
    /// </summary>
    /// <remarks>
    /// If not present it will use the front image blurred and lighted
    /// </remarks>
    public string BackImage { get; set; }

    /// <summary>
    /// Text to print on the front (like cardholder)
    /// </summary>
    public string FrontText { get; set; }

    private string _font = "Arial";
    /// <summary>
    /// Front text font
    /// </summary>
    public string FrontTextFont
    {
        get => _font;
        set => _font = string.IsNullOrWhiteSpace(value) ? "Arial" : value;
    }
    /// <summary>
    /// front text border color
    /// </summary>
    public MagickColor FrontTextBorder { get; set; }
    /// <summary>
    /// Front text fill color
    /// </summary>
    public MagickColor FrontTextColor { get; set; }
    /// <summary>
    /// Front text background fill color
    /// </summary>
    public MagickColor FrontTextBackground { get; set; }
    /// <summary>
    /// use bold weight for front text (if available for font)
    /// </summary>
    public bool fontBold { get; set; } = false;
    /// <summary>
    /// use italic style for front text (if available for font)
    /// </summary>
    public bool fontItalic { get; set; } = false;

    /// <summary>
    /// Pseudo magnetic band color
    /// </summary>
    public MagickColor MagneticBandColor { get; set; }
    /// <summary>
    /// Pseudo magnetic band image
    /// </summary>
    public string MagneticBandImage { get; set; }

    /// <summary>
    /// text to put in the back side
    /// </summary>
    /// <remarks>
    /// The text can be formatted with Pango markup
    /// </remarks>
    public string BackText { get; set; }
    #endregion

    #region constructors
    /// <summary>
    /// Constructor
    /// </summary>
    public CreditCardEngine() : base()
    {
        parameters = new CreditCardParameters();
    }

    /// <summary>
    /// Contructor
    /// </summary>
    /// <param name="par"></param>
    public CreditCardEngine(ICommandLine par) : base(par)
    {
        CreditCardCommandLine p = (CreditCardCommandLine)par;
        BackImage = p.BackImage;
        FrontText = p.FrontText;
        FrontTextFont = p.FrontTextFont;
        FrontTextBorder = p.FrontTextBorder;
        FrontTextColor = p.FrontTextColor;
        FrontTextBackground = p.FrontTextBackground;
        fontBold = p.fontBold;
        fontItalic = p.fontItalic;
        MagneticBandColor = p.MagneticBandColor;
        MagneticBandImage = p.MagneticBandImage;
        BackText = p.BackText;
        parameters = new CreditCardParameters();
    }
    #endregion

    #region json
    /// <summary>
    /// Returns the parameters in json format
    /// </summary>
    /// <returns></returns>
    public override string GetJsonParams()
    {
        GetBaseJsonParams();
        CreditCardParameters p = (CreditCardParameters)parameters;

        p.BackImage = BackImage;
        p.BackText = BackText;
        p.FrontText = FrontText;
        p.FrontTextFont = FrontTextFont;
        p.FrontTextColor = colors.GetColorString(FrontTextColor);
        p.FrontTextBorder = colors.GetColorString(FrontTextBorder);
        p.FrontTextBackground = colors.GetColorString(FrontTextBackground);
        p.fontBold = fontBold;
        p.fontItalic = fontItalic;
        p.MagneticBandImage = MagneticBandImage;
        p.MagneticBandColor = colors.GetColorString(MagneticBandColor);

        return JsonSerializer.Serialize(p);
    }

    /// <summary>
    /// Sets the parameters from json formatted string
    /// </summary>
    /// <param name="json"></param>
    public override void SetJsonParams(string json)
    {
        CreditCardParameters p = JsonSerializer.Deserialize<CreditCardParameters>(json);
        parameters = p;
        SetBaseJsonParams();

        BackImage = p.BackImage;
        BackText = p.BackText;
        FrontText = p.FrontText;
        FrontTextFont = p.FrontTextFont;
        FrontTextColor = colors.GetColor(p.FrontTextColor);
        FrontTextBorder = colors.GetColor(p.FrontTextBorder);
        FrontTextBackground = colors.GetColor(p.FrontTextBackground);
        fontBold = p.fontBold;
        fontItalic = p.fontItalic;
        MagneticBandImage = p.MagneticBandImage;
        MagneticBandColor = colors.GetColor(p.MagneticBandColor);
    }
    #endregion

    #region build
    /// <summary>
    /// Does the dirty work
    /// </summary>
    /// <returns>Image to print</returns>
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
            rear.Level(new Percentage(0), new Percentage(80), 3);
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
            front.Settings.FontWeight = fontBold ? FontWeight.Bold : FontWeight.Normal;
            front.Settings.FontStyle = fontItalic ? FontStyleType.Italic : FontStyleType.Normal;
            front.Settings.FontPointsize = fmt.ToPixels(4);
            front.Settings.TextKerning = 10;
            draw = new();
            if (FrontTextBackground != MagickColors.None)
            {
                // Rectangle behind text
                Drawables drawBackground = new();
                drawBackground.StrokeColor(FrontTextBackground)
                    .FillColor(FrontTextBackground)
                    .StrokeWidth(1)
                    .StrokeAntialias(true);
                var size = front.FontTypeMetrics(FrontText);
                drawBackground.RoundRectangle(fmt.ToPixels(3), front.Height - fmt.ToPixels(8),
                    size.TextWidth + fmt.ToPixels(5), front.Height - fmt.ToPixels(3),
                    fmt.ToPixels(1), fmt.ToPixels(1));
                drawBackground.Draw(front);
            }
            draw.StrokeColor(FrontTextBorder)
                .FillColor(FrontTextColor)
                .StrokeAntialias(true)
                .StrokeWidth(1);
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

            MagickReadSettings settings = new();
            settings.BackgroundColor = MagickColors.Transparent;

            MagickImage backText = new(BackText, settings);
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
    #endregion
}
