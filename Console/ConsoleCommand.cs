using System;

namespace JusticeFramework.Console {
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class ConsoleCommand : Attribute {
		public string Key { get; }
		public string Description { get; }
		public ECommandTarget Target { get; }

		public ConsoleCommand(string command, string description, ECommandTarget target = ECommandTarget.Self) {
			Key = command;
			Description = description;
			Target = target;
		}
	}
}