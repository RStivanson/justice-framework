using JusticeFramework.Console.Exceptions;
using JusticeFramework.Core.Controllers;
using JusticeFramework.Core.Interfaces;
using JusticeFramework.Core.Managers;
using JusticeFramework.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

namespace JusticeFramework.Core.Console {
    [Serializable]
	public class CommandLibrary {
		private const string ReservedSelf = "self";
		private const string ReservedPlayer = "player";
        private const string ReservedTarget = "target";
        private const string ReservedLookAt = "lookat";
        private const string ReservedRaycastHit = "hit";
        private const string ReservedZero = "zero";
        private const string ReservedOne = "one";
        private const string ReservedVector2Zero = "vector2.zero";
        private const string ReservedVector3Zero = "vector3.zero";
		private const string ReservedVector4Zero = "vector4.zero";
        private const string ReservedIdentity = "identity";
		private const string ReservedQuaternionIdentity = "quaternion.identity";

        private const string ReservedReferenceModifierForward = "forward";
        private const string ReservedReferenceModifierBackward = "backward";
        private const string ReservedReferenceModifierUp = "up";
        private const string ReservedReferenceModifierDown = "down";
        private const string ReservedReferenceModifierLeft = "left";
        private const string ReservedReferenceModifierRight = "right";
        private const string ReservedReferenceModifierId = "id";
        private const string ReservedReferenceModifierGameObjectName = "name";
        private const string ReservedReferenceModifierDisplayName = "displayname";

        private const float RaycastDistance = 50.0f;

        [SerializeField]
		private static Dictionary<string, List<Command>> registeredCommands;

		[SerializeField]
		private static Dictionary<string, Func<ParameterInfo, string, object>> keywordHandlers;

		[SerializeField]
		private IInteractionController interactionController;

		public CommandLibrary(bool forceReload = false) {
			if (registeredCommands == null || forceReload) {
				LoadCommands();
			}

			if (keywordHandlers == null) {
				DefineKeywords();
			}
		}

		public void SetInteractionController(IInteractionController controller) {
			interactionController = controller;
		}

#if UNITY_EDITOR

		[ConsoleCommand("help", "Lists all available commands to the Unity console")]
		public static void PrintAllCommands() {
			if (registeredCommands == null) {
				Debug.Log("There are currently no registered commands");
				return;
			}

			foreach (KeyValuePair<string, List<Command>> commandEntry in registeredCommands) {
				foreach (Command command in commandEntry.Value) {
					Debug.Log($"{command.commandData.Key}: {command.commandData.Description}");
				}
			}
		}

#endif

#region Keywords

		private void DefineKeywords() {
			keywordHandlers = new Dictionary<string, Func<ParameterInfo, string, object>>();

            keywordHandlers.Add(ReservedSelf, SelfKeywordHandler);
			keywordHandlers.Add(ReservedPlayer, SelfKeywordHandler);
			keywordHandlers.Add(ReservedTarget, TargetKeywordHandler);
			keywordHandlers.Add(ReservedLookAt, TargetKeywordHandler);
            keywordHandlers.Add(ReservedRaycastHit, RaycastHitKeywordHandler);
            keywordHandlers.Add(ReservedZero, ZeroKeywordHandler);
            keywordHandlers.Add(ReservedOne, OneKeywordHandler);
            keywordHandlers.Add(ReservedVector2Zero, Vector2ZeroKeywordHandler);
            keywordHandlers.Add(ReservedVector3Zero, Vector3ZeroKeywordHandler);
            keywordHandlers.Add(ReservedVector4Zero, Vector4ZeroKeywordHandler);
            keywordHandlers.Add(ReservedIdentity, IdentityKeywordHandler);
            keywordHandlers.Add(ReservedQuaternionIdentity, IdentityKeywordHandler);
		}

		private bool IsKeyword(string data) {
			string[] split = data.Split('.');

			if (split.Length == 0) {
				return false;
			}

			return keywordHandlers.ContainsKey(split[0]);
		}

		private object HandleKeyword(ParameterInfo parameterInfo, string data) {
			string[] split = data.Split('.');

			if (split.Length == 0) {
				return null;
			}

			return keywordHandlers[split[0]].Invoke(parameterInfo, data);
		}

		private object HandleReferenceModifiers(IWorldObject reference, string modifier) {
			object result = null;

			if (reference != null) {
				if (modifier.Equals(ReservedReferenceModifierForward)) {
					result = reference.Transform.forward;
				} else if (modifier.Equals(ReservedReferenceModifierBackward)) {
					result = -reference.Transform.forward;
				} else if (modifier.Equals(ReservedReferenceModifierUp)) {
					result = reference.Transform.up;
				} else if (modifier.Equals(ReservedReferenceModifierDown)) {
					result = -reference.Transform.up;
				} else if (modifier.Equals(ReservedReferenceModifierRight)) {
					result = reference.Transform.right;
				} else if (modifier.Equals(ReservedReferenceModifierLeft)) {
					result = -reference.Transform.right;
				} else if (modifier.Equals(ReservedReferenceModifierId)) {
					result = reference.Id;
				} else if (modifier.Equals(ReservedReferenceModifierGameObjectName)) {
					result = reference.Transform.name;
				} else if (modifier.Equals(ReservedReferenceModifierDisplayName)) {
					result = reference.DisplayName;
				}
			}
			
			return result;
		}

		private object SelfKeywordHandler(ParameterInfo parameterInfo, string data) {
			object result = null;
			string[] split = data.Split('.');

			if (split.Length == 1) {
				result = GameManager.Player;
			} else if (split.Length > 1) {
				result = HandleReferenceModifiers(GameManager.Player, split[1]);
			}

			return result;
		}

		private object TargetKeywordHandler(ParameterInfo parameterInfo, string data) {
			object result = null;
			string[] split = data.Split('.');

			if (split.Length == 1) {
				result = interactionController?.CurrentTarget;
			} else if (split.Length > 1) {
				result = HandleReferenceModifiers(interactionController?.CurrentTarget, split[1]);
			}

			return result;
		}

		private object ZeroKeywordHandler(ParameterInfo parameterInfo, string data) {
			object result = null;

			if (parameterInfo.ParameterType == typeof(Vector2)) {
				result = Vector2.zero;
			} else if (parameterInfo.ParameterType == typeof(Vector2Int)) {
				result = Vector2Int.zero;
			} else if (parameterInfo.ParameterType == typeof(Vector3)) {
				result = Vector3.zero;
			} else if (parameterInfo.ParameterType == typeof(Vector3Int)) {
				result = Vector3Int.zero;
			} else if (parameterInfo.ParameterType == typeof(Vector4)) {
				result = Vector4.zero;
			}

			return result;
		}
		
		private object OneKeywordHandler(ParameterInfo parameterInfo, string data) {
			object result = null;

			if (parameterInfo.ParameterType == typeof(Vector2)) {
				result = Vector2.one;
			} else if (parameterInfo.ParameterType == typeof(Vector2Int)) {
				result = Vector2Int.one;
			} else if (parameterInfo.ParameterType == typeof(Vector3)) {
				result = Vector3.one;
			} else if (parameterInfo.ParameterType == typeof(Vector3Int)) {
				result = Vector3Int.one;
			} else if (parameterInfo.ParameterType == typeof(Vector4)) {
				result = Vector4.one;
			}

			return result;
		}

        private object RaycastHitKeywordHandler(ParameterInfo parameterInfo, string data) {
            Transform mainCamera = Camera.main.transform;
            RaycastHit hit;

            Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, RaycastDistance);

            return hit.transform;
        }

        private object Vector2ZeroKeywordHandler(ParameterInfo parameterInfo, string data) {
            return Vector2.zero;
        }

        private object Vector3ZeroKeywordHandler(ParameterInfo parameterInfo, string data) {
            return Vector3.zero;
        }

        private object Vector4ZeroKeywordHandler(ParameterInfo parameterInfo, string data) {
            return Vector4.zero;
        }

        private object IdentityKeywordHandler(ParameterInfo parameterInfo, string data) {
            return Quaternion.identity;
        }

        private object VectorTypeHandler(ParameterInfo parameterInfo, string data) {
			object result = null;
			
			if (!data.Equals(string.Empty)) {
				string[] split = data.Split(',');
	
				if (parameterInfo.ParameterType == typeof(Vector2)) {
					result = new Vector2(
						float.Parse(split[0]),
						float.Parse(split[1])
					);
				} else if (parameterInfo.ParameterType == typeof(Vector2Int)) {
					result = new Vector2Int(
						int.Parse(split[0]),
						int.Parse(split[1])
					);
				} else if (parameterInfo.ParameterType == typeof(Vector3)) {
					result = new Vector3(
						float.Parse(split[0]),
						float.Parse(split[1]),
						float.Parse(split[2])
					);
				} else if (parameterInfo.ParameterType == typeof(Vector3Int)) {
					result = new Vector3Int(
						int.Parse(split[0]),
						int.Parse(split[1]),
						int.Parse(split[2])
					);
				} else if (parameterInfo.ParameterType == typeof(Vector4)) {
					result = new Vector4(
						float.Parse(split[0]),
						float.Parse(split[1]),
						float.Parse(split[2]),
						float.Parse(split[3])
					);
				} else if (parameterInfo.ParameterType == typeof(Quaternion)) {
					result = new Quaternion(
						float.Parse(split[0]),
						float.Parse(split[1]),
						float.Parse(split[2]),
						float.Parse(split[3])
					);
				}
			}
		
			return result;
		}

		private bool IsQuoted(string data) {
			return data.Length > 1 && data[0] == '"' && data[data.Length - 1] == '"';
		}
		
		private bool IsParenthesized(string data) {
			return data.Length > 1 && data[0] == '(' && data[data.Length - 1] == ')';
		}
		
		private string RemoveCasing(string data) {
			return data.Substring(1, data.Length - 2);
		}
		
#endregion

#region Command Execution

		public void ExecuteCommand(string commandLineString) {
			string[] arguments = SplitCommandArguments(commandLineString);
			
			if (arguments.Length == 0) {
				return;
			}

			// Lower the console command key so we can avoid any one letter mismatches
			arguments[0] = arguments[0].ToLower();

			List<Command> commandsWithKey;
			if (registeredCommands.TryGetValue(arguments[0], out commandsWithKey)) {
				Exception encounteredException = TryCommands(commandsWithKey, arguments);

				if (encounteredException != null) {
					Debug.LogError(encounteredException.Message);
				}
			} else {
				Debug.LogError($"No commands with the given key '{arguments[0]}' have been registered");
			}
		}

		private Exception TryCommands(List<Command> commands, string[] parameters) {
			Exception lastException = null;
			bool successfullyInvokedMethod = false;
			
			foreach (Command command in commands) {
				try {
					object[] parameterObjects = ConvertParametersToObjectList(command, parameters);
					
					if (command.method.IsStatic) {
						successfullyInvokedMethod = command.Invoke(parameterObjects);
					} else if (command.commandData.Target == ECommandTarget.Self) {
						successfullyInvokedMethod = command.Invoke(parameterObjects, GameManager.Player);
					} else if (command.commandData.Target == ECommandTarget.LookAt) {
						if (interactionController != null) {
							if (interactionController.CurrentTarget == null || interactionController.CurrentTarget.NotType(command.parentType)) {
								throw new Exception($"The specified command '{parameters[0]}' requires a target of type {command.parentType.Name}");
							}
							
							successfullyInvokedMethod = command.Invoke(parameterObjects, interactionController.CurrentTarget);
						} else {
							throw new Exception($"The specified command '{parameters[0]}' requires an InteractionController to be set");
						}
					}
				} catch (Exception e) {
					lastException = e;
				}

				if (successfullyInvokedMethod) {
					lastException = null;
					break;
				}
			}

			return lastException;
		}
		
		private string[] SplitCommandArguments(string commandLine) {
			// Get all parameter matches
			//MatchCollection matches = Regex.Matches(commandLine, @"("".*?""|[^ ""]+)+");
			MatchCollection matches = Regex.Matches(commandLine, @"("".*?""|\(.*?\)|[^ ""]+)+");
			List<string> arguments = new List<string>();
			
			// Foreach match
			foreach (Match match in matches) {
				string newValue = match.Value;
				
				// If the match isnt empty
				if (!string.IsNullOrEmpty(newValue)) {
					// If the value is wrapped in quotes, remove the quotes
					/*if (newValue.Length > 1 && newValue[0] == '"' && newValue[newValue.Length - 1] == '"') {
						newValue = newValue.Substring(1, newValue.Length - 2);
					} else if (newValue.Length > 1 && newValue[0] == '(' && newValue[newValue.Length - 1] == ')') {
						// If the value is wrapped in parenthesis, remove the parenthesis
						newValue = newValue.Substring(1, newValue.Length - 2);
					}*/
					
					// Add the argument to the list
					arguments.Add(newValue);
				}
			}
			
			return arguments.ToArray();
		}
		
		private object[] ConvertParametersToObjectList(Command command, string[] parameters) {
			ParameterInfo[] parameterInfos = command.method.GetParameters();
			object[] parameterObjects = new object[parameterInfos.Length];
			int parameterIndex;
			
			// For each parameter info in the methods paramters
			for (int i = 0; i < parameterInfos.Length; i++) {
				// We use (i + 1) because the first argument of the parameters array is the command that is being called
				parameterIndex = i + 1;
				
				// If the index is greater than the parameters we've been given
				if (parameterIndex >= parameters.Length) {
					// Check if the parameter has a default value
					if (parameterInfos[i].RawDefaultValue != null) {
						// Use the default value in place of the parameter
						parameterObjects[i] = parameterInfos[i].RawDefaultValue;   
					} else {
						// If it does not have a default value, throw a fit
						throw new MissingArgumentException(parameterInfos.Length, parameters.Length - 1, null);
					}
				} else {
					// If the given parameter is a reserved keyword
					if (IsKeyword(parameters[parameterIndex])) {
						// handle the keyword
						parameterObjects[i] = HandleKeyword(parameterInfos[i], parameters[parameterIndex]);
					} else if (IsParenthesized(parameters[parameterIndex])) {
						parameterObjects[i] = VectorTypeHandler(parameterInfos[i], RemoveCasing(parameters[parameterIndex]));
					} else {
						string data = parameters[parameterIndex];

						if (IsQuoted(data)) {
							data = RemoveCasing(data);
						}
						
						// Convert the parameter to the specified type
						parameterObjects[i] = TypeDescriptor.GetConverter(parameterInfos[i].ParameterType).ConvertFrom(data);
					}
				}
			}

			return parameterObjects;
		}

#endregion

#region Command Management

		private void LoadCommands() {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            registeredCommands = new Dictionary<string, List<Command>>();

            foreach (Assembly assembly in assemblies) {
                if (assembly.FullName.StartsWith("Assembly-CSharp") || assembly.FullName.StartsWith("JusticeFramework")) {
                    RegisterCommandsFromAssembly(assembly);
                }
            }
		}
		
		private void RegisterCommandsFromAssembly(Assembly assembly) {
			foreach (Type type in assembly.GetTypes()) {
				// We want all public and private, instance and static methods. We also only want to process the top level declared methods
				// because we do not want duplicates for a class showing up in the registered commands list.
				foreach (MethodInfo method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)) {
					RegisterCommandsFromMethod(method);
				}
			}
		}

		private void RegisterCommandsFromMethod(MethodInfo method) {
			foreach (Attribute attribute in method.GetCustomAttributes(true)) {
				if (attribute.NotType<ConsoleCommand>()) {
					continue;
				}
				
				Command newCommand = new Command {
					parentType = method.ReflectedType,
					commandData = (ConsoleCommand)attribute,
					method = method
				};

				// Lower the console command key so we can avoid any one letter mismatches
				string commandKey = newCommand.commandData.Key.ToLower();
				
				if (registeredCommands.ContainsKey(commandKey)) {
					registeredCommands[commandKey].Add(newCommand);
				} else {
					registeredCommands.Add(commandKey, new List<Command> { newCommand });
				}
			}
		}

#endregion
	}
}