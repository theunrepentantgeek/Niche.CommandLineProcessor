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
    $psakePath = resolve-path $path\psake*\tools\psake.psm1 -ErrorAction SilentlyContinue | Sort-Object $toNatural | select-object -last 1
    if ($psakePath -eq $null) {
        # Try NuGet 3.0 style
        $psakePath = resolve-path $path\psake\*\tools\psake.psm1 -ErrorAction SilentlyContinue | Sort-Object $toNatural | select-object -last 1
    }

    if ($psakePath -eq $null) {
        Write-Output "[!] Psake not found at $path"
    } else {
        import-module $psakePath
        Write-Output "[+] Loaded psake from $psakePath"
    }
}

# Try to load a local copy first (so we can ensure a particular version is used)
TryLoad-Psake .\lib\psake

if ((get-module psake) -eq $null) {
    # Don't have psake loaded, try to load it from PowerShell's default module location
    import-module psake -ErrorAction SilentlyContinue
}

if ((get-module psake) -eq $null) {
    # Not yet loaded, try to load it from the packages folder
    TryLoad-Psake ".\packages\"
}

if ((get-module psake) -eq $null) {
    # Still not loaded, let's try the various NuGet caches
    $locals = nuget locals all -list
    foreach($local in $locals)
    {
        $index = $local.IndexOf(":")
        $folder = $local.Substring($index + 2)
        TryLoad-Psake $folder
    }
}

$psake = get-module psake
if ($psake -ne $null) {
    $psakePath = $psake.Path
    Write-Output "[+] Psake loaded from $psakePath"
}
else {
    throw "Unable to load psake"
}

Write-Output ""

