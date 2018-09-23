using JusticeFramework.Core;
using JusticeFramework.Core.Events;
using JusticeFramework.Core.Interfaces;
using JusticeFramework.Core.Models;
using JusticeFramework.Utility.Extensions;
using System;
using UnityEngine;

namespace JusticeFramework.Components {
    /// <inheritdoc cref="WorldObject" />
    /// <summary>
    /// This class houses all model and functions for switches, levers, buttons, etc
    /// </summary>
    [Serializable]
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(AudioSource))]
	public class Activator : WorldObject, IActivator {
		/// <summary>
		/// Event called when the activator's state changes
		/// </summary>
		public event OnActivationStateChanged onActivated;

#region Variables
		
		/// <summary>
		/// Attached animator component
		/// </summary>
		[SerializeField]
		private Animator animator;

		/// <summary>
		/// All linked references to be affected when activated
		/// </summary>
		[SerializeField]
		private WorldObject[] linkedReferences = null;
		
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
		protected override void OnIntialized() {
			animator = GetComponent<Animator>();
			audioSource = GetComponent<AudioSource>();

			animator.SetBool("IsOn", isOn);
		}

		public override void Activate(object sender, ActivateEventArgs e) {
            // If we don't know who activate this, do nothing
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
            PlaySound(ActivationSound, EAudioType.SoundEffect);

			// Activate all attached references
			foreach (WorldObject reference in linkedReferences) {
				reference?.Activate(this, new ActivateEventArgs(this, sender));
			}

			// Send out a state changed event
			OnReferenceStateChanged();
			onActivated?.Invoke(this, sender, isOn);
		}
	}
}
