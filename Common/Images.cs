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
using System;

namespace Casasoft.CCDV;

public class Images
{
    protected Formats fmt;

    #region constructors
    public Images(int dpi)
    {
        fmt = new(dpi);
    }

    public Images() : this(300) { }

    public Images(Formats f)
    {
        fmt = f;
    }
    #endregion

    #region commercial formats
    public MagickImage InCartha20x27_o(MagickColor c) => new(c, fmt.InCartha20x27_o.Width, fmt.InCartha20x27_o.Height);
    public MagickImage InCartha20x27_v(MagickColor c) => new(c, fmt.InCartha20x27_v.Width, fmt.InCartha20x27_v.Height);
    public MagickImage FineArt10x15_o(MagickColor c) => new(c, fmt.FineArt10x15_o.Width, fmt.FineArt10x15_o.Height);
    public MagickImage FineArt10x15_v(MagickColor c) => new(c, fmt.FineArt10x15_v.Width, fmt.FineArt10x15_v.Height);
    public MagickImage FineArt10x18_o(MagickColor c) => new(c, fmt.FineArt10x18_o.Width, fmt.FineArt10x18_o.Height);
    public MagickImage FineArt10x18_v(MagickColor c) => new(c, fmt.FineArt10x18_v.Width, fmt.FineArt10x18_v.Height);

    public MagickImage InCartha20x27_o() => InCartha20x27_o(MagickColors.White);
    public MagickImage InCartha20x27_v() => InCartha20x27_v(MagickColors.White);
    public MagickImage FineArt10x15_o() => FineArt10x15_o(MagickColors.White);
    public MagickImage FineArt10x15_v() => FineArt10x15_v(MagickColors.White);
    public MagickImage FineArt10x18_o() => FineArt10x18_o(MagickColors.White);
    public MagickImage FineArt10x18_v() => FineArt10x18_v(MagickColors.White);
    #endregion

    #region cdv
    public MagickImage CDV_Full_o(MagickColor c) => new(c, fmt.CDV_Full_o.Width, fmt.CDV_Full_o.Height);
    public MagickImage CDV_Full_v(MagickColor c) => new(c, fmt.CDV_Full_v.Width, fmt.CDV_Full_v.Height);
    public MagickImage CDV_Internal_o(MagickColor c) => new(c, fmt.CDV_Internal_o.Width, fmt.CDV_Internal_o.Height);
    public MagickImage CDV_Internal_v(MagickColor c) => new(c, fmt.CDV_Internal_v.Width, fmt.CDV_Internal_v.Height);

    public MagickImage CDV_Full_o() => CDV_Full_o(MagickColors.White);
    public MagickImage CDV_Full_v() => CDV_Full_v(MagickColors.White);
    public MagickImage CDV_Internal_o() => CDV_Internal_o(MagickColors.White);
    public MagickImage CDV_Internal_v() => CDV_Internal_v(MagickColors.White);
    #endregion

    #region credit card
    public MagickImage CC_o(MagickColor c) => new(c, fmt.CC_o.Width, fmt.CC_o.Height);
    public MagickImage CC_v(MagickColor c) => new(c, fmt.CC_v.Width, fmt.CC_v.Height);

    public MagickImage CC_o() => CC_o(MagickColors.White);
    public MagickImage CC_v() => CC_v(MagickColors.White);

    #endregion

    public Drawables Info(string i, string o)
    {
        Drawables d = new Drawables();

        d.FontPointSize(fmt.ToPixels(3))
            .Font("Arial")
            .FillColor(MagickColors.Black)
            .TextAlignment(TextAlignment.Left);

        d.Text(fmt.ToPixels(10), fmt.ToPixels(185), $"{i}Run {DateTime.Now.ToString("R")}")
            .Text(fmt.InCartha20x27_o.Width / 2, fmt.ToPixels(185), $"DPI: {fmt.DPI}\nOutput: {o}");

        return d;
    }

}
