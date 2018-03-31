using System;
using JusticeFramework.Console;
using JusticeFramework.Data.Models;
using JusticeFramework.Data;
using JusticeFramework.Data.Events;
using JusticeFramework.Data.Interfaces;
using JusticeFramework.Utility;
using UnityEngine;

namespace JusticeFramework.Components {
	[Serializable]
	[RequireComponent(typeof(Animator))]
	public class Door : Reference, IDoor {
#region Variables

		[SerializeField]
		private Animator animator;

		[SerializeField]
		private bool isOpen;

		[SerializeField]
		private bool isLocked;

		[SerializeField]
		private ELockDifficulty lockDifficulty;
		
		[SerializeField]
		private string destinationScene;
		
		[SerializeField]
		private Vector3 destinationPosition;
		
		[SerializeField]
		private Vector3 destinationRotation;
		
#endregion

#region Properties

		private DoorModel DoorModel {
			get { return model as DoorModel; }
		}
		
		public override string DisplayName {
			get {
				string displayName = DoorModel.displayName;

				if (IsLocked) {
					displayName = $"{displayName} ({LockDifficulty.ToString()})";
				}

				return displayName;
			}
		}
		
		public override EInteractionType InteractionType {
			get {
				switch (DoorType) {
					case EDoorType.Portal:
						return EInteractionType.Open;
					default:
						return isOpen ? EInteractionType.Close : EInteractionType.Open;
				}
			}
		}
		
		public bool IsLocked {
			get { return isLocked; }
		}

		public ELockDifficulty LockDifficulty {
			get { return lockDifficulty; }
		}
		
		public EDoorType DoorType {
			get { return DoorModel.doorType; }
		}
		
		public string DestinationScene {
			get { return destinationScene; }
		}

		public Vector3 DestinationPosition {
			get { return destinationPosition; }
		}
		
		public Vector3 DestinationRotation {
			get { return destinationRotation; }
		}

		public AudioClip OpenSound {
			get { return DoorModel.activationSound; }
		}

#endregion
		
		protected override void OnIntialize() {
			animator = GetComponent<Animator>();
			animator.SetBool("IsOpen", isOpen);
		}
		
		public void Open() {
			isOpen = true;
			animator.SetBool("IsOpen", true);
			OnReferenceStateChanged();
		}

		public void Close() {
			isOpen = false;
			animator.SetBool("IsOpen", false);
			OnReferenceStateChanged();
		}
		
		[ConsoleCommand("lock", "Locks the target container", ECommandTarget.LookAt)]
		public void Lock() {
			isLocked = true;
			OnReferenceStateChanged();
		}
		
		[ConsoleCommand("unlock", "Unlocks the target container", ECommandTarget.LookAt)]
		public void Unlock() {
			isLocked = false;
			OnReferenceStateChanged();
		}

		private void ToggleLock() {
			isLocked = !isLocked;
			OnReferenceStateChanged();
		}

		public override void Activate(object sender, ActivateEventArgs e) {
			if (IsLocked) {
				Debug.Log("Unable to open this door, it is currently locked!");
			} else {
				switch (DoorType) {
					case EDoorType.Portal:
						GameManager.SendToScene(DestinationScene, DestinationPosition, DestinationRotation);
						break;
					default:
						if (isOpen) {
							Close();
						} else {
							Open();
						}

						break;
				}
			}
		}
	}
}
