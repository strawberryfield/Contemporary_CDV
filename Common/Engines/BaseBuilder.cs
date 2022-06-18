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

using ImageMagick;
using System.IO;

namespace Casasoft.CCDV;

/// <summary>
/// Common builder for boxes and folders
/// </summary>
public class BaseBuilder : IBuilder
{
    public Formats fmt { get; set; }
    protected BaseBuilderCommandLine par;
    public MagickColor fillColor { get; set; }
    public MagickColor borderColor { get; set; }

    protected int spessore;
    /// <summary>
    /// Thickness of the box (mm)
    /// </summary>
    public int Thickness
    {
        get => spessore / fmt.ToPixels(1); 
        set => spessore = fmt.ToPixels(value);
    }
    /// <summary>
    /// target box format
    /// </summary>
    public TargetType targetType { get; set; }
    /// <summary>
    /// Set if box is landscape
    /// </summary>
    public bool isHorizontal { get; set; } = false;

    private string _font = "Arial";
    /// <summary>
    /// Font for border text
    /// </summary>
    public string font
    {
        get => _font;
        set => _font = string.IsNullOrWhiteSpace(value) ? "Arial" : value;
    }
    /// <summary>
    /// Prints text in bold if available
    /// </summary>
    public bool fontBold { get; set; } = false;
    /// <summary>
    /// Prints text in italic if available
    /// </summary>
    public bool fontItalic { get; set; } = false;

    /// <summary>
    /// Text to print on left border
    /// </summary>
    public string borderText { get; set; }

    /// <summary>
    /// Output paper size
    /// </summary>
    public PaperFormats PaperFormat { get; set; }

    protected MagickGeometry topFormat;
    protected MagickGeometry borderFormat;
    protected MagickGeometry frontFormat;

    #region images properties
    /// <summary>
    /// Image for top border
    /// </summary>
    protected MagickImage topImage;
    /// <summary>
    /// Image for bottom border
    /// </summary>
    protected MagickImage bottomImage;
    /// <summary>
    /// Image for left border
    /// </summary>
    protected MagickImage leftImage;
    /// <summary>
    /// Image for right border
    /// </summary>
    protected MagickImage rightImage;
    /// <summary>
    /// image for front cover
    /// </summary>
    protected MagickImage frontImage;
    /// <summary>
    /// image for back cover
    /// </summary>
    protected MagickImage backImage;

    /// <summary>
    /// Image for top border
    /// </summary>
    public string topImagePath { get; set; }
    /// <summary>
    /// Image for bottom border
    /// </summary>
    public string bottomImagePath { get; set; }
    /// <summary>
    /// Image for left border
    /// </summary>
    public string leftImagePath { get; set; }
    /// <summary>
    /// Image for right border
    /// </summary>
    public string rightImagePath { get; set; }
    /// <summary>
    /// Image for front cover
    /// </summary>
    public string frontImagePath { get; set; }
    /// <summary>
    /// Image for back cover
    /// </summary>
    public string backImagePath { get; set; }
    #endregion

    #region constructors
    public BaseBuilder(int Spessore = 5, int dpi = 300, TargetType targetype = TargetType.cdv, bool isHor = false) :
       this(Spessore, new Formats(dpi), targetype, isHor)
    { }

    public BaseBuilder(int Spessore, Formats formats, TargetType targetype = TargetType.cdv, bool isHor = false) :
       this(Spessore, formats, MagickColors.White, MagickColors.Black, targetype, isHor)
    { }

    public BaseBuilder(int Spessore, int dpi, MagickColor fillcolor, TargetType targetype = TargetType.cdv, bool isHor = false) :
       this(Spessore, new Formats(dpi), fillcolor, MagickColors.Black, targetype, isHor)
    { }

    public BaseBuilder(int Spessore,
        int dpi,
        MagickColor fillcolor,
        MagickColor bordercolor,
        TargetType targetype = TargetType.cdv,
        bool isHor = false) :
       this(Spessore, new Formats(dpi), fillcolor, bordercolor, targetype, isHor)
    { }

    public BaseBuilder(int Spessore,
        Formats formats,
        MagickColor fillcolor,
        MagickColor bordercolor,
        TargetType targetype = TargetType.cdv,
        bool isHor = false)
    {
        fmt = formats;
        spessore = fmt.ToPixels(Spessore);
        fillColor = fillcolor;
        borderColor = bordercolor;
        targetType = targetype;
        isHorizontal = isHor;
        makeEmptyImages();
    }

    public BaseBuilder(BaseBuilderCommandLine parameters, Formats formats) :
        this(parameters.thickness, formats, parameters.FillColor, parameters.BorderColor, parameters.targetType, parameters.isHorizontal)
    {
        par = parameters;
        font = parameters.font;
        fontBold = parameters.fontBold;
        fontItalic = parameters.fontItalic;
        borderText = parameters.borderText;
        PaperFormat = parameters.PaperFormat;

        if (par.useSampleImages) CreateTestImages();

        SetFrontImage(par.frontImage);
        SetBackImage(par.backImage, par.isHorizontal);
        SetTopImage(par.topImage);
        SetBottomImage(par.bottomImage);
        SetLeftImage(par.leftImage);
        SetRightImage(par.rightImage);
    }
    #endregion

    public virtual void makeEmptyImages()
    {
        switch (targetType)
        {
            case TargetType.cdv:
                frontFormat = isHorizontal ? fmt.CDV_Full_o : fmt.CDV_Full_v;
                break;
            case TargetType.cc:
                frontFormat = isHorizontal ? fmt.CC_o : fmt.CC_v;
                break;
            default:
                frontFormat = isHorizontal ? fmt.CDV_Full_o : fmt.CDV_Full_v;
                break;
        }
        frontFormat.Width += fmt.ToPixels(5);
        frontFormat.Height += fmt.ToPixels(5);

        borderFormat = new(spessore, frontFormat.Height);
        topFormat = new(frontFormat.Width, spessore);

        topImage = new(fillColor, topFormat.Width, topFormat.Height);
        bottomImage = new(fillColor, topFormat.Width, topFormat.Height);

        leftImage = new(fillColor, borderFormat.Width, borderFormat.Height);
        rightImage = new(fillColor, borderFormat.Width, borderFormat.Height);

        frontImage = new(fillColor, frontFormat.Width, frontFormat.Height);
        backImage = new(fillColor, frontFormat.Width, frontFormat.Height);

    }

    #region set images
    protected MagickImage checkAndLoad(string filename, MagickImage template) =>
        (!string.IsNullOrWhiteSpace(filename) && File.Exists(filename)) ?
        Utils.RotateResizeAndFill(new(filename), template, fillColor) : template;

    /// <summary>
    /// Sets image for the top border
    /// </summary>
    /// <param name="filename"></param>
    public void SetTopImage(string filename)
    {
        topImagePath = filename;
        topImage = checkAndLoad(filename, topImage);
    }

    /// <summary>
    /// Sets image for the bottom border
    /// </summary>
    /// <param name="filename"></param>
    public void SetBottomImage(string filename)
    {
        bottomImagePath = filename;
        bottomImage = checkAndLoad(filename, bottomImage);
    }

    /// <summary>
    /// Sets image for the left border
    /// </summary>
    /// <param name="filename"></param>
    public void SetLeftImage(string filename)
    {
        leftImagePath = filename;
        leftImage = checkAndLoad(filename, leftImage);
        if (!string.IsNullOrWhiteSpace(borderText))
        {
            Utils.CenteredText(borderText, leftImage.Width / 2, leftImage, font, -90, fontBold, fontItalic)
                .Draw(leftImage);
        }
    }

    /// <summary>
    /// Sets image for the right border
    /// </summary>
    /// <param name="filename"></param>
    public void SetRightImage(string filename)
    {
        rightImagePath = filename;
        rightImage = checkAndLoad(filename, rightImage);
    }

    /// <summary>
    /// Sets image for the front cover
    /// </summary>
    /// <param name="filename"></param>
    public void SetFrontImage(string filename)
    {
        frontImagePath = filename;
        frontImage = checkAndLoad(filename, frontImage);
    }

    /// <summary>
    /// Sets image for the back cover
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="isHorizontal"></param>
    public void SetBackImage(string filename, bool isHorizontal = false)
    {
        backImagePath = filename;
        backImage = checkAndLoad(filename, backImage);
        if (isHorizontal)
            backImage.Rotate(180);
    }
    #endregion

    /// <summary>
    /// Creates lines for cut
    /// </summary>
    /// <param name="img"></param>
    public void AddCuttingLines(MagickImage img)
    {
        MagickImage trim = (MagickImage)img.Clone();
        trim.Trim();
        int h_offset = (img.Width - trim.Width) / 2;
        int v_offset = (img.Height - trim.Height) / 2;

        Drawables d = new();
        d.StrokeColor(borderColor).StrokeWidth(1);
        d.Line(0, v_offset, img.Width, v_offset);
        d.Line(0, img.Height - v_offset, img.Width, img.Height - v_offset);
        d.Line(h_offset, 0, h_offset, img.Height - v_offset);
        d.Line(img.Width - h_offset, 0, img.Width - h_offset, img.Height);
        d.Draw(img);
    }

    /// <summary>
    /// Returns empty final image
    /// </summary>
    /// <returns></returns>
    public MagickImage GetOutputImage()
    {
        Images img = new(fmt);
        if (PaperFormat == PaperFormats.Medium)
        {
            return img.InCartha15x20_o();
        }
        else
        {
            return img.InCartha20x27_o();
        }
    }

    #region test
    /// <summary>
    /// Set of images for testing
    /// </summary>
    public virtual void CreateTestImages()
    {
        frontImage = new(MagickColors.LightGray, frontFormat.Width, frontFormat.Height);
        Utils.CenteredText("Front", 120, frontFormat, font, 0, fontBold, fontItalic).Draw(frontImage);

        backImage = new(MagickColors.LightBlue, frontFormat.Width, frontFormat.Height);
        Utils.CenteredText("Back", 120, frontFormat, font, 0, fontBold, fontItalic).Draw(backImage);

        topImage = new(MagickColors.LightGreen, topFormat.Width, topFormat.Height);
        Utils.CenteredText("Top", 30, topFormat, font, 0, fontBold, fontItalic).Draw(topImage);

        bottomImage = new(MagickColors.LightCoral, topFormat.Width, topFormat.Height);
        Utils.CenteredText("Bottom", 30, topFormat, font, 0, fontBold, fontItalic).Draw(bottomImage);

        leftImage = new(MagickColors.LightYellow, borderFormat.Height, borderFormat.Width);
        Utils.CenteredText("Left", 30, leftImage, font, 0, fontBold, fontItalic).Draw(leftImage);
        leftImage.Rotate(90);

        rightImage = new(MagickColors.Linen, borderFormat.Height, borderFormat.Width);
        Utils.CenteredText("Right", 30, rightImage, font, 0, fontBold, fontItalic).Draw(rightImage);
        rightImage.Rotate(90);
    }
    #endregion

}

