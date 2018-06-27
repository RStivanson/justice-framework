using System;
using System.Reflection;

namespace JusticeFramework.Core.Console {
	public class Command {
		public Type parentType;
		public ConsoleCommand commandData;
		public MethodInfo method;
			
		public bool Invoke(object[] parameters, object target = null) {
			if (method.IsStatic) {
				method.Invoke(null, parameters);
			} else if (target != null){
				method.Invoke(target, parameters);
			} else {
				return false;
			}
	
			return true;
		}
	}
}