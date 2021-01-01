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

using ImageMagick;
using System.IO;

namespace Casasoft.CCDV
{
    public class ScatolaBuilder : BaseBuilder
    {

        #region constructors
        public ScatolaBuilder(int Spessore, int dpi) :
            base(Spessore, dpi)
        { }
        public ScatolaBuilder(int Spessore, Formats formats) :
            base(Spessore, formats)
        { }
        public ScatolaBuilder(int Spessore, int dpi, MagickColor fillcolor) :
            base(Spessore, dpi, fillcolor)
        { }
        public ScatolaBuilder(int Spessore, int dpi, MagickColor fillcolor, MagickColor bordercolor) :
            base(Spessore, dpi, fillcolor, bordercolor)
        { }
        public ScatolaBuilder(int Spessore, Formats formats, MagickColor fillcolor, MagickColor bordercolor) :
            base(Spessore, formats, fillcolor, bordercolor)
        { }
        public ScatolaBuilder(int Spessore) :
            base(Spessore)
        { }
        public ScatolaBuilder() :
            base()
        { }
        #endregion

        private MagickGeometry topFormat;
        private MagickGeometry borderFormat;
        private MagickGeometry frontFormat;

        private MagickImage topImage;
        private MagickImage bottomImage;
        private MagickImage leftImage;
        private MagickImage rightImage;
        private MagickImage frontImage;
        private MagickImage backImage;


        protected override void makeEmptyImages()
        {
            frontFormat = fmt.CDV_Full_v;
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

        public void SetTopImage(string filename) => topImage = checkAndLoad(filename, topImage);
        public void SetBottomImage(string filename) => bottomImage = checkAndLoad(filename, bottomImage);
        public void SetLeftImage(string filename) => leftImage = checkAndLoad(filename, leftImage);
        public void SetRightImage(string filename) => rightImage = checkAndLoad(filename, rightImage);
        public void SetFrontImage(string filename) => frontImage = checkAndLoad(filename, frontImage);
        public void SetBackImage(string filename, bool isHorizontal = false)
        {
            backImage = checkAndLoad(filename, backImage);
            if (isHorizontal)
                backImage.Rotate(180);
        }

        public MagickImage Build()
        {
            MagickImage ret = new(MagickColors.White,
                frontFormat.Width * 2 + spessore * 2 + fmt.ToPixels(10) + 2,
                frontFormat.Height + spessore * 2 + fmt.ToPixels(20) + 2);


            // Aggiunge le alette ai bordi della scatola
            MagickImage LeftBorderWithExtra = new(MagickColors.White,
                borderFormat.Width, borderFormat.Height + fmt.ToPixels(20));
            Drawables draw = new();
            draw.StrokeColor(borderColor).StrokeWidth(1);
            draw = cap(draw, spessore, fmt.ToPixels(10));
            draw.Draw(LeftBorderWithExtra);
            LeftBorderWithExtra.Rotate(180);
            draw.Draw(LeftBorderWithExtra);

            MagickImage RightBorderWithExtra = (MagickImage)LeftBorderWithExtra.Clone();
            LeftBorderWithExtra.Composite(leftImage, Gravity.Center);
            RightBorderWithExtra.Composite(rightImage, Gravity.Center);


            // Aggiungiamo coperchi e lembi di chiusura
            MagickImage BackWithTop = new(MagickColors.White,
                frontFormat.Width, frontFormat.Height + spessore + fmt.ToPixels(10));
            draw = new();
            draw.StrokeColor(borderColor).StrokeWidth(1);
            cap(draw, frontFormat.Width, fmt.ToPixels(10)).Draw(BackWithTop);

            MagickImage FrontWithBottom = (MagickImage)BackWithTop.Clone();
            FrontWithBottom.Rotate(180);

            FrontWithBottom.Composite(frontImage, Gravity.North);
            FrontWithBottom.Composite(bottomImage, Gravity.South, new PointD(0, fmt.ToPixels(10)));
            BackWithTop.Composite(backImage, Gravity.South);
            topImage.Rotate(180);
            BackWithTop.Composite(topImage, Gravity.North, new PointD(0, fmt.ToPixels(10)));
            MagickImage frontTopImage = (MagickImage)frontImage.Clone();
            frontTopImage.Crop(frontFormat.Width - 2, fmt.ToPixels(10), Gravity.North);
            frontTopImage.Rotate(180);
            BackWithTop.Composite(frontTopImage, Gravity.North, new PointD(0, 1));

            // Assembliamo le immagini
            ret.Composite(BackWithTop, Gravity.Northwest, new PointD(1, 0));
            ret.Composite(LeftBorderWithExtra, Gravity.West, new PointD(1 + frontFormat.Width, 0));
            ret.Composite(FrontWithBottom, Gravity.Southwest, new PointD(1 + frontFormat.Width + borderFormat.Width, 0));
            ret.Composite(RightBorderWithExtra, Gravity.West, new PointD(1 + frontFormat.Width * 2 + borderFormat.Width, 0));

            // Margini di taglio
            draw = new();
            draw.StrokeColor(borderColor).StrokeWidth(1);
            int bordertop = spessore + fmt.ToPixels(10);
            int borderbottom = bordertop + +frontFormat.Height;
            int marginright = ret.Width - 1;
            draw.Line(0, bordertop - spessore, 0, borderbottom);
            draw.Line(marginright, bordertop, marginright, borderbottom);
            draw.Line(marginright - fmt.ToPixels(10), bordertop, marginright, bordertop);
            draw.Line(marginright - fmt.ToPixels(10), borderbottom, marginright, borderbottom);
            draw.Line(0, borderbottom, frontFormat.Width, borderbottom);
            draw.Line(marginright - fmt.ToPixels(10) - spessore, bordertop,
                marginright - fmt.ToPixels(10) - spessore - frontFormat.Width, bordertop);
            draw.Draw(ret);

            return ret;
        }

        private Drawables cap(Drawables d, int w, int h) =>
            (Drawables)d.Line(0, h - 1, 0, 0).Line(w - 1, h - 1, w - 1, 0).Line(0, 0, w - 1, 0);

        #region test
        public void CreateTestImages()
        {
            frontImage = new(MagickColors.LightGray, frontFormat.Width, frontFormat.Height);
            Utils.CenteredText("Bottom", 120, frontFormat)
                .FontPointSize(50).Text(frontImage.Width / 2, fmt.ToPixels(6), "Front top")
                .Draw(frontImage);

            backImage = new(MagickColors.LightBlue, frontFormat.Width, frontFormat.Height);
            Utils.CenteredText("Back", 120, frontFormat).Draw(backImage);

            topImage = new(MagickColors.LightGreen, topFormat.Width, topFormat.Height);
            Utils.CenteredText("Top", 30, topFormat).Draw(topImage);

            bottomImage = new(MagickColors.LightCoral, topFormat.Width, topFormat.Height);
            Utils.CenteredText("Bottom", 30, topFormat).Draw(bottomImage);

            leftImage = new(MagickColors.LightYellow, borderFormat.Height, borderFormat.Width);
            Utils.CenteredText("Left", 30, leftImage).Draw(leftImage);
            leftImage.Rotate(90);

            rightImage = new(MagickColors.Linen, borderFormat.Height, borderFormat.Width);
            Utils.CenteredText("Right", 30, rightImage).Draw(rightImage);
            rightImage.Rotate(90);
        }
        #endregion
    }
}
