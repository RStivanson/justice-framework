using System;
using UnityEngine;

namespace JusticeFramework {
    [Serializable]
    public class EntityAnimator {
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private AnimatorOverrideController overrideController;

        public Animator Animator {
            get { return animator; }
        }

        public EntityAnimator(Animator animator) {
            this.animator = animator;
            InitializeOverrideController(this.animator.runtimeAnimatorController);
        }

        private void InitializeOverrideController(RuntimeAnimatorController runtimeController) {
            overrideController = new AnimatorOverrideController(runtimeController);
            animator.runtimeAnimatorController = overrideController;
        }

        public void SetAnimation(string name, AnimationClip animationClip) {
            overrideController[name] = animationClip;
        }
    }
}
