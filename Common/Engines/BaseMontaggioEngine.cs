// copyright (c) 2020-2026 Roberto Ceccarelli - Casasoft
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
using ImageMagick.Drawing;
using System;

namespace Casasoft.CCDV.Engines;

/// <summary>
/// Intermediate base class for engines that mount multiple CDV images
/// onto a standard paper sheet, sharing canvas gravity and per-image
/// load/script/resize logic.
/// </summary>
public abstract class BaseMontaggioEngine : BaseEngine
{
    #region properties
    /// <summary>
    /// Gravity used when placing the source image onto the CDV canvas
    /// </summary>
    public Gravity CanvasGravity { get; set; } = Gravity.Center;
    #endregion

    #region constructors
    /// <summary>
    /// Default constructor
    /// </summary>
    protected BaseMontaggioEngine() : base() { }

    /// <summary>
    /// Constructor from command line
    /// </summary>
    /// <param name="par">Command line options</param>
    protected BaseMontaggioEngine(CommandLine par) : base(par) { }
    #endregion

    #region json helpers
    /// <summary>
    /// Populates a <see cref="IMontaggioParameters"/> instance with the
    /// shared montagio fields (PaperFormat, CanvasGravity) plus all base
    /// fields. Call this from the concrete override of
    /// <see cref="BaseEngine.GetJsonParams"/>.
    /// </summary>
    /// <param name="p">The parameters instance to populate. Must not be null.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="p"/> is null.</exception>
    protected void GetBaseMontaggioJsonParams(IMontaggioParameters p)
    {
        if (p is null) throw new ArgumentNullException(nameof(p));
        GetBaseJsonParams();
        p.PaperFormat = PaperFormat;
        p.CanvasGravity = CanvasGravity;
    }

    /// <summary>
    /// Restores the shared montagio fields from a deserialised
    /// <see cref="IMontaggioParameters"/> instance. Call this from the
    /// concrete private SetJsonParams overload.
    /// </summary>
    /// <param name="p">The deserialised parameters to read values from. Must not be null.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="p"/> is null.</exception>
    protected void SetBaseMontaggioJsonParams(IMontaggioParameters p)
    {
        if (p is null) throw new ArgumentNullException(nameof(p));
        SetBaseJsonParams();
        PaperFormat = p.PaperFormat;
        CanvasGravity = p.CanvasGravity;
    }
    #endregion

    #region image loading
    /// <summary>
    /// Loads <paramref name="n"/> images starting at position
    /// <paramref name="counter"/> in <see cref="BaseEngine.FilesList"/>,
    /// resizes each one to <paramref name="orientation"/>, applies the
    /// optional user script, adds a 1-px border and appends the result to
    /// <paramref name="dest"/>. When the file list is exhausted it wraps
    /// around from the beginning. Equivalent to calling the overload with
    /// <c>postProcess: null</c>.
    /// </summary>
    /// <param name="n">Number of images to load.</param>
    /// <param name="counter">Start position index in <see cref="BaseEngine.FilesList"/>.</param>
    /// <param name="dest">Destination <see cref="MagickImageCollection"/> where loaded images are appended.</param>
    /// <param name="quiet">If true suppresses per-file console messages.</param>
    /// <param name="orientation">Final geometry for each loaded image.</param>
    /// <returns>The next index into <see cref="BaseEngine.FilesList"/> after loading <paramref name="n"/> images.</returns>
    protected int LoadImages(
        int n,
        int counter,
        MagickImageCollection dest,
        bool quiet,
        MagickGeometry orientation) =>
        LoadImages(n, counter, dest, quiet, orientation, orientation, null);

    /// <summary>
    /// Same as <see cref="LoadImages(int,int,MagickImageCollection,bool,MagickGeometry)"/>
    /// but applies <paramref name="postProcess"/> to each image after the user
    /// script and before the final resize to <paramref name="orientation"/>.
    /// Pass <c>null</c> for no post-processing (equivalent to the plain overload).
    /// </summary>
    /// <param name="n">Number of images to load.</param>
    /// <param name="counter">Start position index in <see cref="BaseEngine.FilesList"/>.</param>
    /// <param name="dest">Destination <see cref="MagickImageCollection"/> where loaded images are appended.</param>
    /// <param name="quiet">If true suppresses per-file console messages.</param>
    /// <param name="orientation">Final geometry for each loaded image.</param>
    /// <param name="postProcess">Optional transform applied after the script hook and before the final resize. May be null.</param>
    /// <returns>The next index into <see cref="BaseEngine.FilesList"/> after loading <paramref name="n"/> images.</returns>
    protected int LoadImages(
        int n,
        int counter,
        MagickImageCollection dest,
        bool quiet,
        MagickGeometry orientation,
        Func<MagickImage, MagickImage> postProcess) =>
        LoadImages(n, counter, dest, quiet, orientation, orientation, postProcess);

    /// <summary>
    /// Full overload: loads images at <paramref name="loadGeometry"/>, applies
    /// the optional script hook and <paramref name="postProcess"/>, then resizes
    /// to <paramref name="orientation"/> before adding to <paramref name="dest"/>.
    /// Separating the two geometries lets callers load at a smaller size
    /// (e.g. CDV_Internal_v) and composite at a larger one (e.g. CDV_Full_v).
    /// </summary>
    /// <param name="n">Number of images to load.</param>
    /// <param name="counter">Start position index in <see cref="BaseEngine.FilesList"/>.</param>
    /// <param name="dest">Destination <see cref="MagickImageCollection"/> where loaded images are appended.</param>
    /// <param name="quiet">If true suppresses per-file console messages.</param>
    /// <param name="loadGeometry">Geometry passed to <c>Utils.GetImage</c> for initial load.</param>
    /// <param name="orientation">Final target geometry after post-processing.</param>
    /// <param name="postProcess">Optional transform applied after the script hook and before the final resize. May be null.</param>
    /// <returns>The next index into <see cref="BaseEngine.FilesList"/> after loading <paramref name="n"/> images.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="dest"/> is null.</exception>
    protected int LoadImages(
        int n,
        int counter,
        MagickImageCollection dest,
        bool quiet,
        MagickGeometry loadGeometry,
        MagickGeometry orientation,
        Func<MagickImage, MagickImage> postProcess)
    {
        if (dest is null) throw new ArgumentNullException(nameof(dest));
        if (n <= 0) return counter;
        if (FilesList is null || FilesList.Count == 0) return counter;

        // ensure fmt is available to avoid possible null-reference warnings
        var localFmt = fmt ?? throw new InvalidOperationException("Formats instance (fmt) is not initialized.");

        int nImg = counter;
        for (int i = 0; i < n; i++)
        {
            string filename = FilesList[nImg] ?? string.Empty;
            if (!quiet) Console.WriteLine($"Processing: {filename}");

            MagickImage image = Utils.GetImage(filename, loadGeometry, CanvasGravity);
            image = ApplyLoadScript(image);

            if (postProcess is not null)
                image = postProcess(image);

            MagickImage slot = Utils.RotateResizeAndFill(image, orientation, FillColor, CanvasGravity);
            slot.BorderColor = BorderColor;
            slot.Border(1);
            dest.Add(slot);

            nImg++;
            if (nImg >= FilesList.Count) nImg = 0;
        }
        return nImg;
    }

    /// <summary>
    /// Loads a single image from <paramref name="filename"/>, fits it into
    /// <paramref name="targetGeometry"/> respecting <see cref="CanvasGravity"/>
    /// and runs the optional "ProcessOnLoad" user script entry-point.
    /// </summary>
    /// <param name="filename">Path of the source image. Must not be null or empty.</param>
    /// <param name="targetGeometry">Geometry to fit the image into.</param>
    /// <param name="quiet">Suppress console messages when true.</param>
    /// <returns>Processed image sized to <paramref name="targetGeometry"/>.</returns>
    /// <exception cref="ArgumentException"><paramref name="filename"/> is null or whitespace.</exception>
    protected MagickImage LoadSingleImage(string filename, MagickGeometry targetGeometry, bool quiet)
    {
        if (string.IsNullOrWhiteSpace(filename)) throw new ArgumentException("Filename must be provided", nameof(filename));
        if (!quiet) Console.WriteLine($"Processing: {filename}");

        MagickImage image = Utils.GetImage(filename, targetGeometry, CanvasGravity);
        image = ApplyLoadScript(image);

        return Utils.RotateResizeAndFill(image, targetGeometry, FillColor, CanvasGravity);
    }

    /// <summary>
    /// Runs the "ProcessOnLoad" entry-point of the compiled user script on
    /// <paramref name="image"/> when a script is present, otherwise returns
    /// <paramref name="image"/> unchanged.
    /// </summary>
    /// <param name="image">Image to process. Must not be null.</param>
    /// <returns>The processed <see cref="MagickImage"/> or the original image when no script is present.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="image"/> is null.</exception>
    protected MagickImage ApplyLoadScript(MagickImage image)
    {
        if (image is null) throw new ArgumentNullException(nameof(image));
        if (ScriptInstance is not null)
        {
            var result = Compiler.Run(ScriptInstance, "ProcessOnLoad", new object[] { image });
            if (result is not null)
                return (MagickImage)result;
        }
        return image;
    }
    #endregion

    #region layout helpers
    /// <summary>
    /// Returns how many portrait CDV columns fit horizontally on the given
    /// paper format, and the vertical offset (in pixels) of the first row.
    /// Used by subclasses to drive their compositing loops.
    /// </summary>
    /// <param name="format">Output paper format</param>
    /// <param name="portraitCount">Number of portrait CDV slots on the sheet (output).</param>
    /// <param name="landscapeCount">Number of landscape CDV slots on the sheet (output).</param>
    /// <param name="topOffset">Y offset in pixels for the first row of images (output).</param>
    protected void GetLayoutParameters(
        PaperFormats format,
        out int portraitCount,
        out int landscapeCount,
        out int topOffset)
    {
        // guard fmt to avoid possible null reference warnings
        var localFmt = fmt ?? throw new InvalidOperationException("Formats instance (fmt) is not initialized.");

        switch (format)
        {
            case PaperFormats.Small:
                portraitCount = 2;
                landscapeCount = 0;
                topOffset = 0;
                break;
            case PaperFormats.Medium:
                portraitCount = 3;
                landscapeCount = 0;
                topOffset = 0;
                break;
            case PaperFormats.Large:
            case PaperFormats.Large20x30:
                portraitCount = 4;
                landscapeCount = 2;
                topOffset = (int)localFmt.ToPixels(10);
                break;
            case PaperFormats.A4:
                portraitCount = 4;
                landscapeCount = 4;
                topOffset = (int)localFmt.ToPixels(5);
                break;
            case PaperFormats.Panorama:
                portraitCount = 2;
                landscapeCount = 0;
                topOffset = 0;
                break;
            default:
                portraitCount = 2;
                landscapeCount = 0;
                topOffset = 0;
                break;
        }
    }
    /// <summary>
    /// Draws cut-mark lines on <paramref name="final"/> according to
    /// <paramref name="format"/>. Shared by all montagio engines.
    /// </summary>
    /// <param name="final">The destination <see cref="MagickImage"/> that will be modified in-place. Must not be null.</param>
    /// <param name="format">Target <see cref="PaperFormats"/> that determines the placement of cut marks.</param>
    /// <exception cref="ArgumentNullException"><paramref name="final"/> is null.</exception>
    protected void DrawCutMarks(MagickImage final, PaperFormats format)
    {
        if (final is null) throw new ArgumentNullException(nameof(final));

        // guard fmt to avoid possible null reference warnings
        var localFmt = fmt ?? throw new InvalidOperationException("Formats instance (fmt) is not initialized.");

        Drawables draw = new();
        draw.StrokeColor(BorderColor).StrokeWidth(1);

        if (format == PaperFormats.Medium)
        {
            uint top = (final.Height - localFmt.CDV_Full_v.Height) / 2;
            uint left = (final.Width - localFmt.CDV_Full_v.Width * 3) / 2;
            Utils.HLine(draw, top, final.Width);
            Utils.HLine(draw, final.Height - top, final.Width);
            Utils.VLine(draw, left, final.Height);
            Utils.VLine(draw, final.Width - left, final.Height);
        }
        else
        {
            uint h = localFmt.ToPixels((uint)(format == PaperFormats.A4 ? 5 : 10));
            Utils.HLine(draw, h, final.Width);
            h += localFmt.CDV_Full_v.Height;
            Utils.HLine(draw, h, final.Width);
            h += format == PaperFormats.A4 ? localFmt.CDV_Full_v.Height : localFmt.CDV_Full_v.Width;
            Utils.HLine(draw, h, final.Width);
        }

        draw.Draw(final);
    }
    #endregion
}
