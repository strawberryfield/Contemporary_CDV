### Casasoft Contemporary Carte de Visite Tools

# The photographic "carte de visite"

## The history

In 1854 the Parisian photographer André Adolphe Eugène Disderi, 
owner of a studio on Boulevard des Italiens since 1848, 
found the method to obtain eight different negatives on a single plate, 
probably inspired by stereoscopic photographs, 
taken with dual-lens cameras and already in use since 1850.

The camera designed and patented by Disdéri incorporated four lenses 
and a sliding plate holder frame, 
which allowed the positioning of the negative to be able to impress it 
several times. There were ten shots envisaged and described in the 1854 
patent, eight, in reality, those received intact on the uncut sheets.

In this way it was possible to impress, 
develop and print many small images at the same time and reduce 
the production costs of each single photograph.

A sort of serial production of images was thus conceived that brought 
notable changes in the world of photography. The dimensions of a 
visiting card were around 54 mm (2.125 in. = Inch = inch) at the base 
and 89 mm (3.5 in.) in height for vertical images, 
vice versa for horizontal ones.

The image was printed on thin, compact paper, usually albumin. 
It is possible to find ancient copies printed on salted paper, 
but they are very rare and precious.  
More recent was the use with collodion, aristotype or other procedures.

These prints were then mounted, on a rigid paper support. 
Card stock dimensions were approximately 64 mm (2.5 in.) by 100 mm (4 in.)

The difference between the printed paper and the support on which 
it was glued created a sort of passepartout 
where it was possible to insert the imprint of the stamp, in ink or dry, 
of the photographic studio, the place of shooting, 
the title of the image or other indications.   
The reverse of the card was also used to indicate these data, 
printed or handwritten. Usually the medals and prizes won were 
also indicated on the reverse.

The carte de visite have an enormous success involving a lot of people: 
men and women of even modest social extraction, are immortalized 
surrounded by an apparent wealth thanks to the furnishings 
present in Disderi's studios. 
In a short time, the fashion of collecting visiting cards
of personalities from politics, culture and current affairs 
became established: in Victorian environments, the portrait of the 
Queen Mother opens albums dedicated to celebrities, 
albums with more or less precious finishes, 
but all equally conceived to host the magical format in series.

## The contemporary carte de visite

The idea of ​​a photographic business card, on permanent physical support, 
remains revolutionary today, in the current era of images 
and fluid information.

The ambition of the "Contemporary Carte de Visite" contest 
is to re-propose their relevance in its function of communication 
and social testimony, in its role as a tiny instrument 
of artistic expression: pocket-sized, practical, economic; 
concrete and persistent sign of presence in an era of ephemeral figures.

The first world competition for Contemporary Carte de Visite, 
Gianluigi Parpani Prize “Il Mondo in Tasca”, Lodi, 2019 
revealed that there is still ample room for innovation and wonder 
in the photography of the future that knows how to reinvent the past.

This is confirmed by the reinterpretation of ancient processes, 
from oleotype, to bichromate gum, to salt, to cyanotype ... 
The contemporary artists who today creatively reinterpret 
the Carte de Visite take up the challenge of a tiny space 
to create something great and innovative.

### About these tools

Print services use standard formats, larger than those needed 
to produce a carte de visite.  
We will therefore have to combine the prints on a single support, 
exactly as it happened for Disderi's plates.

For my first series I had organized myself with templates 
for the DTP Scribus program, but besides the inconvenience of 
having to manually add pages and images, I forgot to insert references 
for the cut.

So I dusted off ImageMagick, but instead of driving it in javascript 
I decided to take advantage of the brand new Net 6.0 
and the Magick.NET library which already incorporates 
ImageMagick and therefore allows you to create ready-to-use executables.

A more long story is (in Italian) on the my site
[«The Strawberry Field»](https://strawberryfield.altervista.org/carte_de_visite/)


# The Contemporary Carte de Visite Tools common library

This library contains the kernel of the
[Casasoft Contemporary Carte de Visite Tools suite](https://github.com/strawberryfield/Contemporary_CDV)

## Classes areas

### ImageMagick related classes

- *Colors*  
Handles conversions from string with rgb values or mnemonic names to MagickColor class
- *Formats*  
Hosts formats for input and output images
- *Images*  
Predefined empty images
- *PaperFormats*  
Enum with predefined output formats
- *Utils*  
Static class with spare utilities

### Command line handling
These classes handle the command line interface for each tool using **Mono.Options**

- *ICommandLine*  
Interface for all command line related classes
- *CommandLine*  
Base command line class
- *BaseBuilderCommandLine*  
**Scatola** and **Cartella** command line handling
- *CreditCardCommandLine*  
**CreditCard** command line handling
- *CubettiCommandLine*  
**Cubetti** command line handling
- *FlexagonCommandLine*  
**Flexagon** command line handling
- *MontaggioDorsiCommandLine*  
**MontaggioDorsi** command line handling
- *MontaggioFotoCommandLine*  
**MontaggioFoto** command line handling

### Json handling
In order to save parameters from GUIs there are these classes that
can be serialized / deserialized in json

- *IParameters*  
Interface for all json serializable classes
- *CommonParameters*  
Class with the parameters common to all tools
- *BaseBuilderParameters*  
**Scatola** and **Cartella** parameters
- *CreditCardParameters*  
**CreditCard** parameters
- *CubettiParameters*  
**Cubetti** parameters
- *FlexagonParameters*  
**Flexagon** parameters
- *MontaggioDorsiParameters*  
**MontaggioDorsi** parameters
- *MontaggioFotoParameters*  
**MontaggioFoto** parameters

### Tools engines
These classes do the dirty work for each tool; **Scatola** and **Cartella**,
before a refactory, had their engines, named "builder" so there are "engines"
that are only wrappers for the pre-existing "builders"

- *IEngine*   
Interface for all engines
- *IBaseBuilderEngine*  
Extended interface for **Scatola** and **Cartella**
- *BaseEngine*  
Common engines class
- *BaseBuilderEngine*  
Common wrapper for **Scatola** and **Cartella** builders
- *CreditCardEngine*  
**CreditCard** core engine
- *CubettiEngine*  
**Cubetti** core engine
- *FlexagonEngine*  
**Flexagon** core engine
- *FolderEngine*  
**Cartella** builder wrapper
- *MontaggioDorsiEngine*  
**MontaggioDorsi** core engine
- *MontaggioFotoEngine*  
**MontaggioFoto** core engine
- *ScatolaEngine*  
**Scatola** builder wrapper

The old "builders":

- *IBuilder*  
Interface for **Scatola** and **Cartella** builders
- *BaseBuilder*  
Common builder for **Scatola** and **Cartella**
- *FolderBuilder*  
Builder for **Cartella**
- *ScatolaBuilder*  
Builder for **Scatola**

### Scripting
- *Compiler*  
This class compiles at run-time the c# scripts; 
there are other classes that wraps the scripts in a class
specific for any tool.  
These classes are listed below.

- *IScripting*  
Interface for script wrapping classes
- *BaseScripting*  
The base script wrapping
- *BaseBuilderScripting*  
The script wrapping for **Scatola** and **Cartella**
- *CreditCardScripting*  
The script wrapping for **CreditCard**
- *CubettiScripting*  
The script wrapping for **Cubetti**
- *FlexagonScripting*  
The script wrapping for **Flexagon**
- *MontaggioDorsiScripting*  
The script wrapping for **MontaggioDorsi**
- *MontaggioFotoScripting*  
The script wrapping for **MontaggioFoto**

A companion package offers 
[Test templates](https://www.nuget.org/packages/Casasoft.CCDV.Script.Templates/)
to help in writing and testing custom scripts

## Requirements

### .net

The library is compiled for .net 6.0

### Packages

The following packages are required:

- Microsoft.CodeAnalysis.Common Version >= 4.4.0
- Magick.NET-Q16-AnyCPU Version >= 12.2.2
- Magick.NET.SystemWindowsMedia >= 6.1.3
- Microsoft.CodeAnalysis.CSharp Version >= 4.4.0
- Mono.Options Version >= 6.12.0.148
- Casasoft.Xaml.Controls >= 22.4.13
