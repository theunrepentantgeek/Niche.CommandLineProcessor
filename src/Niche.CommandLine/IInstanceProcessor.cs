using System.Collections.Generic;

namespace Niche.CommandLine
{
    public interface IInstanceProcessor
    {
        /// <summary>
        /// Gets all the modes defined
        /// </summary>
        IReadOnlyList<CommandLineMode> Modes { get; }

        /// <summary>
        /// Gets all the switches defined
        /// </summary>
        IReadOnlyList<CommandLineSwitch> Switches { get; }

        /// <summary>
        /// Gets all the parameters defined
        /// </summary>
        IReadOnlyList<CommandLineOptionBase> Parameters { get; }
    }
}