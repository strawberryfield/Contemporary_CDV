### Casasoft Contemporary Carte de Visite Tools

# Common library

This library contains the kernel of the
[Casasoft Contemporary Carte de Visite Tools suite](https://github.com/strawberryfield/Contemporary_CDV)
described (in Italian) on the site
[«The Strawberry Field»](https://strawberryfield.altervista.org/carte_de_visite/preparazione_immagini.php)

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

## Requirements

### .net

The library is compiled for .net 6.0

### Packages

The following packages are required:

- Microsoft.CodeAnalysis.Common Version=4.1.0
- Magick.NET-Q16-AnyCPU Version=11.0.0
- Microsoft.CodeAnalysis.CSharp Version=4.1.0
- Mono.Options Version=6.12.0.14
