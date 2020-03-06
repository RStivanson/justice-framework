using JusticeFramework.Console;
using JusticeFramework.Data;
using JusticeFramework.Interfaces;
using JusticeFramework.Logic;
using System;
using UnityEngine;

namespace JusticeFramework.Components {
    [Serializable]
	[RequireComponent(typeof(Animator))]
	public class Door : WorldObject {
		[SerializeField]
		private Animator animator;

		[SerializeField]
		private bool isOpen;

		[SerializeField]
		private bool isLocked;

		[SerializeField]
		private ELockDifficulty lockDifficulty;

        [SerializeField]
        private ItemData requiredKey;

        [SerializeField]
		private string destinationScene;
		
		[SerializeField]
		private Vector3 destinationPosition;
		
		[SerializeField]
		private Quaternion destinationRotation;
		
		public override string DisplayName {
			get {
                string displayName = base.DisplayName;

                if (isLocked) {
					displayName = $"{displayName} ({lockDifficulty.ToString()})";
				}

				return displayName;
			}
		}
		
		public override EInteractionType InteractionType {
			get {
                DoorData data = dataObject as DoorData;

                if (data == null) {
                    return EInteractionType.Open;
                }

				switch (data.DoorType) {
					case EDoorType.Portal:
						return EInteractionType.Open;
					default:
						return isOpen ? EInteractionType.Close : EInteractionType.Open;
				}
			}
		}
		
		protected override void OnIntialized() {
			animator = GetComponent<Animator>();
			animator?.SetBool("IsOpen", isOpen);
		}
		
		public void Open() {
			isOpen = true;
			animator?.SetBool("IsOpen", true);
            // TODO
            //PlaySound(OpenSound, EAudioType.SoundEffect, 1);
            OnReferenceStateChanged();
		}

		public void Close() {
			isOpen = false;
			animator?.SetBool("IsOpen", false);
            // TODO
            //PlaySound(OpenSound, EAudioType.SoundEffect, 1);
            OnReferenceStateChanged();
		}

        public void ToggleOpen() {
            if (isOpen) {
                Close();
            } else {
                Open();
            }
        }

        [ConsoleCommand("lock", "Locks the target container", ECommandTarget.LookAt)]
		public void Lock() {
			isLocked = true;
            // TODO
            // PlaySound(LockSound, EAudioType.SoundEffect, 1);
            OnReferenceStateChanged();
		}
		
		[ConsoleCommand("unlock", "Unlocks the target container", ECommandTarget.LookAt)]
		public void Unlock() {
			isLocked = false;
            // TODO
            // PlaySound(LockSound, EAudioType.SoundEffect, 1);
            OnReferenceStateChanged();
		}

        protected override Logic.Action OnActivate(IWorldObject activator) {
            DoorData data = dataObject as DoorData;
            Logic.Action action = null;

            if (isLocked) {
                if (lockDifficulty == ELockDifficulty.Impossible && requiredKey == null) {
                    action = new ActionFailed(this, "This door cannot be unlocked.");
                    return action;
                }

                IContainer container = activator as IContainer;
                if (container.Inventory.Contains(requiredKey.Id)) {
                    Unlock();
                } else if (container.Inventory.HasItemWithTag("ItemLockpick")) {
                    // TODO
                    // action = new ActionLockpick(this);
                    // return;
                }
            }

            if (!isLocked) {
                if (data.DoorType == EDoorType.Portal) {
                    action = new ActionTeleport(this, destinationScene, destinationPosition, destinationRotation);
                    return action;
                } else {
                    ToggleOpen();
                    action = new ActionEmpty();
                    return action;
                }
            }

            return new ActionFailed(this, null);
		}
	}
}
