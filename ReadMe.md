Niche.CommandLine
=================

A simple convention based argument handler to make it easy for .NET developers to write console applications.



## Conventions

To define a switch, declare a method with no return and no parameters. Give it a `[Description]` attribute to document
what the switch does. 

For example, this method:

    [Description("Show help listing all available options")]
    public void Help();
    
will give the options `-h` and `--help`.

To define a parameter, declare a method with no return and one parameter. Again, give it a `[Description]` attribute to
document what the option does.

For example, this method:

    [Descrpition("Find files that match a wildcard")]
    public void Find(string wildcard);
    
will give the options `-f <wildcard>` and `--find <wildcard>`.


