@echo off
rem copyright (c) 2020-2022 Roberto Ceccarelli - Casasoft
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
set wix=C:\Program Files (x86)\WiX Toolset v3.11\bin\
set candle="%wix%candle.exe"
set light="%wix%light.exe"
set WixUtils="%wix%WixUtilExtension.dll"

set version=22.03.20
 
@del /S /Q %build%
@del /Q %bin%%pkgname%*.*
set prj=Cartella
@dotnet publish -c Release -o %build% -p:version="%version%" --no-self-contained %repo%%prj%\%prj%.csproj
set prj=CreditCard
@dotnet publish -c Release -o %build% -p:version="%version%" --no-self-contained %repo%%prj%\%prj%.csproj
set prj=MontaggioDorsi
@dotnet publish -c Release -o %build% -p:version="%version%" --no-self-contained %repo%%prj%\%prj%.csproj
set prj=MontaggioFoto
@dotnet publish -c Release -o %build% -p:version="%version%" --no-self-contained %repo%%prj%\%prj%.csproj
set prj=Scatola
@dotnet publish -c Release -o %build% -p:version="%version%" --no-self-contained %repo%%prj%\%prj%.csproj

@echo off
pushd .
cd %build%
mkdir man
copy %docs%*.1 man
copy %repo%LICENSE .
copy %repo%README.md .
copy %repo%Linux.md .
%winrar% a -r -m5 -s ..\%pkgname%_%version%.rar *.*
popd
bash ./pack.sh %version%
bash ./builddeb.sh %version%

@echo on
set prj=CCDV
@echo off
dotnet publish -c Release -o %build% -p:version="%version%" --no-self-contained %repo%%prj%\%prj%.csproj

pushd .
cd %build%
%winrar% a -r -m5 -s ..\%pkgname%_wGUI_%version%.rar *.*
popd

%candle% %repo%WindowsInstaller\Product.wxs ^
 -dProjectDir=%repo%WindowsInstaller\ ^
 -out %bin%build\product.wixobj ^
 -ext WixUIExtension -ext %WixUtils%
%light% -out %bin%%pkgname%_%version%.msi ^
 %bin%build\product.wixobj ^
 -ext WixUIExtension -ext %WixUtils%