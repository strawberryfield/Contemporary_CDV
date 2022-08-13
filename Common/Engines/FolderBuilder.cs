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

namespace Casasoft.CCDV
{
    /// <summary>
    /// Builds a folder
    /// </summary>
    public class FolderBuilder : BaseBuilder
    {

        #region constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Spessore"></param>
        /// <param name="dpi"></param>
        /// <param name="targetype"></param>
        /// <param name="isHor"></param>
        public FolderBuilder(int Spessore = 5, int dpi = 300, TargetType targetype = TargetType.cdv, bool isHor = false) :
           base(Spessore, new Formats(dpi), targetype, isHor)
        { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Spessore"></param>
        /// <param name="formats"></param>
        /// <param name="targetype"></param>
        /// <param name="isHor"></param>
        public FolderBuilder(int Spessore, Formats formats, TargetType targetype = TargetType.cdv, bool isHor = false) :
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
        public FolderBuilder(int Spessore, int dpi, MagickColor fillcolor, TargetType targetype = TargetType.cdv, bool isHor = false) :
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
        public FolderBuilder(int Spessore,
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
        public FolderBuilder(int Spessore,
            Formats formats,
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
        public FolderBuilder(BaseBuilderCommandLine par, Formats formats) :
           base(par, formats)
        { }
        #endregion

        /// <summary>
        /// Does the dirty work
        /// </summary>
        /// <returns></returns>
        public MagickImage Build()
        {
            int fold = fmt.ToPixels(10);
            MagickImage ret = new(MagickColors.White,
                frontFormat.Width * 2 + spessore * 3 + fold + 2 + fmt.ToPixels(20),
                frontFormat.Height + spessore * 2 + fold * 2 + 2);

            // Aggiungiamo coperchi e lembi di chiusura
            MagickImage BackWithTop = new(MagickColors.White,
                frontFormat.Width, frontFormat.Height + (spessore + fold) * 2);
            Drawables draw = new();
            draw.StrokeColor(borderColor).StrokeWidth(1);
            cap(draw, frontFormat.Width, fold).Draw(BackWithTop);
            BackWithTop.Rotate(180);
            cap(draw, frontFormat.Width, fold).Draw(BackWithTop);

            BackWithTop.Composite(backImage, Gravity.Center);
            BackWithTop.Composite(topImage, Gravity.North, 0, fold);
            BackWithTop.Composite(bottomImage, Gravity.South, 0, fold);

            // Clip di chiusura
            MagickImage BackClip = (MagickImage)backImage.Clone();
            BackClip.Crop(fmt.ToPixels(20), fmt.ToPixels(20), Gravity.West);
            MagickImage RightBorderClip = (MagickImage)rightImage.Clone();
            RightBorderClip.Crop(borderFormat.Width, fmt.ToPixels(20), Gravity.Center);
            MagickImageCollection images = new();
            images.Add(RightBorderClip);
            images.Add(BackClip);
            MagickImage Clip = (MagickImage)images.AppendHorizontally();

            // Assembliamo le immagini
            PointD start = new(1, 0);
            start = new(start.X + fold, start.Y);
            ret.Composite(rightImage, Gravity.West, (int)start.X, (int)start.Y);
            start = new(start.X + borderFormat.Width, start.Y);
            ret.Composite(BackWithTop, Gravity.West, (int)start.X, (int)start.Y);
            start = new(start.X + frontFormat.Width, start.Y);
            ret.Composite(leftImage, Gravity.West, (int)start.X, (int)start.Y);
            start = new(start.X + borderFormat.Width, start.Y);
            ret.Composite(frontImage, Gravity.West, (int)start.X, (int)start.Y);
            start = new(start.X + frontFormat.Width, start.Y);
            ret.Composite(Clip, Gravity.West, (int)start.X, (int)start.Y);

            // Margini di taglio
            draw = new();
            draw.StrokeColor(borderColor).StrokeWidth(1);
            int bordertop = spessore + fold;
            int borderbottom = bordertop + +frontFormat.Height;
            int marginright = ret.Width - 1;
            draw.Line(0, bordertop + fold, 0, borderbottom - fold);
            draw.Line(0, bordertop + fold, fold, bordertop);
            draw.Line(0, borderbottom - fold, fold, borderbottom);

            int rpos = fold + spessore;
            draw.Line(fold, bordertop, rpos, bordertop);
            draw.Line(rpos, bordertop, rpos, bordertop - spessore);
            draw.Line(fold, borderbottom, rpos, borderbottom);
            draw.Line(rpos, borderbottom, rpos, borderbottom + spessore);

            rpos += frontFormat.Width;
            draw.Line(rpos, bordertop, rpos, bordertop - spessore);
            draw.Line(rpos, borderbottom, rpos, borderbottom + spessore);

            int lpos = rpos;
            rpos += borderFormat.Width + frontFormat.Width;
            draw.Line(lpos, bordertop, rpos, bordertop);
            draw.Line(lpos, borderbottom, rpos, borderbottom);

            int cliptop = bordertop + frontFormat.Height / 2 - fmt.ToPixels(10);
            int clipbottom = borderbottom - frontFormat.Height / 2 + fmt.ToPixels(10);
            draw.Line(rpos, bordertop, rpos, cliptop);
            draw.Line(rpos, borderbottom, rpos, clipbottom);
            draw.Line(rpos, cliptop, marginright, cliptop);
            draw.Line(rpos, clipbottom, marginright, clipbottom);
            draw.Line(marginright, cliptop, marginright, clipbottom);

            draw.Draw(ret);

            return ret;
        }

        private static Drawables cap(Drawables d, int w, int h) =>
            (Drawables)d.Line(0, h - 1, h, 0).Line(w - 1, h - 1, w - h, 0).Line(h, 0, w - h, 0);

        #region test
        /// <summary>
        /// Set of images for testing
        /// </summary>
        public override void CreateTestImages()
        {
            base.CreateTestImages();

            Drawables draw = new();
            draw.StrokeColor(MagickColors.Black).FontPointSize(50)
                .Text(frontImage.Width / 2, fmt.ToPixels(6), "Front top")
                .Draw(frontImage);
        }
        #endregion
    }
}
