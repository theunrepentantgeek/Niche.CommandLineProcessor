
dotnet restore
. .\scripts\bootstrap.ps1
invoke-psake ./build.ps1 -Task Integration.Build
