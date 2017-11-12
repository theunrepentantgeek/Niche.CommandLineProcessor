
Niche.CommandLine Quick Reference
=================================
A convention based argument handler for writing console applications.

Program.cs
----------
Eliminate most of the boilerplate in Program.cs:

``` csharp
static int Main(string[] args)
{
    var processor = new CommandLineProcessor(args);
    var exitCode = processor.Parse<ProgramOptions>()
        .Execute(Run);

    processor.WhenHelpRequired(ShowHelp)
        .WhenErrors(ShowErrors);

    return exitCode;
}
```
For a complete working demo, see https://github.com/theunrepentantgeek/Niche.CommandLineProcessor/blob/master/src/Niche.CommandLine.Demo/Program.cs

Switches
--------
A switch is defined by a method with no return, no parameters, and a [Description] attribute for documentation.

``` csharp
// This switch can be used as `--help`, `-h` or `/h`
[Description("Show help listing all available options")]
public void Help();
```
See https://github.com/theunrepentantgeek/Niche.CommandLineProcessor/wiki/Conventions#switches for more.

Parameters
----------
A parameter is defined by a method with no return, one parameter, and a [Description] attribute for documentation.

``` csharp
// This parameter can be used as `--find <wildcard>`, `-f <wildcard>` or `/f <wildcard>`
[Description("Find files that match a wildcard")]
public void Find(string wildcard);
```
See https://github.com/theunrepentantgeek/Niche.CommandLineProcessor/wiki/Conventions#parameters for more.

Modes
-----
A mode is defined by a method returning a new options instance, with no parameters and a [Description] attribute for documentation.

``` csharp
[Description("Compare the results of two compilers between systems")]
public CompilerDriver TestCompiler();
```
See https://github.com/theunrepentantgeek/Niche.CommandLineProcessor/wiki/Mode-Support for more.
