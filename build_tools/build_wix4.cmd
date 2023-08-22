@echo off
rem copyright (c) 2020-2023 Roberto Ceccarelli - Casasoft
rem http://strawberryfield.altervista.org 
rem 
rem This file is part of Casasoft Contemporary Carte de Visite Tools
rem https://github.com/strawberryfield/Contemporary_CDV
rem 
rem Casasoft CCDV Tools is free software: 
rem you can redistribute it and/or modify it
rem under the terms of the GNU Affero General Public License as published by
rem the Free Software Foundation, either version 3 of the License, or
rem (at your option) any later version.
rem 
rem Casasoft CCDV Tools is distributed in the hope that it will be useful,
rem but WITHOUT ANY WARRANTY; without even the implied warranty of
rem MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
rem See the GNU General Public License for more details.
rem 
rem You should have received a copy of the GNU AGPL v.3
rem along with Casasoft CCDV Tools.  
rem If not, see <http://www.gnu.org/licenses/>.
@echo on

set name=Contemporary_CDV
set repo=C:\projects\%name%\
set docs=%repo%docs\
set bin=%repo%bin\
set build=%bin%publish\
set pkgname=Casasoft_CCdV
set winrar="C:\Program Files\WinRAR\winrar.exe"
set nuget=C:\Users\rober\.nuget\packages\
set version=23.03.12
 

wix build ^
 -ext %nuget%wixtoolset.ui.wixext\4.0.1\wixext4\WixToolset.UI.wixext.dll ^
 -ext %nuget%wixtoolset.util.wixext\4.0.1\wixext4\WixToolset.Util.wixext.dll ^
 -d var.ProjectDir=%repo%WindowsInstaller\ ^
 -outputtype exe ^
 -o %bin%%pkgname%-%version%.msi ^
 %repo%WindowsInstaller\product.wxs
rem dotnet build -c=Release -a=x64 -o=%bin% ^
rem  %repo%InstallerV4\InstallerV4.wixproj
 
