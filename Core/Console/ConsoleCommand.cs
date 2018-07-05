using System;

namespace JusticeFramework.Core.Console {
    /// <summary>
    /// Command definition that allows a method to be marked as a console command
    /// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class ConsoleCommand : Attribute {
        /// <summary>
        /// The console command key used as a command accessor through the console
        /// </summary>
		public string Key { get; }

        /// <summary>
        /// Deescription of the command and its actions
        /// </summary>
		public string Description { get; }

        /// <summary>
        /// The target of the command whether its the player, or the players target
        /// </summary>
		public ECommandTarget Target { get; }

        /// <summary>
        /// Constructs a new console command
        /// </summary>
        /// <param name="command">The command key used as the command accessor through the console</param>
        /// <param name="description">Description of the command and its actions</param>
        /// <param name="target">Target of the command</param>
		public ConsoleCommand(string command, string description, ECommandTarget target = ECommandTarget.Self) {
			Key = command;
			Description = description;
			Target = target;
		}
	}
}