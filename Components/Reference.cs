using System;
using System.Runtime.InteropServices;
using JusticeFramework.Console;
using JusticeFramework.Data;
using JusticeFramework.Data.Events;
using JusticeFramework.Data.Interfaces;
using JusticeFramework.Data.Models;
using JusticeFramework.Interfaces;
using JusticeFramework.Utility.Extensions;
using UnityEngine;

namespace JusticeFramework.Components {
	/// <summary>
	/// Base class for all in-game objects
	/// </summary>
	[Serializable]
	public abstract class Reference : MonoBehaviour, IWorldObject, IInteractable, IOnReferenceStateChanged {
		public event OnReferenceStateChanged onReferenceStateChanged;

#region Variables
		
		/// <summary>
		/// Stores the model relating to this reference
		/// </summary>
		[SerializeField]
		protected WorldObjectModel model;

#endregion
		
#region Properties

		/// <summary>
		/// The ID associated with this entity
		/// </summary>
		public string Id {
			get { return model.id; }
		}

		/// <summary>
		/// The hashed version of the ID associated with this entity
		/// </summary>
		public int HashedId {
			get { return model.hashedId; }
		}
		
		/// <summary>
		/// The display name of this entity
		/// </summary>
		public virtual string DisplayName {
			get { return model.displayName; }
		}
		
		/// <summary>
		/// Flag indicating if the object can be activated
		/// </summary>
		public virtual bool CanBeActivated {
			get { return model.canBeActivated; }
		}
		
		/// <summary>
		/// The type of interaction used by this reference
		/// </summary>
		public abstract EInteractionType InteractionType { get; }
		
		/// <summary>
		/// The transform that is attached to this object
		/// </summary>
		public Transform Transform {
			get { return transform; }
		}
		
#endregion
		
		/// <summary>
		/// First initialization method called when the object is spawned
		/// </summary>
		private void Awake() {
			// TODO: Remove this once the models are no longer inheriting the ScriptableObject class
			if (model != null) {
				model = Instantiate(model);
			}
			
			GameManager.Instance.Register(this);
			
			OnIntialize();
		}
		
		/// <summary>
		/// Handles the safe removal of the object from the game
		/// </summary>
		private void OnDestroy() {
			GameManager.Instance.Unregister(this);
		}

		/// <summary>
		/// Internal method intialization method called after Awake
		/// </summary>
		protected virtual void OnIntialize() {
		}

		/// <summary>
		/// Sets the data model associated with this entity
		/// </summary>
		/// <param name="newModel">The data to attach to this entity</param>
		/// <param name="clone">Flag indicating if the data should be deep cloned</param>
		public void SetData(WorldObjectModel newModel, bool clone) {
			if (newModel.NotTypeOrSubType<WorldObjectModel>()) {
				return;
			}
			

			model = (clone ? Instantiate(newModel) : newModel);
		}

		/// <summary>
		/// Determines if this reference has an associated model
		/// </summary>
		/// <returns>Returns true if the reference has a model, false otherwise</returns>
		public bool HasModel() {
			return model != null;
		}
		
		public virtual void Activate(object sender, ActivateEventArgs e) {
		}

		[ConsoleCommand("destroy", "Destroys the target reference", ECommandTarget.LookAt)]
		private void Destroy() {
			Destroy(this.gameObject);
		}
		
		public void OnReferenceStateChanged() {
			onReferenceStateChanged?.Invoke(this);
		}
	}
}