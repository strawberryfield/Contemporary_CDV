% MONTAGGIODORSI(1)  
% Roberto Ceccarelli - Casasoft  
% March 2022

# NAME
MontaggioDorsi - Assembling six images over a 20x27 cm paper

# SYNOPSIS
**MontaggioDorsi** \[options\]\* inputfile+

# DESCRIPTION
This program gathers six images on a 20x27cm surface   
that I print on a cardboard coated only on the side of the image.

# OPTIONS
**--paper=VALUE** :  
Output paper size:  
Large \(default\) 20x27cm  
Medium 15x20cm  


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
  colorname    \(use MontaggioDorsi --colors  to see all available colors\)

## JSON
Parameters can also be passed with a json formatted string  
using the following template:  

~~~
{
  "Paper": null,
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

~~~

These are the signatures of the scriptable methods:

~~~
// Script template for MontaggioDorsi

/// <summary>
/// Custom class initialization
/// </summary>
private void Init() { }

/// <summary>
/// Preprocesses the loaded image
/// </summary>
/// <param name="image">The loaded image</param>
/// <returns>The Processed image</returns>
public MagickImage ProcessOnLoad(MagickImage image) => image;
~~~

# COPYRIGHT
Casasoft MontaggioDorsi is free software:  
you can redistribute it and/or modify it  
under the terms of the GNU Affero General Public License as published by  
the Free Software Foundation, either version 3 of the License, or  
\(at your option\) any later version.  

You should have received a copy of the GNU AGPL v.3  
along with Casasoft MontaggioDorsi.  
If not, see <http://www.gnu.org/licenses/>.  

# DISCLAIMER
Casasoft MontaggioDorsi is distributed in the hope that it will be useful,  
but WITHOUT ANY WARRANTY; without even the implied warranty of  
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.   
See the GNU General Public License for more details.
