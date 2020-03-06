using UnityEngine;

namespace JusticeFramework.Core.Extensions {
	/// <summary>
	/// A collection of extension methods that extend the functionality of animator components
	/// </summary>
	public static class AnimatorExtensions {
		/// <summary>
		/// Determines if the given animation layer is currently playing on the given layer
		/// </summary>
		/// <param name="animator">The animator that is playing animations</param>
		/// <param name="layer">The layer of the animation to check</param>
		/// <returns>Returns true if the layer is currently playing, false otherwise</returns>
		public static bool IsPlaying(this Animator animator, int layer){
			AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(layer);
			return (info.length < info.normalizedTime) || animator.IsInTransition(layer);
		}

		/// <summary>
		/// Determines if the given animation layer is currently playing the given state on the given layer
		/// </summary>
		/// <param name="animator">The animator that is playing animations</param>
		/// <param name="layer">The layer of the animation to check</param>
		/// <param name="stateName">The name of the animation state to check</param>
		/// <returns>Returns true if the layer is currently playing the specified state, false otherise</returns>
		public static bool IsPlaying(this Animator animator, int layer, string stateName){
			AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(layer);
			return ((info.length > info.normalizedTime) && info.IsName(stateName)) || animator.IsInTransition(layer);
		}
	}
}
