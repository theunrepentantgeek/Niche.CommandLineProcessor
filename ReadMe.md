Niche.CommandLine
=================
[![Build status](https://ci.appveyor.com/api/projects/status/15gp6ykhvav0g9ip)](https://ci.appveyor.com/project/theunrepentantgeek/niche-commandlineprocessor)

A simple convention based argument handler to make it easy for .NET developers to write console applications.

## Conventions

Commandline options are declared by writing methods that comply with the appropriate convention (see below for examples). 

Each option has a short form starting with a single dash `-` (e.g.: `-f`, `-h` or `-rs`) and a long form starting with a 
double dash `--` (e.g.: `--find`, `--help` or `--report-status`). These names are derived from the names of the 
implementing methods (e.g.: `Find()`, `Help()` or `ReportStatus()`).

### Switches

To define a switch, declare a method with no return and no parameters. Give it a `[Description]` attribute to document
what the switch does. 

For example, this method:

    [Description("Show help listing all available options")]
    public void Help();
    
will give the options `-h` and `--help`.

### Parameters

To define a parameter, declare a method with no return and one parameter. Again, give it a `[Description]` attribute to
document what the option does.

For example, this method:

    [Description("Find files that match a wildcard")]
    public void Find(string wildcard);
    
will give the options `-f <wildcard>` and `--find <wildcard>`.

### Modes

To define a whole new mode, declare a method returning a new driver instance that has no parameters. Again, you "opt-in"
by giving it a `[Description]` attribute to document what the mode does.

For example, this method:

    [Description("Compare the results of two compilers between systems")]
    public CompilerDriver TestCompiler();

declares a mode `test-compiler`.

See [Mode Support](https://github.com/theunrepentantgeek/Niche.CommandLineProcessor/wiki/Mode-Support) for more 
information.
