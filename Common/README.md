# Casasoft Contemporary Carte de Visite Tools — Common Library

[![NuGet](https://img.shields.io/nuget/v/Casasoft.CCDV.Common)](https://www.nuget.org/packages/Casasoft.CCDV.Common/)
[![License: AGPL v3](https://img.shields.io/badge/License-AGPL%20v3-blue.svg)](https://www.gnu.org/licenses/agpl-3.0)

Core library for the
[Casasoft Contemporary Carte de Visite Tools suite](https://github.com/strawberryfield/Contemporary_CDV).  
All tools in the suite share this library for image processing, format management,
command-line parsing, JSON parameter serialisation, and run-time C# scripting.

---

## The photographic "carte de visite"

In 1854, the Parisian photographer [André Adolphe Eugène Disderi](https://it.wikipedia.org/wiki/Andr%C3%A9-Adolphe-Eug%C3%A8ne_Disd%C3%A9ri) , 
who had owned a studio on the Boulevard des Italiens since 1848, discovered a method for producing eight different negatives on a single plate, 
probably drawing inspiration from stereoscopic photographs, taken with twin-lens cameras and already in use since 1850.

The camera designed and patented by Disdéri incorporated four lenses and a sliding plate holder, 
which allowed the negative to be positioned so that it could be exposed multiple times. 
Ten exposures were envisaged and described in the 1854 patent, though in reality only eight survived intact on the uncut sheets.

In this way, it was possible to expose, develop and print many small-format images simultaneously, 
thereby reducing the production costs of each individual photograph.

This led to the development of a form of mass production of images that brought about significant changes in the world of photography. 
The dimensions of a carte de visite were approximately 54 mm (2.125 in.) in width and 89 mm (3.5 in.) in height for vertical images, 
and the reverse for horizontal ones.

The image was printed on thin, compact paper, usually albumen paper. 
It is possible to find antique examples printed on salted paper, but these are very rare and valuable. 
More recent were the collodion, aristotype and other processes.

These prints were then mounted, almost always using heat, onto a rigid paper backing. The dimensions of the card were approximately 64 mm (2.5 in.) by 100 mm (4 in.)

The difference between the printed paper and the backing onto which it was glued created a sort of passe-partout where it was possible to insert 
the imprint of the stamp, either inked or embossed, of the photographic studio, the location of the photograph, the title of the image or other details. 
The reverse of the card was also used to provide this information, either printed or handwritten. Usually, any medals and prizes won were also listed on the reverse.

Visiting cards proved an enormous success, captivating both the powerful and ordinary people: 
thus, even those of modest social standing had their portraits taken surrounded by an air of apparent wealth, 
thanks to the furnishings and decorative items in Disderi’s studios. 
Before long, a fashion took hold for collecting carte de visite of figures from politics, culture and current affairs: 
in Victorian circles, the portrait of the Queen Mother opened the albums dedicated to celebrities, albums with more or less elaborate bindings, 
but all equally designed to house the magical format in series.

In short, the Facebook of the 19th century.

Initially, the carte de visite was a tool for recognition and self-celebration among the rising middle class, 
but it soon proved to be a convenient cultural resource for learning about distant places and works of art.

The CdV chronicles and bears witness to an entire historical period stretching from the Risorgimento to the First World War, 
documenting customs, values and attitudes. 
Its irreplaceable value as a resource for historical and social study and research, combined with its characteristic form as a simple sticker, 
has made it a sought-after collector’s item over time.

### The contemporary carte de visite

The idea of a photographic business card, on a permanent physical medium, 
remains revolutionary even today, in this age of fleeting images and information.

The aim of those involved in **contemporary carte-de-viste** is to highlight their relevance today in terms of their function 
as a means of communication and a record of society, and in their role as a tiny vehicle for artistic expression: 
pocket-sized, practical, affordable; a tangible and lasting mark of presence in an age of fleeting images.

The first [international competition for Contemporary Carte de Visite, Gianluigi Parpani Award 'Il Mondo in Tasca', Lodi, 2019](https://www.gri.it//support/catalogo-cdv-2019.pdf)
revealed that there is still ample scope for innovation and wonder in the photography of the future, which knows how to reinvent the past.

This is confirmed by the reinterpretation of traditional processes, from oleotype to bichromate gum, salt print and cyanotype... 
Contemporary artists who today creatively reinterpret the Carte de Visite take up the challenge of working within a tiny space 
to create something grand and innovative.

### About these tools

Commercial print services use standard paper formats larger than a single CDV, so multiple
cards must be laid out on a single sheet; exactly as Disdéri originally did with his glass
plates. These tools automate that layout step with precise cut marks, multiple paper formats,
and optional run-time C# scripting for custom per-image processing.

A more detailed story (in Italian) is on
[«The Strawberry Field»](https://strawberryfield.altervista.org/carte_de_visite/).

---

## Requirements

| Dependency | Minimum version |
|---|---|
| .NET | 8.0 or 10.0 |
| Magick.NET-Q16-AnyCPU | ≥ 14.13.1 |
| Microsoft.CodeAnalysis.Common | ≥ 5.3.0 |
| Microsoft.CodeAnalysis.CSharp | ≥ 5.3.0 |
| Mono.Options | ≥ 6.12.0.148 |
| Casasoft.XAML.Controls | ≥ 26.5.23 |

A companion package
[Casasoft.CCDV.Script.Templates](https://www.nuget.org/packages/Casasoft.CCDV.Script.Templates/)
provides test templates to help write and test custom scripts.

---

## Paper formats

All engines share the `PaperFormats` enum:

| Value | Dimensions | Typical use |
|---|---|---|
| `Small` | 10 × 15 cm | 2 portrait CDV side by side |
| `Panorama` | 10 × 18 cm | 2 portrait CDV side by side (panoramic strip) |
| `Medium` | 15 × 20 cm | 3 portrait CDV side by side |
| `Medium13x17` | 12.7 × 16.7 cm | MontaggioDorsi/MontaggioFoto: 2 carte verticali affiancate (stesso layout di `Small`). Folder/Scatola: stesso esploso di `Medium`, tagliato sul lato destro se non entra nel foglio. |
| `Large` | 20 × 27 cm | 4 portrait + 2 landscape CDV (InCartha service) |
| `Large20x30` | 20 × 30 cm | 4 portrait + 2 landscape CDV (FineArt paper) |
| `A4` | 21 × 29.7 cm | 4 portrait + 4 landscape CDV |

For `Large` and `Large20x30` the engine adds a 10 mm top margin; for `A4` a 5 mm margin.
Cut marks are drawn automatically on all multi-row formats.

---

## CDV dimensions

| Geometry | Size |
|---|---|
| `CDV_Full_v` / `CDV_Full_o` | 64 × 100 mm (portrait / landscape) |
| `CDV_Internal_v` / `CDV_Internal_o` | 54 × 90 mm — leaves a 5 mm border on every side |

`MontaggioFoto` loads images at `CDV_Internal` size by default, then composites them on a
`CDV_Full` canvas so the characteristic white border is preserved. Use `--fullsize` /
`FullSize = true` to skip the border.

---

## Class reference

### ImageMagick-related classes

**`Colors`**  
Converts between string representations (`#rrggbb`, `#rrggbbaa`, named colours) and
`MagickColor`. The full list of named colours can be queried with `--colors` on any
command-line tool.

**`Formats` / `IFormats`**  
Converts millimetres to pixels at the configured DPI and exposes all standard geometries
(`CDV_Full_v`, `FineArt10x15_o`, `InCartha20x27_o`, `A4_o`, …).

**`Images` / `IImages`**  
Factory for pre-sized blank `MagickImage` canvases at each standard format.

**`PaperFormats`**  
Enum (`Small`, `Medium`, `Large`, `Panorama`, `A4`, `Large20x30`).

**`Utils`**  
Static helpers:
- `AutoRotate` — rotates an image so its orientation matches a target geometry.
- `ResizeAndFill` — resizes and extends the canvas with a fill colour.
- `RotateResizeAndFill` — combines `AutoRotate` + `ResizeAndFill` in one call.
- `GetImage` — loads an image from a file path or from an ImageMagick built-in canvas
  specification (`xc:color`, `gradient:c1-c2`, `pango:text`, …).
- `GetPaperFormat` — parses a paper format string (case-insensitive, supports both
  `"Large20x30"` and `"20x30"`).
- `CenteredText`, `HLine`, `VLine` — drawing helpers.
- `GetLicense` — returns the embedded AGPL 3.0 licence text.

---

### Command-line handling

All classes use **Mono.Options** and implement `ICommandLine`.

**`CommandLine`**  
Base class. Provides `--fillcolor`, `--bordercolor`, `--dpi`, `--json`, `--script`,
`--output`, `--extension`, `--tag`, `--help`, `--helpjson`, `--helpscript`, `--colors`,
`--license`, `--man`, `--nobanner`. Reads environment variables `CDV_OUTPATH`, `CDV_DPI`,
`CDV_FILL`, `CDV_BORDER`. The `--json=@filename` syntax lets parameters be stored in a
file.

**`MontaggioDorsiCommandLine`**  
Adds `--paper` (Large / Medium / A4 / 20x30) and `--gravity`.

**`MontaggioFotoCommandLine`**  
Adds `--paper` (Small / Medium / Large / A4 / 20x30), `--fullsize`, `--withborder`,
`--trim`, `--padding`, `--gravity`.

**`BaseBuilderCommandLine`**  
Shared base for `Scatola` and `Cartella`. Adds image paths for all six faces, thickness,
border text, font, orientation, and target format (CDV or credit card).

**`CreditCardCommandLine`**  
Adds front-text options, magnetic-band colour and overlay, back image, and back text
(Pango markup supported).

**`CubettiCommandLine`**  
Adds `--rows`, `--columns`, `--size`, `--paper`, `--sample`.

**`FlexagonCommandLine`**  
Adds `--faces` (3, 4, or 6) and `--sample`.

---

### JSON parameter classes

All classes implement `IParameters` and can be serialised/deserialised with
`System.Text.Json`. The `--helpjson` flag on any tool prints a ready-to-use template.

**`CommonParameters`**  
`FillColor`, `BorderColor`, `Dpi`, `OutputName`, `Extension`, `Script`, `Tag`,
`FilesList`.

**`BaseMontaggioParameters : CommonParameters, IMontaggioParameters`**  
Adds `Paper` (string, round-trips through `PaperFormats`) and `CanvasGravity`.
Used by `MontaggioDorsiEngine`.

**`MontaggioFotoParameters : BaseMontaggioParameters`**  
Adds `FullSize`, `Trim`, `WithBorder`, `Padding`.

**`BaseBuilderParameters : CommonParameters`**  
Adds all six face image paths, thickness, border text, font, orientation, target format,
`Paper`, `PaperFormat`.

**`CreditCardParameters`, `CubettiParameters`, `FlexagonParameters`**  
Tool-specific parameter sets.

**`IMontaggioParameters`**  
Interface shared by `BaseMontaggioParameters` and `MontaggioFotoParameters`. Exposes
`PaperFormat` and `CanvasGravity` so `BaseMontaggioEngine` can handle JSON without
knowing the concrete type.

---

### Engine classes

All engines implement `IEngine` and inherit from `BaseEngine`.

#### `BaseEngine`

Manages `FilesList`, `FillColor`, `BorderColor`, `Dpi`, `OutputName`, `Extension`,
`Tag`, `Script`, `PaperFormat`, and the scripting infrastructure (`ScriptingClass`,
`CustomCode`, `ScriptInstance`).

Constructor priority when both `--json` and command-line values are present:
**JSON wins**. The constructor applies command-line values first as defaults, then calls
`SetJsonParams` which overwrites everything, including `FilesList`.

Key methods: `GetResult()`, `GetResult(bool quiet)`, `GetResults()`,
`GetOutputPaper(PaperFormats)`, `SetImageParameters(MagickImage)`,
`SetImageInfo(…)`, `GetJsonParams()`, `SetJsonParams(string)`.

#### `BaseMontaggioEngine : BaseEngine`

Abstract intermediate class for engines that tile CDV images onto a sheet.

Additional members:
- `CanvasGravity` — `Gravity` used when placing source images.
- `LoadImages(n, counter, dest, quiet, orientation)` — loads _n_ images from `FilesList`
  (wrapping cyclically), applies `AutoRotate`, resizes to `orientation`, runs the
  `ProcessOnLoad` script hook, adds a 1-px border.
- `LoadImages(…, loadGeometry, orientation, postProcess)` — full overload: loads at
  `loadGeometry`, applies the optional `Func<MagickImage, MagickImage>` post-processor,
  then resizes to `orientation`. Used by `MontaggioFotoEngine` to separate the internal
  load size from the final slot size.
- `LoadSingleImage(filename, targetGeometry, quiet)` — loads, auto-rotates, and resizes
  a single image; also runs the script hook.
- `ApplyLoadScript(image)` — runs the `ProcessOnLoad` entry-point of the compiled user
  script, or returns the image unchanged.
- `GetLayoutParameters(format, out portraitCount, out landscapeCount, out topOffset)` —
  returns the slot counts and vertical offset for each paper format.
- `DrawCutMarks(final, format)` — draws cut-mark lines shared by both montagio engines.
- `GetBaseMontaggioJsonParams(p)` / `SetBaseMontaggioJsonParams(p)` — JSON round-trip
  helpers for `PaperFormat`, `CanvasGravity`, and all base fields.

#### `MontaggioDorsiEngine : BaseMontaggioEngine`

Assembles CDV **back** images on a sheet. Supports `Medium` (3 portrait), `Large` /
`Large20x30` (4 portrait + 2 landscape), and `A4` (4 portrait + 4 landscape). When no
files are supplied, blank placeholders are used. Uses `BaseMontaggioParameters` for JSON.

#### `MontaggioFotoEngine : BaseMontaggioEngine`

Assembles CDV **front** images on a sheet. Supports all six paper formats:

| Format | Layout | Output mode |
|---|---|---|
| `Small` / `Panorama` / `Medium13x17` | 2-up with info overlay | One numbered file per pair |
| `Medium` | 3 portrait | Single file |
| `Large` / `Large20x30` | 4 portrait + 2 landscape | Single file |
| `A4` | 4 portrait + 4 landscape | Single file |

Border/padding options applied uniformly to every slot regardless of format:
- `FullSize` — fit image to full 100 × 64 mm CDV area.
- `WithBorder` — place the image (CDV_Internal size) on a full-CDV canvas with a visible
  5 mm white border.
- `Trim` — remove any white border introduced by the resize.
- `Padding` — add a uniform blank margin (in mm) around the image.

The 2-up layout (`BuildTwoUp`) also overlays informational text (source filename, tool
banner, run timestamp) in the margin of each half-card.

`GetResult(bool quiet, int startIndex)` lets the caller start from an arbitrary position
in `FilesList`, enabling the multi-file loop in `Program.cs` for `Small` / `Panorama`.

#### `BaseBuilderEngine : BaseEngine`

Abstract wrapper for box and folder builders. Holds a reference to `IBuilder` and
forwards `SetJsonParams` / `GetResult` to the builder.

#### `ScatolaEngine` / `FolderEngine`

Thin wrappers over `ScatolaBuilder` / `FolderBuilder` that place the built image on the
output paper and add cut marks.

#### `CreditCardEngine`

Produces a two-sided credit-card layout (front + rear, landscape) with optional magnetic
band, front text (with background rectangle), and Pango-formatted back text.

#### `CubettiEngine`

Generates a puzzle of cubes from six source images, sliced into a _rows × columns_ grid.
Each cube face gets its own output file with cut and fold marks. Supports sample images
for testing.

#### `FlexagonEngine`

Generates TetraFlexagons with 3, 4, or 6 faces from source CDV images.

---

### Scripting

Scripts are C# source files compiled at run-time by `Compiler` (Roslyn). They are wrapped
in a class that receives a strongly-typed engine reference, so all engine properties and
methods are accessible.

Enable with `--script=path/to/script.cs` or `--script=@filename`. Use `--helpscript` to
print the template for any tool.

**Available entry-points** (return `null` to keep the default behaviour):

| Method | Description |
|---|---|
| `void Init()` | Called once after the engine is constructed |
| `MagickImage OutputImage()` | Override the blank output canvas |
| `MagickImage ProcessOnLoad(MagickImage image)` | Transform each source image after loading |
| `MagickImage ProcessOnLoad{Face}(MagickImage image)` | Per-face hook for box/folder builders (`Front`, `Back`, `Top`, `Bottom`, `Left`, `Right`) |

**Scripting classes:**

| Class | Wraps engine |
|---|---|
| `BaseScripting` | `BaseEngine` |
| `MontaggioDorsiScripting` | `MontaggioDorsiEngine` |
| `MontaggioFotoScripting` | `MontaggioFotoEngine` |
| `BaseBuilderScripting` | `BaseBuilderEngine` |
| `CreditCardScripting` | `CreditCardEngine` |
| `CubettiScripting` | `CubettiEngine` |
| `FlexagonScripting` | `FlexagonEngine` |

The `Compiler` static class exposes `Compile(string sourceCode)`, `New(assembly, engine)`,
and `Run(obj, method, args)` for instantiating and invoking user scripts.

---

## JSON parameter files

Any tool accepts `--json=@parameters.json` to load all settings from a file. The JSON
completely overrides command-line defaults for colours, DPI, paper format, gravity, and
file lists. Use `--helpjson` to print a ready-to-use template. A minimal example for
`MontaggioFoto`:

```json
{
  "Paper": "Large20x30",
  "CanvasGravity": 5,
  "FullSize": false,
  "WithBorder": false,
  "Trim": false,
  "Padding": 0,
  "FillColor": "#FFFFFF",
  "BorderColor": "#000000",
  "Dpi": 300,
  "OutputName": "",
  "Extension": "jpg",
  "Script": "",
  "Tag": "",
  "FilesList": [
    "C:\\photos\\01_front.jpg",
    "C:\\photos\\02_front.jpg"
  ]
}
```

---

## Environment variables

| Variable | Description |
|---|---|
| `CDV_OUTPATH` | Base directory prepended to relative output paths |
| `CDV_DPI` | Default output resolution (overrides the built-in 300 DPI default) |
| `CDV_FILL` | Default fill colour |
| `CDV_BORDER` | Default border colour |

---

## Colour syntax

Colours can be specified in any of these formats:

```
#rgb
#rrggbb
#rrggbbaa
#rrrrggggbbbb
#rrrrggggbbbbaaaa
colorname          (use --colors to list all named colours)
```

---

## Built-in canvas generators

Instead of a file path, any image slot can use an ImageMagick canvas specification:

| Prefix | Description |
|---|---|
| `xc:color` | Solid fill |
| `gradient:c1-c2` | Linear gradient (top to bottom) |
| `radial-gradient:c1-c2` | Radial gradient |
| `plasma:c1-c2` | Plasma gradient |
| `plasma:fractal` | Random plasma |
| `label:text` | Plain text, no word-wrap |
| `caption:text` | Plain text, auto word-wrap |
| `pango:text` | Pango-markup formatted text |

The text argument can be stored in a file and referenced as `pango:@filename`.

---

## Licence

Casasoft Contemporary Carte de Visite Tools is free software distributed under the
**GNU Affero General Public License v3.0** or later.  
See [http://www.gnu.org/licenses/](http://www.gnu.org/licenses/).

Copyright © 2020-2026 Roberto Ceccarelli — Casasoft  
[https://strawberryfield.altervista.org/carte_de_visite/](https://strawberryfield.altervista.org/carte_de_visite/)
