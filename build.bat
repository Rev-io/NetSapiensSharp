@echo Off
set config=%1
if "%config%" == "" (
   set config=Release
)
 
set nuget=
if "%nuget%" == "" (
	set nuget=tools\nuget
)

mkdir Build
mkdir Build\lib
mkdir Build\lib\net40

%nuget% pack "src\NetSapiensSharp\NetSapiensSharp.nuspec" -NoPackageAnalysis -Verbosity detailed -OutputDirectory Build -Properties Configuration="%config%"