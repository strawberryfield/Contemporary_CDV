### Casasoft Contemporary Carte de Visite Tools

# Build scripts

Tools for building help and binaries

These scripts generate output for Windows and Linux, but are designed to run under Windows 10 with Ubuntu 20.04 in wsl2.

Other required tools are:

- [Wix Installer](https://wixtoolset.org/)
- [WinRar](https://www.win-rar.com/)
- [Pandoc](https://pandoc.org/)

## buildhelp

Extracts command line syntax, json and script templates, man page in markdown format and unix man page

## publish

Publish all the projects in framework-dependent mode: you need .NET 6 runtime installed to run the programs

Five packages are generated:

- **Casasoft_CCdV_*yy.mm.dd*.rar**  
Unpack and run binaries in rar format
- **Casasoft_CCDV_*yy.mm.dd*.tar.bz2**  
Unpack and run binaries in tar+bzip2 format
- **Casasoft_CCdV_wGUI_*yy.mm.dd*.rar**  
Unpack and run binaries in rar format with GUI (only for Windows)
- **Casasoft_CCdV_*yy.mm.dd*.msi**  
Windows installer package
- **ccdv_*yy.mm.dd*_amd64.deb**  
Debian/Ubuntu installer package



