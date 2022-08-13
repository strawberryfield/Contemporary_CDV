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

public MagickImage ProcessOnLoad(MagickImage image)
{
    // Get the image name
    string filename = Path.GetFileNameWithoutExtension(image.FileName);

    // Resize the image and put aligned to top
    MagickImage img1 = Utils.RotateResizeAndFill(image, engine.fmt.CDV_Internal_v, engine.FillColor);
    img1.Trim();
    image = engine.img.CDV_Full_v();
    image.Composite(img1, Gravity.North, 0, engine.fmt.ToPixels(5));

    // Format and overlay image name
    MagickReadSettings settings = new();
    settings.BackgroundColor = MagickColors.Transparent;
    MagickImage text = new(@$"pango:<span size='50000'>{filename}</span>", settings);
    image.Composite(text, Gravity.South, 0, engine.fmt.ToPixels(5), CompositeOperator.Over);

    return image;
}