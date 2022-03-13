rem test for montaggiofoto scripting

set projectdir=C:\projects\Contemporary_CDV\
set bin=%projectdir%bin\build\net6.0\
set samples=%projectdir%Test\

%bin%MontaggioFoto --dpi=600 ^
 --script=@%samples%AddLabel.cs ^
 --fullsize ^
 --output=%samples%AddLabel ^
 %samples%cdv*.jpg 
