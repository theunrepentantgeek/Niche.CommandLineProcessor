## ==================================================================================================== 
##   PSake build script for Niche Commandline library
## ==================================================================================================== 

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

Task CI.Build -Depends Debug.Build, Generate.Version, Compile.Assembly, Coverage.Report

## ----------------------------------------------------------------------------------------------------
##   Core Tasks 
## ----------------------------------------------------------------------------------------------------
## The key build tasks themselves (see below for supporting tasks, listed in order of execution)

Task Compile.Assembly -Depends Requires.BuildType, Requires.MSBuild, Requires.BuildDir {

    exec { 
        & $msbuildExe /p:Configuration=$buildType /verbosity:minimal /fileLogger /flp:verbosity=detailed`;logfile=$buildDir\Niche.CommandLine.msbuild.log .\Niche.CommandLine.sln /p:Version=$semver20
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

Task Unit.Tests -Depends Requires.dotNet, Configure.TestResultsFolder, Compile.Assembly {

    $testProjects = Get-ChildItem -Path $srcDir\*.Tests\*.Tests.csproj
    foreach($testProject in $testProjects) 
    {
        Write-Header $testProject.Name

        $reportFile = [System.IO.Path]::ChangeExtension($testProject.Name, ".xunit.xml")
        $reportPath = join-path $testResultsFolder $reportFile

        Write-Host "Test report: $reportPath"

        pushd $testProject.Directory.FullName
        exec {
            & $dotnetExe xunit -nobuild -configuration $buildType -xml $reportPath
        }
        popd 
    }    
}

Task Coverage.Tests -Depends Requires.OpenCover, Requires.dotNet, Configure.TestResultsFolder, Compile.Assembly {

    $filter = "+[*]* -[xunit.*]* -[Fluent*]* -[*.Tests]*"
    $logLevel = "info"

    foreach ($project in (resolve-path $srcDir\*.Tests\*.csproj)){
    
        $projectName = split-path $project -Leaf
        $projectFolder = split-path $project

        Write-Host "Testing $projectName"

        pushd $projectFolder
        exec {
            & $openCoverExe -oldStyle "-target:$dotnetExe" "-targetargs:xunit" -register:user "-filter:$filter" -log:$loglevel -output:$testResultsFolder\$projectName.cover.xml
        }
        popd
    }
}

Task Coverage.Report -Depends Requires.ReportGenerator, Configure.OpenCoverReportFolder, Coverage.Tests {

    exec {
        & $reportGeneratorExe -reports:$testResultsFolder\*.cover.xml -targetdir:$openCoverReportFolder -historydir:$baseDir\history\coverage
    }

    $openCoverIndex = resolve-path $openCoverReportFolder\index.htm
    & $openCoverIndex
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

Task Configure.TestResultsFolder -Depends Requires.BuildDir {

    $script:testResultsFolder = join-path $buildDir testing.results
    Write-Host "Test results folder: $testResultsFolder"

    if (test-path $testResultsFolder) {
        remove-item $testResultsFolder -recurse -force -erroraction silentlycontinue    
    }

    mkdir $testResultsFolder | Out-Null    
}

Task Configure.OpenCoverReportFolder -Depends Requires.BuildDir {

    $script:OpenCoverReportFolder = join-path $buildDir opencover.report
    Write-Host "OpenCover report folder: $OpenCoverReportFolder"

    if (test-path $OpenCoverReportFolder) {
        remove-item $OpenCoverReportFolder -recurse -force -erroraction silentlycontinue    
    }

    mkdir $OpenCoverReportFolder | Out-Null    
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

Task Requires.BuildType {
    
    if ($buildType -eq $null) {
        
        throw "No build type specified"
    }

    Write-Host "$buildType build confirmed"
}

Task Requires.DotNet {
    $script:dotnetExe = (get-command dotnet).Source

    if ($dotnetExe -eq $null) {
        
        throw "Failed to find dotnet.exe"
    }

    Write-Host "Found dotnet here: $dotnetExe"
}

Task Requires.MSBuild {
    
    $script:msbuildExe = 
        resolve-path "C:\Program Files (x86)\Microsoft Visual Studio\*\*\MSBuild\*\Bin\MSBuild.exe"
    
    if ($msbuildExe -eq $null)
    {
        throw "Failed to find MSBuild"
    }
    
    Write-Host "Found MSBuild here: $msbuildExe"
}
    
Task Requires.NuGet { 

    $script:nugetExe = (get-command nuget).Source -ErrorAction SilentlyContinue

    if ($nugetExe -eq $null) {
        resolve-path ".\packages\NuGet.CommandLine.*\tools\nuget.exe" -ErrorAction SilentlyContinue
    }

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

Task Requires.ReportGenerator {

    $toNatural = { [regex]::Replace($_, '\d+', { $args[0].Value.PadLeft(20) }) }
    $script:reportGeneratorExe =
        resolve-path $env:userprofile\.nuget\packages\reportgenerator\*\tools\ReportGenerator.exe | sort-object $toNatural | select-object -last 1

    if ($reportGeneratorExe -eq $null)
    {
        throw "Failed to find ReportGenerator.exe"
    }

    Write-Host "Found Report Generator here: $reportGeneratorExe"
}

Task Requires.XUnitConsole {
    
        $script:xunitExe =
            resolve-path ".\packages\xunit.runner.console.*\tools\xunit.console.exe"
    
        if ($xunitExe -eq $null) {
            throw "Failed to find XUnit.Console.exe"
        }
    
        Write-Host "Found XUnit.Console here: $xunitExe"
    }
    
## ----------------------------------------------------------------------------------------------------
##   Utility Methods
## ----------------------------------------------------------------------------------------------------

formatTaskName { 
	param($taskName) 
    
    $width = 70
    $hostWidth = (get-host).UI.RawUI.WindowSize.Width
    if ($hostWidth -ne $null) {
        $width = $hostWidth - 2
    }

    $divider = "-" * $width
    
    $now = get-date
    $nowString = $now.ToString("HH:mm:ss").PadLeft($width - $taskName.Length - 5)

    if ($lastTaskStart -ne $null) {
        $duration = Format-Duration($now - $lastTaskStart)
        $duration = $duration.PadLeft($width - 2)
    }

    $script:lastTaskStart = $now

    return "$duration`r`n$divider`r`n  $taskName $nowString`r`n$divider`r`n"
} 

function Write-Header($message) {
    $divider = "-" * ($message.Length + 4)
    Write-Host "`r`n  $message`r`n$divider`r`n"
}

function Format-Duration($duration) {
    if ($duration.TotalMinutes -ge 60) {
        return "{0}h {1}m" -f $duration.Hours, $duration.Minutes
    }
    
    if ($duration.TotalSeconds -ge 60) {
        return "{0}m {1}s" -f $duration.Minutes, $duration.Seconds
    }
    
    return "{0:N3}s" -f $duration.TotalSeconds
}
