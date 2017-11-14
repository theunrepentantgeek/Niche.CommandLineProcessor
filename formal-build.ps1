
dotnet restore
. .\scripts\bootstrap.ps1
invoke-psake ./build.ps1 -Task Formal.Build
if ($psake.build_success -eq $false) {
    return -1
}