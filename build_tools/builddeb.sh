#!/bin/bash

# copyright (c) 2020-2023 Roberto Ceccarelli - Casasoft
# http://strawberryfield.altervista.org 
# 
# This file is part of Casasoft Contemporary Carte de Visite Tools
# https://github.com/strawberryfield/Contemporary_CDV
# 
# Casasoft CCDV Tools is free software: 
# you can redistribute it and/or modify it
# under the terms of the GNU Affero General Public License as published by
# the Free Software Foundation, either version 3 of the License, or
# (at your option) any later version.
# 
# Casasoft CCDV Tools is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
# See the GNU General Public License for more details.
# 
# You should have received a copy of the GNU AGPL v.3
# along with Casasoft CCDV Tools.  
# If not, see <http://www.gnu.org/licenses/>.

package=ccdv
version=$1
arch=amd64
origin=/mnt/c/projects/Contemporary_CDV

workdir=${package}_${version}_${arch}

cd ~
rm -r ${package}_*
mkdir $workdir
cd $workdir
mkdir -p usr/share/ccdv
cd usr/share/ccdv
cp $origin/bin/publish/* .
rm *.exe

prg=Cartella
sed -i s_${prg}_/usr/share/ccdv/${prg}_g ${prg}
prg=Scatola
sed -i s_${prg}_/usr/share/ccdv/${prg}_g ${prg}
prg=MontaggioDorsi
sed -i s_${prg}_/usr/share/ccdv/${prg}_g ${prg}
prg=MontaggioFoto
sed -i s_${prg}_/usr/share/ccdv/${prg}_g ${prg}
prg=CreditCard
sed -i s_${prg}_/usr/share/ccdv/${prg}_g ${prg}
prg=Cubetti
sed -i s_${prg}_/usr/share/ccdv/${prg}_g ${prg}
prg=Flexagon
sed -i s_${prg}_/usr/share/ccdv/${prg}_g ${prg}

mkdir runtimes
cp -r $origin/bin/publish/runtimes/linux-x64 runtimes

cd ~/$workdir
mkdir -p usr/bin
cd usr/bin
ln -s ../share/ccdv/Cartella .
ln -s ../share/ccdv/Scatola .
ln -s ../share/ccdv/MontaggioFoto .
ln -s ../share/ccdv/MontaggioDorsi .
ln -s ../share/ccdv/CreditCard .
ln -s ../share/ccdv/Cubetti .
ln -s ../share/ccdv/Flexagon .

cd ~/$workdir
mkdir -p usr/share/man/man1
cd usr/share/man/man1
cp $origin/docs/*.1 .

cd ~/$workdir
mkdir DEBIAN
stdbuf -o0 -i0 -e0 echo "Package: $package" >DEBIAN/control
stdbuf -o0 -i0 -e0 echo "Version: $version" >>DEBIAN/control
stdbuf -o0 -i0 -e0 echo "Architecture: $arch" >>DEBIAN/control
stdbuf -o0 -i0 -e0 echo "Section: contrib/graphics" >>DEBIAN/control
stdbuf -o0 -i0 -e0 echo "Maintainer: <Roberto Ceccarelli> strawberryfield@altervista.org" >>DEBIAN/control
stdbuf -o0 -i0 -e0 echo "Homepage: https://strawberryfield.altervista.org/carte_de_visite" >>DEBIAN/control
stdbuf -o0 -i0 -e0 echo "Description: Casasoft Contemporary Carte de Visite tools" >>DEBIAN/control

cd ~
dpkg-deb --build --root-owner-group ${workdir}
cp ${workdir}.deb $origin/bin/

arch=arm64
workarm=${package}_${version}_${arch}

mv ${workdir} ${workarm}
cd $workarm
cd usr/share/ccdv
rm -r runtimes/*
cp -r $origin/bin/publish/runtimes/linux-arm64 runtimes

cd ~/$workarm
stdbuf -o0 -i0 -e0 echo "Package: $package" >DEBIAN/control
stdbuf -o0 -i0 -e0 echo "Version: $version" >>DEBIAN/control
stdbuf -o0 -i0 -e0 echo "Architecture: $arch" >>DEBIAN/control
stdbuf -o0 -i0 -e0 echo "Section: contrib/graphics" >>DEBIAN/control
stdbuf -o0 -i0 -e0 echo "Maintainer: <Roberto Ceccarelli> strawberryfield@altervista.org" >>DEBIAN/control
stdbuf -o0 -i0 -e0 echo "Homepage: https://strawberryfield.altervista.org/carte_de_visite" >>DEBIAN/control
stdbuf -o0 -i0 -e0 echo "Description: Casasoft Contemporary Carte de Visite tools" >>DEBIAN/control

cd ~
dpkg-deb --build --root-owner-group ${workarm}
cp ${workarm}.deb $origin/bin/
