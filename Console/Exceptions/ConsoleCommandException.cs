using System;

namespace JusticeFramework.Console.Exceptions {
	public class ConsoleCommandException : Exception {
		public ConsoleCommandException(string message, Exception innerException = null) : base(message, innerException) {
		}
	}
}