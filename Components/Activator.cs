using JusticeFramework.Core.Extensions;
using JusticeFramework.Data;
using JusticeFramework.Interfaces;
using JusticeFramework.Logic;
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
	public class Activator : WorldObject {
		/// <summary>
		/// Attached animator component
		/// </summary>
		[SerializeField]
		private Animator animator;

		/// <summary>
		/// All linked references to be affected when activated
		/// </summary>
		[SerializeField]
		private WorldObject[] linkedWorldObjects;
		
		/// <summary>
		/// Flag variables stating if the activator is currently on or off
		/// </summary>
		[SerializeField]
		private bool isOn;

        /// <inheritdoc />
        public override EInteractionType InteractionType {
			get {
                if (animator.IsPlaying(0))
                    return EInteractionType.None;
                return EInteractionType.Activate;
            }
		}

        public bool IsOn {
            get { return isOn; }
            set {
                if (isOn != value) {
                    isOn = value;
                    animator.SetBool("IsOn", isOn);

                    // Send out a state changed event
                    OnReferenceStateChanged();
                }
            }
        }

		/// <inheritdoc />
		protected override void OnIntialized() {
			animator = GetComponent<Animator>();
			audioSource = GetComponent<AudioSource>();

			animator.SetBool("IsOn", isOn);
		}

        protected override Logic.Action OnActivate(IWorldObject activator) {
            // If we are not still animating
            if (!animator.IsPlaying(0)) {
                ActivatorData data = dataObject as ActivatorData;
                return new ActionActivate(this, linkedWorldObjects, data.ActivationSound, EAudioType.SoundEffect, 1);
            }

            return base.OnActivate(activator);
        }
    }
}
