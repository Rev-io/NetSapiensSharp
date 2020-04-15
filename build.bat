@echo Off
set config=%1
if "%config%" == "" (
   set config=Release
)
 
set nuget=
if "%nuget%" == "" (
	set nuget=tools\nuget
)

%nuget% restore NetSapiensSharp.sln

%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild NetSapiensSharp.sln /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=diag /nr:false

mkdir Build
mkdir Build\lib
mkdir Build\lib\net40

%nuget% pack "src\NetSapiensSharp\NetSapiensSharp.nuspec" -NoPackageAnalysis -Verbosity detailed -OutputDirectory Build -Properties Configuration="%config%"