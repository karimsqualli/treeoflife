echo off
cls

set verbose=1
if %verbose%==1 (
    echo verbose is on
    pause
) else (
    echo verbose is off
)

set Compilo="C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"
if not exist %Compilo% (
	set Compilo="C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"
)
if not exist %Compilo% (
	echo Compilo not found
    pause
	exit
)

echo Compilo = %Compilo%
pause

if %verbose%==1 (
	echo run command: %Compilo% "TreeOfLife.sln" /t:Rebuild /p:Configuration=Release /p:Platform="any cpu"
	pause
)

%Compilo% "TreeOfLife.sln" /t:Rebuild /p:Configuration=Release /p:Platform="any cpu"

if %verbose%==1 (
	pause
	cls
	echo run command: %Compilo% "TreeOfLife.sln" /t:Rebuild /p:Configuration=ReleaseUser /p:Platform="any cpu"
	pause
)

%Compilo% "TreeOfLife.sln" /t:Rebuild /p:Configuration=ReleaseUser /p:Platform="any cpu"

if %verbose%==1 (
	pause
	cls
	echo run command : copy "..\BinAdmin\TreeOfLife.exe" ..\TreeOfLifeExt.exe
)
copy "..\BinAdmin\TreeOfLife.exe" ..\TreeOfLifeExt.exe

if %verbose%==1 (
	echo run command : copy "..\BinUser\TreeOfLife.exe" ..\TreeOfLife.exe
)

copy "..\BinUser\TreeOfLife.exe" ..\TreeOfLife.exe

if %verbose%==1 pause
