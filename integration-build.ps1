dotnet restore -v diag
. .\scripts\bootstrap.ps1
invoke-psake ./build.ps1 -Task Integration.Build
