To run the programs in Linux you need to set FONTCONFIG_PATH with

~~~
export FONTCONFIG_PATH=/etc/fonts
~~~

Latest version of the bash scripts do that if FONTCONFIG_PATH is not set

If you get a lot of warning and errors from **fontconfig**
you can try this solution by [Dirk Lemstra ](https://github.com/dlemstra/Magick.NET/issues/598)

~~~
sudo apt update
sudo apt full-upgrade
sudo apt-get install -y cabextract wget xfonts-utils 
curl -s -o ttf-mscorefonts-installer_3.7_all.deb http://ftp.us.debian.org/debian/pool/contrib/m/msttcorefonts/ttf-mscorefonts-installer_3.7_all.deb 
sudo sh -c "echo ttf-mscorefonts-installer msttcorefonts/accepted-mscorefonts-eula select true | debconf-set-selections" 
sudo dpkg -i ttf-mscorefonts-installer_3.7_all.deb
~~~
