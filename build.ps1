#
# PSake build script for Niche Commandline library
#

properties {
    $baseDir = resolve-path .\
    $buildDir = "$baseDir\build"
    $srcDir = resolve-path $baseDir\src
}

## ----------------------------------------------------------------------------------------------------
##   Targets 
## ----------------------------------------------------------------------------------------------------
## Top level targets used to run builds

Task Integration.Build -Depends Generate.VersionInfo, Debug.Build, Compile.Assembly, Unit.Tests

Task Formal.Build -Depends Release.Build, Generate.Version, Compile.Assembly, Unit.Tests, Compile.NuGet

Task CI.Build -Depends Debug.Build, Generate.Version, Compile.Assembly, Unit.Tests, Compile.NuGet

## ----------------------------------------------------------------------------------------------------
##   Core Tasks 
## ----------------------------------------------------------------------------------------------------
## The key build tasks themselves (see below for supporting tasks)

Task Compile.Assembly -Depends Requires.BuildType, Requires.MSBuild, Requires.BuildDir, Generate.VersionInfo {

    exec { 
        & $msbuildExe /p:Configuration=$buildType /verbosity:minimal /fileLogger /flp:verbosity=detailed`;logfile=$buildDir\Niche.CommandLine.txt .\Niche.CommandLine.sln 
    }
}  

Task Compile.NuGet -Depends Requires.NuGet, Requires.BuildType, Requires.BuildDir, Compile.Assembly, Configure.PackagesFolder {

    $nugetFolder = join-path $packagesFolder Niche.CommandLine
    mkdir $nugetFolder | Out-Null

    $csprojFile = resolve-path .\src\Niche.CommandLine\Niche.CommandLine.csproj

    exec {
        & $nugetExe pack $csprojFile -version $semver10 -outputdirectory $packagesFolder -basePath $buildDir -properties Configuration=$buildType
    }
}

Task Unit.Tests -Depends Requires.XUnitConsole, Compile.Assembly {

    exec {
        & $xunitExe .\build\Niche.CommandLine.Tests\$buildType\Niche.CommandLine.Tests.dll
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
        $script:semver10 = "$version.$patchVersion"
        $script:semver20 = "$version.$patchVersion"
    }
    elseif ($branch -eq "develop") {
        $script:semver10 = "$version.$patchVersion-beta"
        $script:semver20 = "$version.$patchVersion-beta.$commit"
    }
    else {
        $semverBranch = $branch -replace "[^A-Za-z0-9-]+", "."
        $script:semver10 = "$version.$patchVersion-alpha"
        $script:semver20 = "$version.$patchVersion-alpha.$semverBranch.$commit"
    }

    Write-Host "Semver 1.0:      $semver10"
    Write-Host "Semver 2.0:      $semver20"
}

# Generate a VersionInfo.cs file for this build
Task Generate.VersionInfo -Depends Generate.Version {

    foreach($assemblyInfo in (get-childitem $srcDir\AssemblyInfo.cs -recurse)) {
        $versionInfo = Join-Path $assemblyInfo.Directory "VersionInfo.cs"
        set-content $versionInfo -encoding UTF8 `
            "// Generated file - do not modify",
            "using System.Reflection;",
            "[assembly: AssemblyVersion(`"$version`")]",
            "[assembly: AssemblyFileVersion(`"$version.$patchVersion`")]",
            "[assembly: AssemblyInformationalVersion(`"$semver20`")]"
        Write-Host "Generated $versionInfo"
    }
}

## ----------------------------------------------------------------------------------------------------
##   Configure 
## ----------------------------------------------------------------------------------------------------
## Tasks for finding or creating folders that are needed

Task Configure.xUnitResultFolder -Depends Requires.BuildDir {

    $script:xUnitResultFolder = join-path $buildDir xUnit.results
    Write-Host "XUnit results folder: $xUnitResultFolder"

    if (test-path $xUnitResultFolder) {
        remove-item $xUnitResultFolder -recurse -force -erroraction silentlycontinue    
    }

    mkdir $xUnitResultFolder | Out-Null    
}

Task Configure.PackagesFolder -Depends Requires.BuildDir {

    $script:packagesFolder = join-path $buildDir packages
    Write-Host "Packaging folder: $packagesFolder"
    if (test-path $packagesFolder) {
        remove-item $packagesFolder -recurse -force -erroraction silentlycontinue    
    }

    mkdir $packagesFolder | Out-Null    
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
    if (test-path $buildDir)
    {
        Write-Host "Build folder is: $buildDir"
    }
    else {
        Write-Host "Creating build folder $buildDir"
        mkdir $buildDir | out-null
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

