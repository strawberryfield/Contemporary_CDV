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
using System.Collections.Generic;

namespace Casasoft.CCDV.Engines;

public class BaseEngine : IEngine
{
    private int _dpi;
    public int Dpi
    {
        get => _dpi;
        set
        {
            _dpi = value;
            fmt = new(_dpi);
        }
    }
    public List<string> FilesList { get; set; }
    public MagickColor FillColor { get; set; }
    public MagickColor BorderColor { get; set; }

    protected Formats fmt;
    protected Images img;

    public BaseEngine()
    {
        Dpi = 300;
        FilesList = new List<string>();
        FillColor = MagickColors.White;
        BorderColor = MagickColors.Black;
    }

    public BaseEngine(ICommandLine par)
    {
        Dpi = par.Dpi;
        FilesList = par.FilesList;
        FillColor = par.FillColor;
        BorderColor = par.BorderColor;
    }

    public MagickImage GetResult() => GetResult(false);
    public virtual MagickImage GetResult(bool quiet) => null;

    public void SetImageParameters(MagickImage image) => fmt.SetImageParameters(image);

    public void SetImageInfo(string o, MagickImage image) => img.Info(WelcomeBannerText(), o).Draw(image);
    public void SetImageInfo(string i, string o, MagickImage image) => img.Info(i, o).Draw(image);
    public virtual string WelcomeBannerText() =>
        "Casasoft Contemporary Carte de Visite GUI\nCopyright (c) 2020-2021 Roberto Ceccarelli - Casasoft\n";
}