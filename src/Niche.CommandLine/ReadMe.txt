
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

Options
-------
To configure the options available, follow the following conventions:

``` csharp
// A switch
// (void method with no parameters)
// --verbose
// -v
[Description("Verbose output")]
public void Verbose() ...
```

See https://github.com/theunrepentantgeek/Niche.CommandLineProcessor/wiki/Conventions#switches for more.

``` csharp
// A parameter with a single value
// (void method with single parameter)
// --output-file
// -of
[Description("Destination file for output")]
public void OutputFile(string file) ...
```

See https://github.com/theunrepentantgeek/Niche.CommandLineProcessor/wiki/Conventions#parameters for more.

``` csharp
// A parameter with multiple values
// (void method with IEnumerable parameter)
// --input-file
// -if
[Description("Input files to consume")]
public void InputFile(IEnumerable<string> files)
```

``` csharp
// A distinct program mode
// (method with no parameter returning a new options instance)
// render
[Description("Render documents")]
public RenderOptions Render()
{
    return new RenderOptions();
}
```

See https://github.com/theunrepentantgeek/Niche.CommandLineProcessor/wiki/Mode-Support for more.
