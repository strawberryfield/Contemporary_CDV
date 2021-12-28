% MONTAGGIOFOTO(1)  
% Roberto Ceccarelli - The Strawberry Field  
% Dec 2021

# NAME
MontaggioFoto - Assembling two images over a 10x15 cm paper

# SYNOPSIS
**MontaggioFoto** [options]* inputfile+

# DESCRIPTION
This program gathers two images on a 10x15cm surface 
that you can print on a paper of ypur choice.

# OPTIONS
**--fullsize**
: resize image to full format

**--withborder**
: include border to full format

**--trim**
: trim white space

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
  colorname    (use MontaggioFoto --colors  to see all available colors)

## ENVIRONMENT VARIABLES
The program can read values from these variables:  
  CDV_OUTPATH  Base path for output files  
  CDV_DPI      Resolution for output files  
  CDV_FILL     Color used to fill images  
  CDV_BORDER   Border color

# COPYRIGHT
Casasoft MontaggioFoto is free software:  
you can redistribute it and/or modify it  
under the terms of the GNU Affero General Public License as published by  
the Free Software Foundation, either version 3 of the License, or  
(at your option) any later version.  

You should have received a copy of the GNU AGPL v.3  
along with Casasoft MontaggioFoto.  
If not, see <http://www.gnu.org/licenses/>.  

# DISCLAIMER
Casasoft CCDV Tools is distributed in the hope that it will be useful,  
but WITHOUT ANY WARRANTY; without even the implied warranty of  
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.   
See the GNU General Public License for more details.
