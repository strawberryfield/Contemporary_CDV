% CREDITCARD(1)  
% Roberto Ceccarelli - Casasoft  
% March 2022

# NAME
CreditCard - Creates a credit card recto and verso.

# SYNOPSIS
**CreditCard** \[options\]\* inputfile

# DESCRIPTION
This program allows you to create a credit card with recto and verso.     
You can add a pseudo magnetic band, text on front and a more long text on the back.

# OPTIONS
**--fronttext=VALUE** :  
text in front \(Cardholder name\)  


**--fronttextfont=VALUE** :  
front text font \(default 'Arial'\)  


**--fronttextcolor=VALUE** :  
front text color \(default \#000000\)  


**--fronttextborder=VALUE** :  
  
front text border color \(default \#000000\)  


**--fronttextbackground=V** :  
ALUE  
front text background color \(default \#00000000\)  


**--fontbold** :  
use bold font weight  


**--fontitalic** :  
use italic font style  


**--mbcolor=VALUE** :  
magnetic band color \(default \#8B4513\)  


**--mbimage=VALUE** :  
magnetic band overlay image  


**--backimage=VALUE** :  
image for back side  


**--backtext=VALUE** :  
pango markup for text on back side.  
Text can be stored in a file instead of a string.  
The file must be referenced as '@filename'  


**--fillcolor=VALUE** :  
set the color used to fiil the images  
\(default \#FFFFFF\)  


**--bordercolor=VALUE** :  
set the color used to border the images  
\(default \#000000\)  


**--dpi=VALUE** :  
set output resolution \(default 300\)  


**--json=VALUE** :  
parameters in json format,  
use --helpjson for sample template  
Text can be stored in a file instead of a string  
The file must be referenced as '@filename'  


**--script=VALUE** :  
c\# script for custom processing,  
use --helpscript for sample template  
Text can be stored in a file instead of a string  
The file must be referenced as '@filename'  


**-o, --output=VALUE** :  
set output dir/filename  


**--tag=VALUE** :  
extra info for user scripts  
Text can be stored in a file instead of a string  
The file must be referenced as '@filename'  


**--nobanner** :  
suppress the banner  


**-h, --help** :  
show this message and exit  


**--helpjson** :  
show json parameters template  


**--helpscript** :  
show script template  


**--man** :  
show the man page source and exit  


**--colors** :  
list available colors by name  


**--license** :  
show program license \(AGPL 3.0\)  


## COLORS
Colors can be written in any of these formats:  
  \#rgb  
  \#rrggbb  
  \#rrggbbaa  
  \#rrrrggggbbbb  
  \#rrrrggggbbbbaaaa  
  colorname    \(use CreditCard --colors  to see all available colors\)

## JSON
Parameters can also be passed with a json formatted string  
using the following template:  

~~~
{
  "FrontText": "",
  "FrontTextFont": "Arial",
  "FrontTextColor": "#FFFFFF",
  "FrontTextBorder": "#000000",
  "FrontTextBackground": "#00000000",
  "fontBold": false,
  "fontItalic": false,
  "MagneticBandColor": "#000000FF",
  "MagneticBandImage": "",
  "BackImage": "",
  "BackText": "",
  "FillColor": "#FFFFFF",
  "BorderColor": "#000000",
  "Dpi": 300,
  "OutputName": null,
  "Script": null,
  "Tag": null,
  "FilesList": []
}
~~~

## ENVIRONMENT VARIABLES
The program can read values from these variables:  
  CDV\_OUTPATH  Base path for output files  
  CDV\_DPI      Resolution for output files  
  CDV\_FILL     Color used to fill images  
  CDV\_BORDER   Border color

# SCRIPTING
You can add custom c# code, compiled at runtime, with the --script parameter.
You can call a property *engine* that exposes all the parameters passed
to the main program.

The following using are declared:  
~~~

using Casasoft.CCDV;
using Casasoft.CCDV.Engines;
using Casasoft.CCDV.JSON;
using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

~~~

These are the signatures of the scriptable methods:

~~~
// Script template for CreditCard

/// <summary>
/// Custom class initialization
/// </summary>
public void Init() { }

/// <summary>
/// Image for final output
/// </summary>
/// <returns></returns>
public MagickImage OutputImage() => null;

~~~

# COPYRIGHT
Casasoft CreditCard is free software:  
you can redistribute it and/or modify it  
under the terms of the GNU Affero General Public License as published by  
the Free Software Foundation, either version 3 of the License, or  
\(at your option\) any later version.  

You should have received a copy of the GNU AGPL v.3  
along with Casasoft CreditCard.  
If not, see <http://www.gnu.org/licenses/>.  

# DISCLAIMER
Casasoft CreditCard is distributed in the hope that it will be useful,  
but WITHOUT ANY WARRANTY; without even the implied warranty of  
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.   
See the GNU General Public License for more details.
