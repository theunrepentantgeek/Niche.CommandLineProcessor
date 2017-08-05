#
# PSake build script for Niche Commandline library
#

properties {
    $baseDir = resolve-path .\
    $buildDir = "$baseDir\build"
}

Task Integration.Build -Depends Debug.Build, Compile.Assembly, Unit.Tests

Task Formal.Build -Depends Release.Build, Compile.Assembly, Unit.Tests

Task CI.Build -Depends Debug.Build, Compile.Assembly, Unit.Tests


Task Release.Build {
    $script:buildType = "Release"
    Write-Host "Release build configured"
}

Task Debug.Build {
    $script:buildType = "Debug"
    Write-Host "Debug build configured"
}

Task Compile.Assembly -Depends Requires.BuildType, Requires.MSBuild, Requires.BuildDir {

    exec { 
        & $msbuildExe /p:Configuration=$buildType /verbosity:minimal /fileLogger /flp:verbosity=detailed`;logfile=$buildDir\Niche.CommandLine.txt .\Niche.CommandLine.sln 
    }
}  

Task Unit.Tests -Depends Requires.XUnitConsole, Compile.Assembly {

    exec {
        & $xunitExe .\build\Niche.CommandLine.Tests\Debug\Niche.CommandLine.Tests.dll
    }
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

    if ($xunitExe -eq $null)
    {
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

formatTaskName { 
	param($taskName) 
    
    $divider = "-" * 70

    return "`r`n" + $divider + "`r`n" + $taskName + "`r`n" + $divider
} 

