% CREDITCARD(1)  
% Roberto Ceccarelli - Casasoft  
% Jan 2022

# NAME
CreditCard - Creates a credit card recto and verso.

# SYNOPSIS
**CreditCard** \[options\]\* inputfile

# DESCRIPTION
This program allows you to create a credit card with recto and verso.   
You can add a pseudo magnetic band, text on front and a more long text on the back.

# OPTIONS
**--fronttext=VALUE**
: text in front \(Cardholder name\)

**--fronttextfont=VALUE**
: front text font \(default 'Arial'\)

**--fronttextcolor=VALUE**
: front text color \(default \#000000\)

**--fronttextborder=VALUE**
: front text border color \(default \#000000\)

**--fontbold**
: use bold font weight

**--fontitalic**
: use italic font style

**--mbcolor=VALUE**
: magnetic band color \(default \#8B4513\)

**--mbimage=VALUE**
: magnetic band overlay image

**--backimage=VALUE**
: image for back side

**--backtext=VALUE**
: pango markup for text on back side.Text can be stored in a file instead of a string.The file must be referenced as '@filename'

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
  colorname    \(use CreditCard --colors  to see all available colors\)

## ENVIRONMENT VARIABLES
The program can read values from these variables:  
  CDV\_OUTPATH  Base path for output files  
  CDV\_DPI      Resolution for output files  
  CDV\_FILL     Color used to fill images  
  CDV\_BORDER   Border color

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
Casasoft CCDV Tools is distributed in the hope that it will be useful,  
but WITHOUT ANY WARRANTY; without even the implied warranty of  
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.   
See the GNU General Public License for more details.
