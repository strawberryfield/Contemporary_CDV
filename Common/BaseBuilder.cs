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
using System.IO;

namespace Casasoft.CCDV
{
    public class BaseBuilder
    {
        protected int spessore;
        protected Formats fmt;
        protected BaseBuilderCommandLine par;
        protected MagickColor fillColor;
        protected MagickColor borderColor;
        protected TargetType targetType;

        protected MagickGeometry topFormat;
        protected MagickGeometry borderFormat;
        protected MagickGeometry frontFormat;

        protected MagickImage topImage;
        protected MagickImage bottomImage;
        protected MagickImage leftImage;
        protected MagickImage rightImage;
        protected MagickImage frontImage;
        protected MagickImage backImage;


        #region constructors
        public BaseBuilder(int Spessore, int dpi) :
            this(Spessore, new Formats(dpi), TargetType.cdv)
        { }
        public BaseBuilder(int Spessore, int dpi, TargetType targetype) :
            this(Spessore, new Formats(dpi), targetype)
        { }

        public BaseBuilder(int Spessore, Formats formats) :
            this(Spessore, formats, MagickColors.White, MagickColors.Black, TargetType.cdv)
        { }
        public BaseBuilder(int Spessore, Formats formats, TargetType targetype) :
            this(Spessore, formats, MagickColors.White, MagickColors.Black, targetype)
        { }

        public BaseBuilder(int Spessore, int dpi, MagickColor fillcolor) :
            this(Spessore, new Formats(dpi), fillcolor, MagickColors.Black, TargetType.cdv)
        { }
        public BaseBuilder(int Spessore, int dpi, MagickColor fillcolor, TargetType targetype) :
           this(Spessore, new Formats(dpi), fillcolor, MagickColors.Black, targetype)
        { }

        public BaseBuilder(int Spessore, int dpi, MagickColor fillcolor, MagickColor bordercolor) :
            this(Spessore, new Formats(dpi), fillcolor, bordercolor, TargetType.cdv)
        { }
        public BaseBuilder(int Spessore, int dpi, MagickColor fillcolor, MagickColor bordercolor, TargetType targetype) :
           this(Spessore, new Formats(dpi), fillcolor, bordercolor, targetype)
        { }

        public BaseBuilder(int Spessore, Formats formats, MagickColor fillcolor, MagickColor bordercolor) :
            this(Spessore, formats, fillcolor, bordercolor, TargetType.cdv)
        { }
        public BaseBuilder(int Spessore, Formats formats, MagickColor fillcolor, MagickColor bordercolor, TargetType targetype)
        {
            fmt = formats;
            spessore = fmt.ToPixels(Spessore);
            fillColor = fillcolor;
            borderColor = bordercolor;
            targetType = targetype;
            makeEmptyImages();
        }

        public BaseBuilder(int Spessore) : this(Spessore, 300) { }
        public BaseBuilder() : this(5) { }

        public BaseBuilder(BaseBuilderCommandLine parameters, Formats formats) :
            this(parameters.thickness, formats, parameters.FillColor, parameters.BorderColor, parameters.targetType)
        {
            par = parameters;

            if (par.useSampleImages) CreateTestImages();

            SetFrontImage(par.frontImage);
            SetBackImage(par.backImage, par.isHorizontal);
            SetTopImage(par.topImage);
            SetBottomImage(par.bottomImage);
            SetLeftImage(par.leftImage);
            SetRightImage(par.rightImage);
        }
        #endregion

        protected virtual void makeEmptyImages()
        {
            switch (targetType)
            {
                case TargetType.cdv:
                    frontFormat = fmt.CDV_Full_v;
                    break;
                case TargetType.cc:
                    frontFormat = fmt.CC_o;
                    break;
                default:
                    frontFormat = fmt.CDV_Full_v;
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

        protected MagickImage checkAndLoad(string filename, MagickImage template) =>
            (!string.IsNullOrWhiteSpace(filename) && File.Exists(filename)) ?
            Utils.RotateResizeAndFill(new(filename), template, fillColor) : template;

        public void SetTopImage(string filename) => topImage = checkAndLoad(filename, topImage);
        public void SetBottomImage(string filename) => bottomImage = checkAndLoad(filename, bottomImage);
        public void SetLeftImage(string filename)
        {
            leftImage = checkAndLoad(filename, leftImage);
            if (!string.IsNullOrWhiteSpace(par.borderText))
            {
                Utils.CenteredText(par.borderText, leftImage.Width / 2, leftImage, par.font, -90).Draw(leftImage);
            }
        }
        public void SetRightImage(string filename) => rightImage = checkAndLoad(filename, rightImage);
        public void SetFrontImage(string filename) => frontImage = checkAndLoad(filename, frontImage);
        public void SetBackImage(string filename, bool isHorizontal = false)
        {
            backImage = checkAndLoad(filename, backImage);
            if (isHorizontal)
                backImage.Rotate(180);
        }

        #region test
        public virtual void CreateTestImages()
        {
            frontImage = new(MagickColors.LightGray, frontFormat.Width, frontFormat.Height);
            Utils.CenteredText("Front", 120, frontFormat, par.font).Draw(frontImage);

            backImage = new(MagickColors.LightBlue, frontFormat.Width, frontFormat.Height);
            Utils.CenteredText("Back", 120, frontFormat, par.font).Draw(backImage);

            topImage = new(MagickColors.LightGreen, topFormat.Width, topFormat.Height);
            Utils.CenteredText("Top", 30, topFormat, par.font).Draw(topImage);

            bottomImage = new(MagickColors.LightCoral, topFormat.Width, topFormat.Height);
            Utils.CenteredText("Bottom", 30, topFormat, par.font).Draw(bottomImage);

            leftImage = new(MagickColors.LightYellow, borderFormat.Height, borderFormat.Width);
            Utils.CenteredText("Left", 30, leftImage, par.font).Draw(leftImage);
            leftImage.Rotate(90);

            rightImage = new(MagickColors.Linen, borderFormat.Height, borderFormat.Width);
            Utils.CenteredText("Right", 30, rightImage, par.font).Draw(rightImage);
            rightImage.Rotate(90);
        }
        #endregion

    }
}
