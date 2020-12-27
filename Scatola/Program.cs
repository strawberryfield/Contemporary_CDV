// copyright (c) 2020 Roberto Ceccarelli - Casasoft
// http://strawberryfield.altervista.org 
// 
// This file is part of Casasoft Contemporary Carte de Visite Tools
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
using System.Collections.Generic;
using System.IO;

#region command line
bool shouldShowHelp = false;
string outputName = "box";
string dpi = "300";
string thickness = "5";

string topImage = string.Empty;
string bottomImage = string.Empty;
string frontImage = string.Empty;
string backImage = string.Empty;
string leftImage = string.Empty;
string rightImage = string.Empty;

string exeName = Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName);
Utils.WelcomeBanner(exeName);

OptionSet options = new OptionSet
{
    { "a|aboveimage=", "set the image for the top cover", i => topImage = i },
    { "z|bottomimage=", "set the image for the bottom", i => bottomImage = i },
    { "l|leftimage=", "set the image for the left border", i => leftImage = i },
    { "r|rightimage=", "set the image for the right border", i => rightImage = i },
    { "f|frontimage=", "set the image for the front", i => frontImage = i },
    { "b|backimage=", "set the image for the back", i => backImage = i },
    { "t|thickness=", "set the box thickness (default 5mm)", t => thickness = t },
    { "o|output=", "set output filename (default 'box')", o => outputName = o },
    { "dpi=", "set output resolution (default 300)", res => dpi = res },
    { "h|help", "show this message and exit", h => shouldShowHelp = h != null },
};

List<string> filesList;
try
{
    // parse the command line
    filesList = options.Parse(args);
}
catch (OptionException e)
{
    Utils.ShowParametersError(exeName, e);
    return;
}

if (shouldShowHelp)
    Utils.ShowHelp(exeName, "[option]+", options);

int nthickness = Utils.GetIntParameter(thickness, 5, "Incorrect thickness value '{0}'. Using default value.");
int ndpi = Utils.GetDPI(dpi, 300);
#endregion

Formats fmt = new(ndpi);
Images img = new(fmt);
MagickImage output = img.InCartha20x27_o();
ScatolaBuilder sc = new(nthickness, fmt);

// only for test 
sc.CreateTestImages();

sc.SetFrontImage(frontImage);
sc.SetBackImage(backImage);
sc.SetTopImage(topImage);
sc.SetBottomImage(bottomImage);
sc.SetLeftImage(leftImage);
sc.SetRightImage(rightImage);

output.Composite(sc.Build(), Gravity.Center);
fmt.SetImageParameters(output);
output.Write($"{outputName}.jpg");
