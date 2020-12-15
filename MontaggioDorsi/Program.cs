// copyright (c) 2020 Roberto Ceccarelli - CasaSoft
// http://strawberryfield.altervista.org 
// 
// This file is part of CasaSoft Contemporary Carte de Visite Tools
// 
// CasaSoft CCDV Tools is free software: 
// you can redistribute it and/or modify it
// under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// CasaSoft CCDV Tools is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
// See the GNU General Public License for more details.
// 
// You should have received a copy of the GNU AGPL v.3
// along with CasaSoft CCDV Tools.  
// If not, see <http://www.gnu.org/licenses/>.

using Casasoft.CCDV;
using ImageMagick;
using Mono.Options;
using System;
using System.Collections.Generic;
using System.IO;

bool shouldShowHelp = false;
string outputName = "dorsi";

string exeName = Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName);
Utils.WelcomeBanner(exeName);

OptionSet options = new OptionSet
{
    { "o|output=", "set output dir/filename", o => outputName = o },
    { "h|help", "show this message and exit", h => shouldShowHelp = h != null },
};

List<string> extra;
try
{
    // parse the command line
    extra = options.Parse(args);
}
catch (OptionException e)
{
    Utils.ShowParametersError(exeName, e);
    return;
}

if (shouldShowHelp)
    Utils.ShowHelp(exeName, "[-o |--output=OutPathName] inputfile", options);


Formats fmt = new(300);
Images img = new(fmt);

MagickImage final = img.InCartha20x27_o();
MagickImageCollection images = new();

MagickImage dorsoOrig;
if (extra.Count > 0)
    dorsoOrig = new(extra[0]);
else
    dorsoOrig = img.CDV_Full_v();

MagickImage dorso = Utils.ResizeAndFill(dorsoOrig, fmt.CDV_Full_v);
dorso.BorderColor = MagickColors.Black;
dorso.Border(1);

for (int i = 0; i < 4; i++) images.Add(dorso.Clone());
final.Composite(images.AppendHorizontally(), Gravity.North, new PointD(0, fmt.ToPixels(10)));

images.Clear();
dorso.Rotate(90);
for (int i = 0; i < 2; i++) images.Add(dorso.Clone());
final.Composite(images.AppendHorizontally(), Gravity.North, new PointD(0, fmt.ToPixels(10) + dorso.Width - 1));

final.Write($"{outputName}.jpg");
