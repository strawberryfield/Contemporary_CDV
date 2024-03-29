﻿// copyright (c) 2020-2023 Roberto Ceccarelli - Casasoft
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

namespace Casasoft.CCDV;

/// <summary>
/// Builds a box
/// </summary>
public class ScatolaBuilder : BaseBuilder
{

    #region constructors
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="Spessore"></param>
    /// <param name="dpi"></param>
    /// <param name="targetype"></param>
    /// <param name="isHor"></param>
    public ScatolaBuilder(int Spessore = 5, int dpi = 300, TargetType targetype = TargetType.cdv, bool isHor = false) :
       base(Spessore, new Formats(dpi), targetype, isHor)
    { }
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="Spessore"></param>
    /// <param name="formats"></param>
    /// <param name="targetype"></param>
    /// <param name="isHor"></param>
    public ScatolaBuilder(int Spessore, IFormats formats, TargetType targetype = TargetType.cdv, bool isHor = false) :
       base(Spessore, formats, MagickColors.White, MagickColors.Black, targetype, isHor)
    { }
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="Spessore"></param>
    /// <param name="dpi"></param>
    /// <param name="fillcolor"></param>
    /// <param name="targetype"></param>
    /// <param name="isHor"></param>
    public ScatolaBuilder(int Spessore, int dpi, MagickColor fillcolor, TargetType targetype = TargetType.cdv, bool isHor = false) :
      base(Spessore, new Formats(dpi), fillcolor, MagickColors.Black, targetype, isHor)
    { }
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="Spessore"></param>
    /// <param name="dpi"></param>
    /// <param name="fillcolor"></param>
    /// <param name="bordercolor"></param>
    /// <param name="targetype"></param>
    /// <param name="isHor"></param>
    public ScatolaBuilder(int Spessore,
        int dpi,
        MagickColor fillcolor,
        MagickColor bordercolor,
        TargetType targetype = TargetType.cdv,
        bool isHor = false) :
       base(Spessore, new Formats(dpi), fillcolor, bordercolor, targetype, isHor)
    { }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="Spessore"></param>
    /// <param name="formats"></param>
    /// <param name="fillcolor"></param>
    /// <param name="bordercolor"></param>
    /// <param name="targetype"></param>
    /// <param name="isHor"></param>
    public ScatolaBuilder(int Spessore,
        IFormats formats,
        MagickColor fillcolor,
        MagickColor bordercolor,
        TargetType targetype = TargetType.cdv,
        bool isHor = false) :
       base(Spessore, formats, fillcolor, bordercolor, targetype, isHor)
    { }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="par"></param>
    /// <param name="formats"></param>
    public ScatolaBuilder(BaseBuilderCommandLine par, IFormats formats) :
        base(par, formats)
    { }
    #endregion


    /// <summary>
    /// Does the dirty work
    /// </summary>
    /// <returns></returns>
    public MagickImage Build()
    {
        int fold = fmt.ToPixels(12);
        MagickImage ret = new(MagickColors.White,
            frontFormat.Width * 2 + spessore * 2 + fold + 2,
            frontFormat.Height + spessore * 2 + fold * 2 + 2);


        // Aggiunge le alette ai bordi della scatola
        MagickImage LeftBorderWithExtra = new(MagickColors.White,
            borderFormat.Width, borderFormat.Height + fold * 2);
        Drawables draw = new();
        draw.StrokeColor(borderColor).StrokeWidth(1);
        draw = cap(draw, spessore, fold);
        draw.Draw(LeftBorderWithExtra);
        LeftBorderWithExtra.Rotate(180);
        draw.Draw(LeftBorderWithExtra);

        MagickImage RightBorderWithExtra = (MagickImage)LeftBorderWithExtra.Clone();
        LeftBorderWithExtra.Composite(leftImage, Gravity.Center);
        RightBorderWithExtra.Composite(rightImage, Gravity.Center);


        // Aggiungiamo coperchi e lembi di chiusura
        MagickImage BackWithTop = new(MagickColors.White,
            frontFormat.Width, frontFormat.Height + spessore + fold);
        draw = new();
        draw.StrokeColor(borderColor).StrokeWidth(1);
        cap(draw, frontFormat.Width, fold).Draw(BackWithTop);

        MagickImage FrontWithBottom = (MagickImage)BackWithTop.Clone();
        FrontWithBottom.Rotate(180);

        FrontWithBottom.Composite(frontImage, Gravity.North);
        FrontWithBottom.Composite(bottomImage, Gravity.South, 0, fold);
        BackWithTop.Composite(backImage, Gravity.South);
        topImage.Rotate(180);
        BackWithTop.Composite(topImage, Gravity.North, 0, fold);
        MagickImage frontTopImage = (MagickImage)frontImage.Clone();
        frontTopImage.Crop(frontFormat.Width - 2, fold, Gravity.North);
        frontTopImage.Rotate(180);
        BackWithTop.Composite(frontTopImage, Gravity.North, 0, 1);

        // Assembliamo le immagini
        ret.Composite(BackWithTop, Gravity.Northwest, 1, 0);
        ret.Composite(LeftBorderWithExtra, Gravity.West, 1 + frontFormat.Width, 0);
        ret.Composite(FrontWithBottom, Gravity.Southwest, 1 + frontFormat.Width + borderFormat.Width, 0);
        ret.Composite(RightBorderWithExtra, Gravity.West, 1 + frontFormat.Width * 2 + borderFormat.Width, 0);

        // Margini di taglio
        draw = new();
        draw.StrokeColor(borderColor).StrokeWidth(1);
        int bordertop = spessore + fold;
        int borderbottom = bordertop + +frontFormat.Height;
        int marginright = ret.Width - 1;
        draw.Line(0, bordertop - spessore, 0, borderbottom);
        draw.Line(marginright, bordertop, marginright, borderbottom);
        draw.Line(marginright - fold, bordertop, marginright, bordertop);
        draw.Line(marginright - fold, borderbottom, marginright, borderbottom);
        draw.Line(0, borderbottom, frontFormat.Width, borderbottom);
        draw.Line(marginright - fold - spessore, bordertop,
            marginright - fold - spessore - frontFormat.Width, bordertop);
        draw.Draw(ret);

        return ret;
    }

    private Drawables cap(Drawables d, int w, int h) =>
        (Drawables)d.Line(0, h - 1, 0, 0).Line(w - 1, h - 1, w - 1, 0).Line(0, 0, w - 1, 0);

    #region test
    /// <summary>
    /// Set of images for testing
    /// </summary>
    public override void CreateTestImages()
    {
        base.CreateTestImages();

        Drawables draw = new();
        if (!string.IsNullOrWhiteSpace(font))
        {
            draw.Font(font,
             fontItalic ? FontStyleType.Italic : FontStyleType.Normal,
             fontBold ? FontWeight.Bold : FontWeight.Normal,
             FontStretch.Normal);
        }
        draw.StrokeColor(MagickColors.Black).FontPointSize(50)
            .Text(frontImage.Width / 2, fmt.ToPixels(6), "Front top")
            .Draw(frontImage);
    }
    #endregion
}
