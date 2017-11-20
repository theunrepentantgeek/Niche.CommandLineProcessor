dotnet restore
. .\scripts\bootstrap.ps1
invoke-psake ./build.ps1 -Task Formal.Build
exit (!$psake.build_success)
