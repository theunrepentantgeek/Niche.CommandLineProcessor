
// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Naming",
    "CA1715:Type parameter names should be prefixed with 'T'",
    Justification = "This project prefers single character capital letters for type parameters.")]

[assembly:SuppressMessage(
    "Performance",
    "RCS1080:Use 'Count/Length' property instead of 'Any' method.",
    Justification = "This project prefers the clarity of Any()")]

