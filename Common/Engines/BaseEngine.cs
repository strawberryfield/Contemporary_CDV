﻿// copyright (c) 2020-2022 Roberto Ceccarelli - Casasoft
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

using Casasoft.CCDV.JSON;
using Casasoft.CCDV.Scripting;
using ImageMagick;
using System.Collections.Generic;
using System.Reflection;

namespace Casasoft.CCDV.Engines;

/// <summary>
/// Base class for various images managers
/// </summary>
public class BaseEngine : IEngine
{
    #region properties
    private int _dpi;
    /// <summary>
    /// Output resolution
    /// </summary>
    public int Dpi
    {
        get => _dpi;
        set
        {
            _dpi = value;
            fmt = new(_dpi);
        }
    }
    /// <summary>
    /// List of files to process
    /// </summary>
    public List<string> FilesList { get; set; }
    /// <summary>
    /// Color to fill empty spaces
    /// </summary>
    public MagickColor FillColor { get; set; }
    /// <summary>
    /// Color for lines and borders
    /// </summary>
    public MagickColor BorderColor { get; set; }

    /// <summary>
    /// Instance of formats handler
    /// </summary>
    public Formats fmt;
    /// <summary>
    /// Instance of images handler
    /// </summary>
    public Images img;
    /// <summary>
    /// Class for json parameters handling
    /// </summary>
    protected IParameters parameters;
    /// <summary>
    /// Colors conversion utilities
    /// </summary>
    protected Colors colors;

    private string _script;
    /// <summary>
    /// c# script for custom processing
    /// </summary>
    public string Script
    {
        get => _script;
        set
        {
            _script = value;
            if (!string.IsNullOrWhiteSpace(value))
            {
                CustomCode = ScriptingClass.Compile(value);
            }
        }
    }
    /// <summary>
    /// compiled script for custom processing
    /// </summary>
    public Assembly CustomCode { get; set; }
    /// <summary>
    /// Class that handles user scripts
    /// </summary>
    public IScripting ScriptingClass { get; set; }
    /// <summary>
    /// Storage for the instantiated Script object
    /// </summary>
    protected object ScriptInstance { get; set; }   

    #endregion

    #region constructors
    /// <summary>
    /// Constructor
    /// </summary>
    public BaseEngine()
    {
        colors = new();
        Dpi = 300;
        FilesList = new List<string>();
        FillColor = MagickColors.White;
        BorderColor = MagickColors.Black;
    }
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="par">Command line options</param>
    public BaseEngine(ICommandLine par)
    {
        colors = new();
        FilesList = new();
        FilesList.AddRange(par.FilesList);

        if (!string.IsNullOrWhiteSpace(par.JSON))
        {
            SetJsonParams(par.JSON);
        }

        Dpi = par.Dpi;
        FillColor = par.FillColor;
        BorderColor = par.BorderColor;
    }
    #endregion

    #region json
    /// <summary>
    /// Returns the parameters in json format
    /// </summary>
    /// <returns></returns>
    public virtual string GetJsonParams() => string.Empty;

    /// <summary>
    /// Sets the common parameters to serialize json format
    /// </summary>
    public void GetBaseJsonParams()
    {
        parameters.BorderColor = colors.GetColorString(BorderColor);
        parameters.FillColor = colors.GetColorString(FillColor);
        parameters.Dpi = Dpi;
        parameters.Script = Script;
        parameters.FilesList = new();
        parameters.FilesList.AddRange(FilesList);
    }

    /// <summary>
    /// Sets the parameters from json formatted string
    /// </summary>
    /// <param name="json"></param>
    public virtual void SetJsonParams(string json)
    {
    }

    /// <summary>
    /// Sets the common parameters from deserialized json
    /// </summary>
    public void SetBaseJsonParams()
    {
        BorderColor = colors.GetColor(parameters.BorderColor);
        FillColor = colors.GetColor(parameters.FillColor);
        Dpi = parameters.Dpi;
        FilesList.Clear();
        FilesList.AddRange(parameters.FilesList);
    }
    #endregion

    #region build
    /// <summary>
    /// Does the dirty work
    /// </summary>
    /// <returns>Image to print</returns>
    public MagickImage GetResult() => GetResult(false);
    /// <summary>
    /// Does the dirty work
    /// </summary>
    /// <param name="quiet">suppress messages when running</param>
    /// <returns>Image to print</returns>
    public virtual MagickImage GetResult(bool quiet) => null;

    /// <summary>
    /// Writes exif infos on image
    /// </summary>
    /// <param name="image">image to process</param>
    public void SetImageParameters(MagickImage image) => fmt.SetImageParameters(image);

    /// <summary>
    /// Writes info text on images
    /// </summary>
    /// <param name="o">output related infos</param>
    /// <param name="image">image to process</param>
    public void SetImageInfo(string o, MagickImage image) => img.Info(WelcomeBannerText(), o).Draw(image);
    /// <summary>
    /// Writes info text on images
    /// </summary>
    /// <param name="i">input related infos</param>
    /// <param name="o">output related infos</param>
    /// <param name="image">image to process</param>
    public void SetImageInfo(string i, string o, MagickImage image) => img.Info(i, o).Draw(image);
    /// <summary>
    /// gets the program banner
    /// </summary>
    /// <returns></returns>
    public virtual string WelcomeBannerText() =>
        "Casasoft Contemporary Carte de Visite GUI\nCopyright (c) 2020-2022 Roberto Ceccarelli - Casasoft\n";
    #endregion
}