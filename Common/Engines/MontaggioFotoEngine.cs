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
using System.Text.Json;

namespace Casasoft.CCDV.Engines;

/// <summary>
/// MontaggioFoto engine.
/// Supports all paper formats defined in <see cref="PaperFormats"/>:
/// <list type="bullet">
///   <item><description><see cref="PaperFormats.Small"/> – 2 portrait CDV side by side (original behaviour)</description></item>
///   <item><description><see cref="PaperFormats.Medium"/> – 3 portrait CDV side by side</description></item>
///   <item><description><see cref="PaperFormats.Large"/> / <see cref="PaperFormats.Large20x30"/> – 4 portrait + 2 landscape CDV</description></item>
///   <item><description><see cref="PaperFormats.A4"/> – 4 portrait + 4 landscape CDV</description></item>
///   <item><description><see cref="PaperFormats.Panorama"/> – 2 portrait CDV side by side on panoramic paper</description></item>
/// </list>
/// All border/padding options (<see cref="FullSize"/>, <see cref="Trim"/>,
/// <see cref="WithBorder"/>, <see cref="Padding"/>) are applied uniformly to
/// every slot regardless of format.
/// </summary>
public class MontaggioFotoEngine : BaseMontaggioEngine
{
    #region properties
    /// <summary>
    /// When true the image is fitted to the full CDV size (100×64 mm)
    /// instead of the internal area (90×54 mm).
    /// </summary>
    public bool FullSize { get; set; } = false;
    /// <summary>
    /// When true any white border left by the resize is trimmed away.
    /// </summary>
    public bool Trim { get; set; } = false;
    /// <summary>
    /// When true the image (already fitted to CDV_Internal) is placed onto a
    /// full-CDV-sized canvas, adding a visible border around it.
    /// </summary>
    public bool WithBorder { get; set; } = false;
    /// <summary>
    /// Uniform blank padding (in pixels) added around each image.
    /// Ignored when <see cref="WithBorder"/> is true.
    /// </summary>
    public uint Padding { get; set; } = 0;
    #endregion

    #region constructors
    /// <summary>
    /// Default constructor
    /// </summary>
    public MontaggioFotoEngine() : base()
    {
        parameters = new MontaggioFotoParameters();
        ScriptingClass = new MontaggioFotoScripting();
        OutputName = "front";
    }

    /// <summary>
    /// Constructor from command line
    /// </summary>
    /// <param name="par">Command line options</param>
    public MontaggioFotoEngine(CommandLine par) : base(par)
    {
        parameters = new MontaggioFotoParameters();
        MontaggioFotoCommandLine p = (MontaggioFotoCommandLine)par;
        PaperFormat = p.PaperFormat;
        FullSize = p.FullSize ? true : FullSize;
        Trim = p.Trim ? true : Trim;
        WithBorder = p.WithBorder ? true : WithBorder;
        Padding = p.Padding;
        CanvasGravity = p.CanvasGravity;
        ScriptingClass = new MontaggioFotoScripting();
        Script = p.Script;
    }
    #endregion

    #region json
    /// <summary>
    /// Returns the parameters in json format
    /// </summary>
    public override string GetJsonParams()
    {
        MontaggioFotoParameters p = (MontaggioFotoParameters)parameters;
        GetBaseMontaggioJsonParams(p);
        p.FullSize = FullSize;
        p.WithBorder = WithBorder;
        p.Trim = Trim;
        p.Padding = Padding;
        return JsonSerializer.Serialize(p);
    }

    /// <summary>
    /// Sets the parameters from json formatted string
    /// </summary>
    public override void SetJsonParams(string json) =>
        SetJsonParams(JsonSerializer.Deserialize<MontaggioFotoParameters>(json));

    /// <summary>
    /// Sets the parameters from json deserialized object
    /// </summary>
    public override void SetJsonParams(IParameters json) =>
        SetJsonParams((MontaggioFotoParameters)json);

    private void SetJsonParams(MontaggioFotoParameters p)
    {
        parameters = p;
        SetBaseMontaggioJsonParams(p);
        FullSize = p.FullSize;
        WithBorder = p.WithBorder;
        Trim = p.Trim;
        Padding = p.Padding;
        Script = p.Script;
    }
    #endregion

    #region build
    /// <summary>
    /// Does the dirty work – entry point that starts from the first file.
    /// </summary>
    public override MagickImage GetResult(bool quiet) => GetResult(quiet, 0);

    /// <summary>
    /// Does the dirty work starting from file index <paramref name="startIndex"/>.
    /// Dispatches to the appropriate layout method based on
    /// <see cref="BaseEngine.PaperFormat"/>.
    /// </summary>
    /// <param name="quiet">Suppress console messages when true</param>
    /// <param name="startIndex">Index of the first file to process</param>
    public MagickImage GetResult(bool quiet, int startIndex)
    {
        _ = base.GetResult(quiet);

        return PaperFormat switch
        {
            PaperFormats.Small => BuildTwoUp(quiet, startIndex, fmt.FineArt10x15_o),
            PaperFormats.Panorama => BuildTwoUp(quiet, startIndex, fmt.FineArt10x18_o),
            PaperFormats.Medium => BuildMultiRow(quiet, startIndex),
            PaperFormats.Large => BuildMultiRow(quiet, startIndex),
            PaperFormats.Large20x30 => BuildMultiRow(quiet, startIndex),
            PaperFormats.A4 => BuildMultiRow(quiet, startIndex),
            _ => BuildTwoUp(quiet, startIndex, fmt.FineArt10x15_o)
        };
    }

    // -----------------------------------------------------------------------
    // Layout: 2-up (Small / Panorama) – original behaviour
    // -----------------------------------------------------------------------

    /// <summary>
    /// Produces a sheet with two portrait CDV images placed side by side.
    /// Each half also carries the informational text overlay.
    /// </summary>
    private MagickImage BuildTwoUp(bool quiet, int startIndex, MagickGeometry paperGeometry)
    {
        MagickImage final = GetOutputPaper(PaperFormat);

        int i = startIndex;
        MagickImage img1 = GetProcessed(FilesList[i], quiet);
        string name1 = FilesList[i];
        i++;

        MagickImage img2;
        string name2;
        if (i < FilesList.Count)
        {
            img2 = GetProcessed(FilesList[i], quiet);
            name2 = FilesList[i];
        }
        else
        {
            img2 = img.CDV_Internal_v();
            name2 = string.Empty;
        }

        final.Composite(HalfCard(img1, name1, paperGeometry), Gravity.West);
        final.Composite(HalfCard(img2, name2, paperGeometry), Gravity.East);
        return final;
    }

    // -----------------------------------------------------------------------
    // Layout: multi-row (Medium / Large / Large20x30 / A4)
    // -----------------------------------------------------------------------

    /// <summary>
    /// Produces a multi-row sheet.  Portrait CDV images fill the first row;
    /// landscape CDV images (rotated 90°) fill a second row where the format
    /// requires it.  The same border/padding options are applied to every slot.
    /// Cut marks are drawn as in <see cref="MontaggioDorsiEngine"/>.
    /// </summary>
    private MagickImage BuildMultiRow(bool quiet, int startIndex)
    {
        MagickImage final = GetOutputPaper(PaperFormat);

        GetLayoutParameters(PaperFormat,
            out int portraitCount,
            out int landscapeCount,
            out int topOffset);

        MagickImageCollection imagesV = new();
        MagickImageCollection imagesO = new();

        int nImg = startIndex;
        for (int i = 0; i < portraitCount; i++)
        {
            imagesV.Add(GetSlot(nImg, fmt.CDV_Full_v, quiet));
            nImg++;
            if (nImg >= FilesList.Count) nImg = 0;
        }

        for (int i = 0; i < landscapeCount; i++)
        {
            imagesO.Add(GetSlot(nImg, fmt.CDV_Full_o, quiet));
            nImg++;
            if (nImg >= FilesList.Count) nImg = 0;
        }

        // Cut marks
        DrawCutMarks(final);

        // Composite rows
        switch (PaperFormat)
        {
            case PaperFormats.Medium:
                final.Composite(imagesV.AppendHorizontally(), Gravity.Center, 0, 0);
                break;
            case PaperFormats.Large:
            case PaperFormats.Large20x30:
                final.Composite(imagesV.AppendHorizontally(), Gravity.North, 0, topOffset);
                final.Composite(imagesO.AppendHorizontally(), Gravity.North,
                    0, topOffset + (int)fmt.CDV_Full_v.Height - 1);
                break;
            case PaperFormats.A4:
                final.Composite(imagesV.AppendHorizontally(), Gravity.North, 0, topOffset);
                final.Composite(imagesO.AppendHorizontally(), Gravity.North,
                    0, topOffset + (int)fmt.CDV_Full_v.Height - 1);
                break;
        }

        if (PaperFormat is PaperFormats.Large or PaperFormats.Medium or PaperFormats.Large20x30)
        {
            SetImageInfo(WelcomeBannerText(), $"{OutputName}.{Extension}", final, PaperFormat);
        }

        return final;
    }

    /// <summary>
    /// Builds a single CDV slot: loads and processes the image (applying
    /// border/padding options at CDV_Internal scale), then resizes the result
    /// to <paramref name="orientation"/> (CDV_Full_v or CDV_Full_o) and adds
    /// a 1-px border.  The two-step sizing mirrors MontaggioDorsiEngine and
    /// preserves all border/padding semantics regardless of the paper format.
    /// </summary>
    private MagickImage GetSlot(int fileIndex, MagickGeometry orientation, bool quiet)
    {
        string filename = FilesList[fileIndex];
        MagickImage processed = GetProcessed(filename, quiet);
        MagickImage slot = Utils.RotateResizeAndFill(processed, orientation, FillColor, CanvasGravity);
        slot.BorderColor = BorderColor;
        slot.Border(1);
        return slot;
    }

    /// <summary>
    /// Draws cut-mark lines on <paramref name="final"/> according to the
    /// current <see cref="BaseEngine.PaperFormat"/>.
    /// </summary>
    private void DrawCutMarks(MagickImage final)
    {
        Drawables draw = new();
        draw.StrokeColor(BorderColor).StrokeWidth(1);

        if (PaperFormat == PaperFormats.Medium)
        {
            uint top = (final.Height - fmt.CDV_Full_v.Height) / 2;
            uint left = (final.Width - fmt.CDV_Full_v.Width * 3) / 2;
            Utils.HLine(draw, top, final.Width);
            Utils.HLine(draw, final.Height - top, final.Width);
            Utils.VLine(draw, left, final.Height);
            Utils.VLine(draw, final.Width - left, final.Height);
        }
        else
        {
            uint h = fmt.ToPixels((uint)(PaperFormat == PaperFormats.A4 ? 5 : 10));
            Utils.HLine(draw, h, final.Width);
            h += fmt.CDV_Full_v.Height;
            Utils.HLine(draw, h, final.Width);
            h += PaperFormat == PaperFormats.A4 ? fmt.CDV_Full_v.Height : fmt.CDV_Full_v.Width;
            Utils.HLine(draw, h, final.Width);
        }

        draw.Draw(final);
    }

    // -----------------------------------------------------------------------
    // Per-image processing (border / padding / trim)
    // -----------------------------------------------------------------------

    /// <summary>
    /// Loads, scripts and applies all border/padding options to a single image
    /// at <see cref="IFormats.CDV_Internal_v"/> size, returning it ready to be
    /// placed into a slot (where <see cref="GetSlot"/> will resize it to the
    /// target orientation) or directly into a half-card (2-up layouts).
    /// </summary>
    private MagickImage GetProcessed(string filename, bool quiet)
    {
        // Note: Console.WriteLine is NOT called here — LoadSingleImage handles
        // the log so it appears exactly once per image.
        MagickImage image = LoadSingleImage(filename, fmt.CDV_Internal_v, quiet);

        if (Trim) image.Trim();

        if (WithBorder)
        {
            return ApplyWithBorder(image);
        }

        if (Padding > 0)
        {
            return ApplyPadding(image);
        }

        return image;
    }

    /// <summary>
    /// Places <paramref name="image"/> (CDV_Internal size) onto a full-CDV
    /// canvas, offsetting it so that it appears centred within the border area.
    /// </summary>
    private MagickImage ApplyWithBorder(MagickImage image)
    {
        uint offset = Math.Min(
            fmt.CDV_Full_v.Width - image.Width,
            fmt.CDV_Full_v.Height - image.Height) / 2;

        uint offsetX = 0;
        uint offsetY = 0;

        switch (CanvasGravity)
        {
            case Gravity.Northwest:
            case Gravity.Northeast:
            case Gravity.Southwest:
            case Gravity.Southeast:
                offsetX = offset;
                offsetY = offset;
                break;
            case Gravity.South:
            case Gravity.North:
                offsetY = offset;
                break;
            case Gravity.West:
            case Gravity.East:
                offsetX = offset;
                break;
        }

        MagickImage canvas = img.CDV_Full_v(FillColor);
        canvas.Composite(image, CanvasGravity, (int)offsetX, (int)offsetY);
        return canvas;
    }

    /// <summary>
    /// Adds a uniform blank border of <see cref="Padding"/> pixels around
    /// <paramref name="image"/> using the fill colour.
    /// </summary>
    private MagickImage ApplyPadding(MagickImage image)
    {
        MagickGeometry size = fmt.CDV_Internal_v;
        if (Trim)
        {
            size = new MagickGeometry(image.Width, image.Height);
        }
        MagickImage canvas = img.Padded(FillColor, size, Padding);
        canvas.Composite(image, CanvasGravity);
        return canvas;
    }

    // -----------------------------------------------------------------------
    // Half-card helper (2-up layouts only)
    // -----------------------------------------------------------------------

    private Drawables BaseText()
    {
        Drawables d = new();
        d.FontPointSize(fmt.ToPixels(3) / 2)
            .Font("Arial")
            .FillColor(MagickColors.Black)
            .TextAlignment(TextAlignment.Left)
            .Gravity(Gravity.Northwest)
            .Rotation(90);
        return d;
    }

    /// <summary>
    /// Wraps <paramref name="image"/> in a half-sheet canvas and overlays
    /// the informational text (source filename, banner, run timestamp).
    /// </summary>
    private MagickImage HalfCard(MagickImage image, string filename, MagickGeometry paperGeometry)
    {
        image.BorderColor = BorderColor;
        image.Border(1);
        MagickImage half = new(MagickColors.White, paperGeometry.Width / 2, paperGeometry.Height);
        half.Composite(image, Gravity.Center);
        BaseText()
            .Text(fmt.ToPixels(5), -(int)half.Width + (int)fmt.ToPixels(3), $"Source: {filename}")
            .Text(fmt.ToPixels(5), -(int)fmt.ToPixels(3), WelcomeBannerText())
            .Text(half.Height / 2, -(int)fmt.ToPixels(3), $"Run {DateTime.Now:R}")
            .Draw(half);
        return half;
    }
    #endregion
}
