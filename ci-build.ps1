
$here = Split-Path -parent $MyInvocation.MyCommand.Definition
$psakePath = Resolve-Path $here\packages\psake*
Import-Module $psakePath\tools\psake.psm1
invoke-psake $here/build.ps1 -Task CI.Build
