using System;
using JusticeFramework.Data.Models;
using JusticeFramework.Data.Events;
using JusticeFramework.Data.Interfaces;
using JusticeFramework.Utility.Extensions;
using UnityEngine;

namespace JusticeFramework.Components {
	/// <inheritdoc cref="Reference" />
	/// <summary>
	/// This class houses all model and functions for switches, levers, buttons, etc
	/// </summary>
	[Serializable]
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(AudioSource))]
	public class Activator : Reference, IActivator {
		/// <summary>
		/// Event called when the activator's state changes
		/// </summary>
		public event OnActivationStateChanged OnActivated;

#region Variables
		
		/// <summary>
		/// Attached animator component
		/// </summary>
		[SerializeField]
		private Animator animator;

		/// <summary>
		/// Attached audio source component
		/// </summary>
		[SerializeField]
		private AudioSource audioSource;
		
		/// <summary>
		/// All linked references to be affected when activated
		/// </summary>
		[SerializeField]
		private Reference[] linkedReferences = null;
		
		/// <summary>
		/// Flag variables stating if the activator is currently on or off
		/// </summary>
		[SerializeField]
		private bool isOn;
		
#endregion

#region Properties

		private ActivatorModel ActivatorModel {
			get { return model as ActivatorModel; }
		}
		
		/// <inheritdoc />
		public override EInteractionType InteractionType {
			get { return EInteractionType.Activate; }
		}

		public AudioClip ActivationSound {
			get { return ActivatorModel.activationSound; }
			set { ActivatorModel.activationSound = value; }
		}

#endregion

		/// <inheritdoc />
		protected override void OnIntialize() {
			animator = GetComponent<Animator>();
			audioSource = GetComponent<AudioSource>();

			animator.SetBool("IsOn", isOn);
		}

		public override void Activate(object sender, ActivateEventArgs e) {
			if (e?.Activator != null) {
				return;
			}
			
			// If the animator is currently animated, do nothing
			if (animator.IsPlaying(0)) {
				return;
			}

			// Toggle the activator on
			isOn = !isOn;
			animator.SetBool("IsOn", isOn);

			// Play the activation sound
			if (ActivatorModel.activationSound != null) {
				audioSource.pitch = 0.5f;
				audioSource.clip = ActivatorModel.activationSound;
				audioSource.Play(0);
			}

			// Activate all attached references
			foreach (Reference reference in linkedReferences) {
				reference?.Activate(this, new ActivateEventArgs(this, sender));
			}

			// Send out a state changed event
			OnReferenceStateChanged();
			OnActivated?.Invoke(this, sender, isOn);
		}
	}
}
