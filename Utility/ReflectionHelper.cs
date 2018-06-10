using JusticeFramework.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace JusticeFramework.Utility {
    public static class ReflectionHelper {
        public static List<MethodInfo> GetMethodsFromAssembly<T>(Assembly assembly, BindingFlags bindingFlags) where T : Attribute {
            List<MethodInfo> methods = new List<MethodInfo>();

            foreach (Type type in assembly.GetTypes()) {
                foreach (MethodInfo method in type.GetMethods(bindingFlags)) {
                    if (MethodHasAttribute<T>(method)) {
                        methods.Add(method);
                    }
                }
            }

            return methods;
        }

        public static bool MethodHasAttribute<T>(MethodInfo method) where T : Attribute {
            foreach (Attribute attribute in method.GetCustomAttributes(true)) {
                if (attribute.NotType<T>()) {
                    continue;
                }

                return true;
            }

            return false;
        }
    }
}
