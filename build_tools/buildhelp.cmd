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

set repo=C:\projects\Contemporary_CDV\
set build=bin\build\net6.0\
set docs=%repo%docs\

set prg=MontaggioFoto
@call :CREATE

set prg=MontaggioDorsi
@call :CREATE

set prg=Scatola
@call :CREATE

set prg=Cartella
@call :CREATE

set prg=CreditCard
@call :CREATE

set prg=Cubetti
@call :CREATE

set prg=Flexagon
@call :CREATE

goto :END

:CREATE
@echo off
%repo%%build%%prg% --help >%docs%help_%prg%.txt
%repo%%build%%prg% --helpjson >%docs%helpjson_%prg%.txt
%repo%%build%%prg% --helpscript >%docs%helpscript_%prg%.txt
%repo%%build%%prg% --man >%docs%man_%prg%.md
pandoc %docs%man_%prg%.md -s -t man -o %docs%%prg%.1
@echo on
@exit /B

:END