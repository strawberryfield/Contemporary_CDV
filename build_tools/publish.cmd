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
set nuget=C:\Users\rober\.nuget\packages\
set winrar="C:\Program Files\WinRAR\winrar.exe"

set version=23.08.22
 
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
set prj=Cubetti
@dotnet publish -c Release -o %build% -p:version="%version%" --no-self-contained %repo%%prj%\%prj%.csproj
set prj=Flexagon
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
%winrar% k ..\%pkgname%_%version%.rar 
popd
bash ./pack.sh %version%
bash ./builddeb.sh %version%

@echo on
set prj=CCDV
@echo off
dotnet publish -c Release -o %build% -p:version="%version%" --no-self-contained %repo%%prj%\%prj%.csproj

pushd .
cd %build%
rmdir /s /q runtimes\linux-arm64
rmdir /s /q runtimes\linux-x64
rmdir /s /q runtimes\linux-musl-x64
rmdir /s /q runtimes\osx-x64
rmdir /s /q man
%winrar% a -r -m5 -s ..\%pkgname%_wGUI_%version%.rar *.*
%winrar% k ..\%pkgname%_wGUI_%version%.rar
popd

wix build ^
 -ext %nuget%wixtoolset.ui.wixext\4.0.1\wixext4\WixToolset.UI.wixext.dll ^
 -ext %nuget%wixtoolset.util.wixext\4.0.1\wixext4\WixToolset.Util.wixext.dll ^
 -d var.ProjectDir=%repo%WindowsInstaller\ ^
 -outputtype exe ^
 -o %bin%%pkgname%-%version%.msi ^
 %repo%WindowsInstaller\product.wxs
 
dotnet pack %repo%Templates\ScriptTestTemplatesPack.csproj -p:PackageVersion=%version% 
dotnet pack %repo%ProjectTemplate\CCDV_ProjectTemplate.csproj -p:PackageVersion=%version% 