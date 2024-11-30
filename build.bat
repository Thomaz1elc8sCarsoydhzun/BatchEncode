@echo off
echo Building BatchEncode project...

:: Restore NuGet packages
dotnet restore

:: Build the project in Release configuration
dotnet build -c Release

:: Publish the project to the bin/Release folder
dotnet publish -c Release -o ./bin/Release/net8.0

echo Build completed successfully.
pause
