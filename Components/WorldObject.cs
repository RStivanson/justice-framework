using JusticeFramework.Console;
using JusticeFramework.Data;
using JusticeFramework.Interfaces;
using JusticeFramework.Logic;
using JusticeFramework.Models.Settings;
using System;
using UnityEngine;

namespace JusticeFramework.Components {
    /// <summary>
    /// Base class for all in-game objects
    /// </summary>
    [Serializable]
	public abstract class WorldObject : MonoBehaviour, IWorldObject, IInteractable, IDisplayable, IOnStateChanged<WorldObject> {
		public event OnStateChanged<WorldObject> onStateChanged;

        /// <summary>
        /// Stores the data object relating to this reference.
        /// </summary>
        [SerializeField]
        public ScriptableDataObject dataObject;

        /// <summary>
        /// A source to play audio from
        /// </summary>
        [SerializeField]
        protected AudioSource audioSource;

        public string Id {
            get { return dataObject.Id; }
        }

        public virtual string DisplayName {
            get {
                string displayName = null;

                if (dataObject is IDisplayable displayable) {
                    displayName = displayable?.DisplayName;
                }

                return displayName ?? "Undefined display name";
            }
        }

        /// <summary>
        /// The type of interaction that is performed with this object
        /// </summary>
        public virtual EInteractionType InteractionType {
            get { return EInteractionType.None; }
        }

		/// <summary>
		/// The transform that is attached to this object
		/// </summary>
		public Transform Transform {
			get { return transform; }
		}
		
		/// <summary>
		/// First initialization method called when the object is spawned
		/// </summary>
		private void Awake() {
			OnIntialized();
		}
		
		/// <summary>
		/// Handles the safe removal of the object from the game
		/// </summary>
		private void OnDestroy() {
            OnDestroyed();
		}

		/// <summary>
		/// Internal method intialization method called after Awake
		/// </summary>
		protected virtual void OnIntialized() {
		}

        /// <summary>
        /// Internal method called when the object is destroyed
        /// </summary>
        protected virtual void OnDestroyed() {
        }

        /// <summary>
        /// Sets the data model associated with this entity
        /// </summary>
        /// <param name="newModel">The data to attach to this entity</param>
        /// <param name="clone">Flag indicating if the data should be deep cloned</param>
        public void SetData(ScriptableDataObject newModel) {
            dataObject = newModel;
            OnDataModelChanged();
        }

        /// <summary>
        /// Gets the assigned data object as the given type.
        /// </summary>
        /// <typeparam name="GetAs">The type of data object to convert to</typeparam>
        /// <returns>Returns the data object in the given type, null if it fails.</returns>
        public GetAs GetData<GetAs>() where GetAs : ScriptableDataObject {
            return dataObject as GetAs;
        }

        /// <summary>
        /// Callback invoked when the data on this object changes
        /// </summary>
        protected virtual void OnDataModelChanged() {
        }

        /// <summary>
        /// Activates the reference.
        /// </summary>
        /// <param name="activator">The object that caused the activation.</param>
        /// <returns>Returns a generated action caused by this event.</returns>
        public Logic.Action Activate(IWorldObject activator) {
            if (dataObject == null) {
                return new ActionFailed(this, "This object has no valid data object.");
            }

            return OnActivate(activator);
        }

        /// <summary>
        /// Internal implementation of the activation method.
        /// </summary>
        /// <param name="activator">The object that caused the activation.</param>
        /// <returns>Returns a generated action caused by this event.</returns>
        protected virtual Logic.Action OnActivate(IWorldObject activator) {
            return new ActionEmpty();
        }

        /// <summary>
        /// Destroys the gameobject
        /// </summary>
		[ConsoleCommand("destroy", "Destroys the target reference", ECommandTarget.LookAt)]
		public void Destroy() {
			Destroy(gameObject);
		}
		
        /// <summary>
        /// Activates the internal reference state changed event
        /// </summary>
		public void OnReferenceStateChanged() {
			onStateChanged?.Invoke(this);
		}

        /// <summary>
        /// Plays a sound from the given audio source
        /// </summary>
        /// <param name="clip">The audio clip to play</param>
        /// <param name="volume">The volume to play the sound at</param>
        public void PlaySound(AudioClip clip, EAudioType audioType, float volume = 1) {
            if (clip == null || audioSource == null) {
                return;
            }

            volume = SystemSettings.GetScaledVolume(volume, audioType);
            audioSource.PlayOneShot(clip, volume);
        }
        
    }
}