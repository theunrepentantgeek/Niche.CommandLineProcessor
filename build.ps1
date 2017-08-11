#
# PSake build script for Niche Commandline library
#

properties {
    $baseDir = resolve-path .\
    $buildDir = "$baseDir\build"
    $srcDir = resolve-path $baseDir\src
}

Task Integration.Build -Depends Generate.VersionInfo, Debug.Build, Compile.Assembly, Unit.Tests

Task Formal.Build -Depends Release.Build, Generate.Version, Compile.Assembly, Compile.NuGet, Unit.Tests

Task CI.Build -Depends Debug.Build, Generate.Version, Compile.Assembly, Unit.Tests


Task Compile.Assembly -Depends Requires.BuildType, Requires.MSBuild, Requires.BuildDir, Generate.VersionInfo {

    exec { 
        & $msbuildExe /p:Configuration=$buildType /verbosity:minimal /fileLogger /flp:verbosity=detailed`;logfile=$buildDir\Niche.CommandLine.txt .\Niche.CommandLine.sln 
    }
}  

Task Compile.NuGet -Depends Requires.NuGet, Requires.BuildType, Compile.Assembly {

    exec {
        & $nugetExe pack $srcDir\Niche.CommandLine\Niche.CommandLine.nuspec -version 2.1.0.0 -outputdirectory $buildDir -basePath $buildDir\Niche.CommandLine\$buildType
    }
}

Task Unit.Tests -Depends Requires.XUnitConsole, Compile.Assembly {

    exec {
        & $xunitExe .\build\Niche.CommandLine.Tests\Debug\Niche.CommandLine.Tests.dll
    }
}

Task Coverage.Tests -Depends Requires.OpenCover, Compile.Assembly {

}

## ----------------------------------------------------------------------------------------------------
##   Specifiers 
## ----------------------------------------------------------------------------------------------------
## Tasks used to configure the way the build will execute

Task Release.Build {
    $script:buildType = "Release"
    Write-Host "Release build configured"
}

Task Debug.Build {
    $script:buildType = "Debug"
    Write-Host "Debug build configured"
}



## ----------------------------------------------------------------------------------------------------
##   Generate 
## ----------------------------------------------------------------------------------------------------
## Tasks for creating source files and other useful things

# Generate the version number to use for this build
Task Generate.Version {

    $script:version = get-content $baseDir\version.txt -ErrorAction SilentlyContinue
    if ($version -eq $null) {
        throw "Unable to load .\version.txt"
    }
    Write-Host "Version          $script:version"

    $versionLastUpdated = git rev-list -1 HEAD $baseDir\version.txt
    $script:patchVersion = git rev-list "$versionLastUpdated..HEAD" --count
    Write-Host "Patch            $patchVersion"

    $branch = git name-rev --name-only HEAD
    Write-Host "Current Branch   $branch"

    $commit = git rev-parse --short head
    Write-Host "Current Commit   $commit"


    if ($branch -eq "master") {
        $script:semanticVersion = "$version.$patchVersion"
    }
    elseif ($branch -eq "develop") {
        $script:semanticVersion = "$version.$patchVersion-beta.$commit"
    }
    else {
        $semanticBranch = $branch -replace "[^A-Za-z0-9-]+", "."
        $script:semanticVersion = "$version.$patchVersion-alpha.$semanticBranch.$commit"
    }

    Write-Host "Semantic version $semanticVersion"
}

# Generate a VersionInfo.cs file for this build
Task Generate.VersionInfo -Depends Generate.Version {

    foreach($assemblyInfo in (get-childitem $srcDir\AssemblyInfo.cs -recurse)) {
        $versionInfo = Join-Path $assemblyInfo.Directory "VersionInfo.cs"
        set-content $versionInfo "// Generated file - do not modify",
            "using System.Reflection;",
            "[assembly: AssemblyVersion(`"$version`")]",
            "[assembly: AssemblyFileVersion(`"$version.$patchVersion`")]",
            "[assembly: AssemblyInformationalVersion(`"$semanticVersion`")]"
        Write-Host "Generated $versionInfo"
    }
}

## ----------------------------------------------------------------------------------------------------
##   Configure 
## ----------------------------------------------------------------------------------------------------
## Tasks for finding or creating folders that are needed

Task Configure.xUnitResultFolder {

    $script:xUnitResultFolder = join-path $buildDir xUnit.results
    if (test-path $xUnitResultFolder) {
        Write-Host "Found XUnit results folder: $xUnitResultFolder"
    }
    else {
        mkdir $xUnitResultFolder -erroraction silentlycontinue | Out-Null
        Write-Host "Created XUnit results folder: $xUnitResultFolder"
    }
}


## ----------------------------------------------------------------------------------------------------
##   Requires 
## ----------------------------------------------------------------------------------------------------
## Tasks for finding required resources that must already be available.
## This includes executables and other tools

Task Requires.MSBuild {

    $script:msbuildExe = 
        resolve-path "C:\Program Files (x86)\Microsoft Visual Studio\*\*\MSBuild\*\Bin\MSBuild.exe"

    if ($msbuildExe -eq $null)
    {
        throw "Failed to find MSBuild"
    }

    Write-Host "Found MSBuild here: $msbuildExe"
}

Task Requires.BuildDir {
    if (!(test-path $buildDir))
    {
        Write-Host "Creating build folder $buildDir"
        mkdir $buildDir > $null
    }
    else {
        Write-Host "Build folder is: $buildDir"
    }
}

Task Requires.XUnitConsole {

    $script:xunitExe =
        resolve-path ".\packages\xunit.runner.console.*\tools\xunit.console.exe"

    if ($xunitExe -eq $null) {
        throw "Failed to find XUnit.Console.exe"
    }

    Write-Host "Found XUnit.Console here: $xunitExe"
}

Task Requires.BuildType {
    
    if ($buildType -eq $null) {
        
        throw "No build type specified"
    }

    Write-Host "$buildType build confirmed"
}

Task Requires.NuGet { 

    $script:nugetExe =
        resolve-path ".\packages\NuGet.CommandLine.*\tools\nuget.exe"

    if ($nugetExe -eq $null)
    {
        throw "Failed to find nuget.exe"
    }

    Write-Host "Found Nuget here: $nugetExe"
}

Task Requires.OpenCover { 

    $script:opencoverExe =
        resolve-path ".\packages\OpenCover.*\tools\OpenCover.Console.exe"

    if ($opencoverExe -eq $null)
    {
        throw "Failed to find opencover.console.exe"
    }

    Write-Host "Found OpenCover here: $opencoverExe"
}

## ----------------------------------------------------------------------------------------------------
##   Utility Methods
## ----------------------------------------------------------------------------------------------------

formatTaskName { 
	param($taskName) 
    
    $divider = "-" * 70

    return "`r`n" + $divider + "`r`n" + $taskName + "`r`n" + $divider
} 

