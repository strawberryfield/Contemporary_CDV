% CUBETTI(1)  
% Roberto Ceccarelli - Casasoft  
% April 2023

# NAME
Cubetti - Creates a cube matrix with 6 photos.

# SYNOPSIS
**Cubetti** \[options\]\* inputfiles

# DESCRIPTION
This program allows you to create a cube matrix with 6 photos.     
Each face of the cube has a portion of one of the six images.

# OPTIONS
**-r, --rows=VALUE** :  
set the number of rows of the output matrix \(  
default 2\)  


**-c, --columns=VALUE** :  
set the number of columns of the output matrix \(  
default 2\)  


**-s, --size=VALUE** :  
set the size of each cube \(default 50mm\)  


**--paper=VALUE** :  
Output paper size:  
Large 20x27cm  
Medium \(default\) 15x20cm  
A4 210x297mm  


**--sample** :  
generate sample images  


**--fillcolor=VALUE** :  
set the color used to fill the images  
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


**--extension=VALUE** :  
file extension for output file \(default 'jpg'\)  


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
  colorname    \(use Cubetti --colors  to see all available colors\)

## JSON
Parameters can also be passed with a json formatted string  
using the following template:  

~~~
{
  "Rows": 2,
  "Columns": 3,
  "Size": 50,
  "Paper": null,
  "FillColor": "#FFFFFF",
  "BorderColor": "#000000",
  "Dpi": 300,
  "OutputName": "",
  "Extension": "jpg",
  "Script": null,
  "Tag": null,
  "FilesList": []
}
~~~

## ENVIRONMENT VARIABLES
The program can read values from these variables:  

**CDV\_OUTPATH** :  
Base path for output files


**CDV\_DPI** :  
Resolution for output files


**CDV\_FILL** :  
Color used to fill images


**CDV\_BORDER** :  
Border color




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
// Script template for Cubetti

/// <summary>
/// Custom class initialization
/// </summary>
public void Init() { }

/// <summary>
/// Image for final output
/// </summary>
/// <returns></returns>
public MagickImage OutputImage() => null;

/// <summary>
/// Preprocesses the loaded image
/// </summary>
/// <param name="image">The loaded image</param>
/// <returns>The Processed image</returns>
public MagickImage ProcessOnLoad(MagickImage image) => image;
~~~


# COPYRIGHT
Casasoft Casasoft Cubetti is free software:  
you can redistribute it and/or modify it  
under the terms of the GNU Affero General Public License as published by  
the Free Software Foundation, either version 3 of the License, or  
\(at your option\) any later version.  

You should have received a copy of the GNU AGPL v.3  
along with Casasoft Casasoft Cubetti.  
If not, see <http://www.gnu.org/licenses/>.  

# DISCLAIMER
Casasoft Casasoft Cubetti is distributed in the hope that it will be useful,  
but WITHOUT ANY WARRANTY; without even the implied warranty of  
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.   
See the GNU General Public License for more details.

