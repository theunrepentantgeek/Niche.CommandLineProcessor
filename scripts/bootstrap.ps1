## ------------------------------------------------------------
##   Ensure Psake is loaded as a module so we can run a build
## ------------------------------------------------------------
## If psake is not already available, it probes several likely locations to try and find it
## 

function TryLoad-Psake($path) {
    $psakeModule = get-module psake
    if ($psakeModule -ne $null) {
        # Already loaded, don't do anything
        return
    }

	$toNatural = { [regex]::Replace($_, '\d+', { $args[0].Value.PadLeft(20) }) }

    # Resolve the path we were given; in the case of multiple matches, take the "latest" one
    $psakePath = resolve-path $path\psake.psm1 -ErrorAction SilentlyContinue | Sort-Object $toNatural | select-object -last 1

    if ($psakePath -eq $null) {
        Write-Output "[!] Psake not found at $path"
    } else {
        import-module $psakePath
        Write-Output "[+] Loaded psake from $psakePath"
    }
}

$psake = get-module psake
if ($psake -ne $null) {
    $psakePath = $psake.Path
    Write-Output "[+] Psake already loaded from $psakePath"
}

# Try to load a local copy first (so we can ensure a particular version is used)
TryLoad-Psake .\lib\psake

if ($psakeModule -eq $null) {
    # Don't have psake loaded, try to load it from PowerShell's default module location
    import-module psake -ErrorAction SilentlyContinue
}

if ($env:USERPROFILE -ne $null) {
    # Still don't  have psake loaded, try to load it from the Nuget cache in the users profile folder on Windows
    TryLoad-Psake "$env:USERPROFILE\.nuget\psake\*"
}

if ($env:HOME -ne $null) {
    # Still don't  have psake loaded, try to load it from the users profile folder on Linux
    TryLoad-Psake "$env:HOME\.nuget\packages\psake\*\tools"
}

if ($env:NugetMachineInstallRoot-ne $null) {
    # Still don't  have psake loaded, try to load it from the NuGet machine cache on Windows
    TryLoad-Psake "$env:NugetMachineInstallRoot\.nuget\psake\*\psake.psm1"
}

$psakeModule = get-module psake
if ($psakeModule -eq $null) {
    throw "Unable to load psake"
}

Write-Output ""

