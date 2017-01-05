@echo off
if "%1" == "Release" (
dotnet pack --version-suffix "build%date:~6,4%%date:~3,2%%date:~0,2%%time:~0,2%%time:~3,2%%time:~6,2%" --output C:\LibraryPackages --no-build --configuration %1
robocopy bin\\Release C:\\LibraryPackages\\%2 /E /MIR
)