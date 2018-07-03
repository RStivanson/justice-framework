using JusticeFramework.Utility.Extensions;
using System;
using UnityEngine;

namespace JusticeFramework {
    [Serializable]
    public class ActorAnimator {
        [SerializeField]
        private Animator firstPersonAnimator;

        [SerializeField]
        private Animator thirdPersonAnimator;

        public Animator FirstPersonAnimator {
            get { return firstPersonAnimator; }
        }

        public Animator ThirdPersonAnimator {
            get { return thirdPersonAnimator; }
        }

        public void SetOverrideController(AnimatorOverrideController fpsOverride, AnimatorOverrideController tpsOverride) {
            if (firstPersonAnimator != null) {
                firstPersonAnimator.runtimeAnimatorController = fpsOverride;
            }

            if (thirdPersonAnimator != null) {
                thirdPersonAnimator.runtimeAnimatorController = tpsOverride;
            }
        }

        public bool IsPlaying(int layer, string name) {
            return (firstPersonAnimator?.IsPlaying(layer, name) ?? false)
                || (thirdPersonAnimator?.IsPlaying(layer, name) ?? false);
        }

        public void SetBool(string name, bool value) {
            if (firstPersonAnimator != null)
                firstPersonAnimator.SetBool(name, value);

            if (thirdPersonAnimator != null)
                thirdPersonAnimator.SetBool(name, value);
        }

        public void SetTrigger(string name) {
            if (firstPersonAnimator != null)
                firstPersonAnimator?.SetTrigger(name);

            if (thirdPersonAnimator != null)
                thirdPersonAnimator?.SetTrigger(name);
        }
    }
}
