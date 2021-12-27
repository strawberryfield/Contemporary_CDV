% SCATOLA(1)  
% Roberto Ceccarelli - The Strawberry Field  
% Dec 2021

# NAME
Scatola - Box builder using a 20x27 cm paper

# SYNOPSIS
**Scatola** [options]*

# DESCRIPTION
This program allows you to create an exploded view of a box case for Carte de Visite.   
It has dimensions 105x70mm with a thickness that can be changed by a parameter on the command line.

# OPTIONS
**-a, --aboveimage=VALUE**
: set the image for the top cover

**-z, --bottomimage=VALUE**
: set the image for the bottom

**-l, --leftimage=VALUE**
: set the image for the left border

**-r, --rightimage=VALUE**
: set the image for the right border

**-f, --frontimage=VALUE**
: set the image for the front

**-b, --backimage=VALUE**
: set the image for the back

**-t, --thickness=VALUE**
: set the box thickness (default 5mm)

**--bordertext=VALUE**
: text to print on left border

**--font=VALUE**
: text font (default Arial)

**--fontbold**
: use bold font weight

**--fontitalic**
: use italic font style

**--format=VALUE**
: size of the box: 'cdv' or 'cc' (default 'CDV')

**--orientation=VALUE**
: orientation of the box: 'portrait' or 'landscape' (default 'PORTRAIT')

**--sample**
: generate sample images

**--fillcolor=VALUE**
: set the color used to fiil the images(default #FFFFFF)

**--bordercolor=VALUE**
: set the color used to border the images(default #000000)

**--dpi=VALUE**
: set output resolution (default 300)

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
: show program license (AGPL 3.0)

## COLORS
Colors can be written in any of these formats:  
  #rgb  
  #rrggbb  
  #rrggbbaa  
  #rrrrggggbbbb  
  #rrrrggggbbbbaaaa  
  colorname    (use Scatola --colors  to see all available colors)

## ENVIRONMENT VARIABLES
The program can read values from these variables:  
  CDV_OUTPATH  Base path for output files  
  CDV_DPI      Resolution for output files  
  CDV_FILL     Color used to fill images  
  CDV_BORDER   Border color

# COPYRIGHT
Casasoft Scatola is free software:  
you can redistribute it and/or modify it  
under the terms of the GNU Affero General Public License as published by  
the Free Software Foundation, either version 3 of the License, or  
(at your option) any later version.  

You should have received a copy of the GNU AGPL v.3  
along with Casasoft Scatola.  
If not, see <http://www.gnu.org/licenses/>.  

# DISCLAIMER
Casasoft CCDV Tools is distributed in the hope that it will be useful,  
but WITHOUT ANY WARRANTY; without even the implied warranty of  
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.   
See the GNU General Public License for more details.
