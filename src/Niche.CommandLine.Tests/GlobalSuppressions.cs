
// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Design",
    "CA1052:Static holder types should be Static or NotInheritable",
    Justification = "This project uses wrapper classes to group tests.")]
[assembly: SuppressMessage(
    "Design",
    "RCS1102:Mark class as static.",
    Justification = "This project uses wrapper classes to group tests.")]

