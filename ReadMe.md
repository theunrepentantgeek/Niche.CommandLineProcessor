Niche.CommandLine
=================
[![Build status](https://ci.appveyor.com/api/projects/status/15gp6ykhvav0g9ip)](https://ci.appveyor.com/project/theunrepentantgeek/niche-commandlineprocessor)

A simple convention based argument handler to make it easy for .NET developers to write console applications.

## Design Goals

* Simple conventions that "just work"
* Little to no dependency on the commandline library

## Conventions

Commandline options are declared by writing methods that comply with the approriate convention (see below). Each option
has a short form starting with a single dash `-` (e.g.: `-f`, `-h` or `-rs`) and a long form starting with a double
dash `--` (e.g.: `--find`, `--help` or `--report-status`). These names are derived from the names of the implementing
methods (e.g.: `Find()`, `Help()` or `ReportStatus()`).

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

    [Descrpition("Find files that match a wildcard")]
    public void Find(string wildcard);
    
will give the options `-f <wildcard>` and `--find <wildcard>`.


