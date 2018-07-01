using JusticeFramework.Core.Console;
using JusticeFramework.Core.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JusticeFramework.UI.Views {
    [Serializable]
	public class ConsoleView : Window {
		public InputField commandTextInput;

		[SerializeField]
		private CommandLibrary commandLibrary;
		
		[SerializeField]
	    private static List<string> commandHistory;
	    
		[SerializeField]
		[HideInInspector]
	    private int historyIndex = -1;

		private void Update() {
			if (!commandTextInput.isFocused) {
				return;
			}

			if (commandHistory.Count == 0) {
				return;
			}

			if (Input.GetKeyDown(KeyCode.UpArrow)) {
				DecreaseCommandHistoryIndex();
			} else if (Input.GetKeyDown(KeyCode.DownArrow)) {
				IncreaseCommandHistoryIndex();
			}
		}

		public void SetCommandLibrary(CommandLibrary library) {
			commandLibrary = library;
		}
		
#region Event Callbacks

		private void OnInputEndEdit(string content) {
			if (Input.GetKeyDown(KeyCode.Return) && !commandTextInput.text.Equals(string.Empty)) {
				commandLibrary.ExecuteCommand(commandTextInput.text);
				commandHistory.Add(commandTextInput.text);

				commandTextInput.text = string.Empty;
				historyIndex = -1;
			}

			commandTextInput.ActivateInputField();
		}
		
		protected override void OnShow() {
			if (commandHistory == null) {
				commandHistory = new List<string>();
				historyIndex = -1;
			}

			commandTextInput.ActivateInputField();
			commandTextInput.onEndEdit.AddListener(OnInputEndEdit);
		}

#endregion

#region History Methods

		private void AddHistoryEntry(string command) {
			commandHistory.Add(command);
		}
		
		private void IncreaseCommandHistoryIndex() {
			if (historyIndex == -1) {
				return;
			}

			historyIndex++;
            
			if (historyIndex == commandHistory.Count) {
				historyIndex = -1;
				commandTextInput.text = string.Empty;
			} else {
				commandTextInput.text = commandHistory[historyIndex];
			}
			
			commandTextInput.MoveTextEnd(false);
		}
	    
		private void DecreaseCommandHistoryIndex() {
			if (historyIndex == -1) {
				historyIndex = commandHistory.Count - 1;
			} else if (historyIndex != 0) {
				historyIndex--;
			}

			commandTextInput.text = commandHistory[historyIndex];
			commandTextInput.MoveTextEnd(false);
		}

#endregion
	}
}