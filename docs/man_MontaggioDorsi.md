% MONTAGGIODORSI(1)  
% Roberto Ceccarelli - Casasoft  
% Jan 2022

# NAME
MontaggioDorsi - Assembling six images over a 20x27 cm paper

# SYNOPSIS
**MontaggioDorsi** \[options\]\* inputfile+

# DESCRIPTION
This program gathers six images on a 20x27cm surface 
that I print on a cardboard coated only on the side of the image.

# OPTIONS
**--fillcolor=VALUE**
: set the color used to fiil the images\(default \#FFFFFF\)

**--bordercolor=VALUE**
: set the color used to border the images\(default \#000000\)

**--dpi=VALUE**
: set output resolution \(default 300\)

**-o, --output=VALUE**
: set output dir/filename

**--nobanner**
: suppress the banner

**-h, --help**
: show this message and exit

**--man**
: show the man page source and exit

**--colors**
: list available colors by name

**--license**
: show program license \(AGPL 3.0\)

## COLORS
Colors can be written in any of these formats:  
  \#rgb  
  \#rrggbb  
  \#rrggbbaa  
  \#rrrrggggbbbb  
  \#rrrrggggbbbbaaaa  
  colorname    \(use MontaggioDorsi --colors  to see all available colors\)

## ENVIRONMENT VARIABLES
The program can read values from these variables:  
  CDV\_OUTPATH  Base path for output files  
  CDV\_DPI      Resolution for output files  
  CDV\_FILL     Color used to fill images  
  CDV\_BORDER   Border color

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
Casasoft CCDV Tools is distributed in the hope that it will be useful,  
but WITHOUT ANY WARRANTY; without even the implied warranty of  
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.   
See the GNU General Public License for more details.
