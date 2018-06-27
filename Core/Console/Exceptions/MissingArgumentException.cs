using System;
using System.Reflection;

namespace JusticeFramework.Console.Exceptions {
	public class MissingArgumentException : ConsoleCommandException {
		public MissingArgumentException(int expected, int received, Exception innerException)
			: base($"Expected {expected} arguments, received {received}") {
		}
	}
}