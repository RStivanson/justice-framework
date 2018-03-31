using System;
using System.Runtime.InteropServices.WindowsRuntime;
using JusticeFramework.Components;
using JusticeFramework.Data;
using JusticeFramework.Data.Events;
using JusticeFramework.Data.Models;
using UnityEngine;

namespace JusticeFramework.Components {
	[Serializable]
	public class StaticInteractable : Reference {
		[SerializeField]
		private Transform interactPosition;

		[SerializeField]
		private bool isInUse;

		public override EInteractionType InteractionType {
			get { return EInteractionType.Use; }
		}

		public Vector3 InteractPosition {
			get { return interactPosition.position; }
		}

		public bool IsInUse {
			get { return isInUse; }
		}
		
		public virtual void BeginUse(Animator animator) {
			isInUse = true;
		}

		public virtual void EndUse(Animator anim) {
			isInUse = false;
		}
	}
}