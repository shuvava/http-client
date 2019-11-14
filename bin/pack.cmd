@echo OFF
SET root=%~dp0..\src
SET version=%1
SET apikey=%2
IF "%1"=="" (
    SET version=2.0.0.0
)
IF "%2"=="" (
    echo nuget api key is not provided
    GOTO :EOF
)
echo cleanup
if exist %~dp0..\out (
    rd %~dp0..\out /S /Q
)
mkdir %~dp0..\out

echo run build
dotnet build %~dp0\..\http-client.sln --configuration Release  -p:Version=%version%

echo run pack command
for /f %%f in ('dir /ad /b %root%') do (
    if exist %root%\%%f\.nuspec (
        echo %root%\%%f
        nuget.exe pack %root%\%%f\.nuspec -NonInteractive -OutputDirectory %~dp0..\out -Properties Configuration=Release
    )
)
echo publishing
FOR %%i IN (%~dp0..\out\*.nupkg) do (
    nuget push %%i %apikey% -Source https://api.nuget.org/v3/index.json
)
