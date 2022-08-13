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

int counter = 0;

public void Init()
{
    counter = Convert.ToInt16(engine.Tag);
}

public MagickImage ProcessOnLoad(MagickImage image)
{
    // Get the image path
    string filedir = Path.GetDirectoryName(image.FileName);

    // Get the text to render
    MagickReadSettings settings = new();
    settings.BackgroundColor = MagickColors.Transparent;
    string pangoback = File.ReadAllText(Path.Combine(filedir, "author.txt"));
    MagickImage text = new($"pango:{pangoback}", settings);

    // Render the big number
    string pangonumber = $"pango:<span size='800000' face='arial' color='lightgray'><b>{counter}</b></span>";
    MagickImage number = new(pangonumber);

    // Compose the images
    image = engine.img.CDV_Full_o();
    image.Composite(number, Gravity.East,  CompositeOperator.Over);
    image.Composite(text, Gravity.Southwest, engine.fmt.ToPixels(5), engine.fmt.ToPixels(5), CompositeOperator.Over);

    counter++;
    return image;
}
