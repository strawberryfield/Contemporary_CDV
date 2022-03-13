rem test for montaggiodorsi scripting

set projectdir=C:\projects\Contemporary_CDV\
set bin=%projectdir%bin\build\net6.0\
set samples=%projectdir%Test\

%bin%MontaggioDorsi ^
 --script=@%samples%Numbers.cs ^
 --tag=1 ^
 --output=%samples%Numbers ^
 %samples%dorso_o.jpg 
