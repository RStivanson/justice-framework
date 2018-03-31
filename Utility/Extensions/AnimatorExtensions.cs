using UnityEngine;

namespace JusticeFramework.Utility.Extensions {
	public static class AnimatorExtensions {
		public static bool IsPlaying(this Animator animator, int layer){
			AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(layer);
			return (info.length < info.normalizedTime) || animator.IsInTransition(layer);
		}

		public static bool IsPlaying(this Animator animator, int layer, string stateName){
			AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(layer);
			return ((info.length > info.normalizedTime) && info.IsName(stateName)) || animator.IsInTransition(layer);
		}
	}
}
