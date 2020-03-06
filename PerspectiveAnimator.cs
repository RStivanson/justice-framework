using JusticeFramework.Core.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeFramework {
    [Serializable]
    public class PerspectiveAnimator {
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

        public void ResetOverrideControllers() {
            if (firstPersonAnimator != null) {
                ResetController(firstPersonAnimator);
            }

            if (thirdPersonAnimator != null) {
                ResetController(thirdPersonAnimator);
            }
        }

        private void ResetController(Animator animator) {
            if (firstPersonAnimator.runtimeAnimatorController.NotType<AnimatorOverrideController>()) {
                return;
            }

            AnimatorOverrideController overrideController = (AnimatorOverrideController)animator.runtimeAnimatorController;
            AnimatorStateInfo[] layerInfo = new AnimatorStateInfo[animator.layerCount];
            Dictionary<AnimatorControllerParameter, object> parameters = new Dictionary<AnimatorControllerParameter, object>();

            for (int i = 0; i < animator.layerCount; i++) {
                layerInfo[i] = animator.GetCurrentAnimatorStateInfo(i);
            }

            foreach (AnimatorControllerParameter parameter in animator.parameters) {
                object value;

                switch (parameter.type) {
                    case AnimatorControllerParameterType.Bool:
                    case AnimatorControllerParameterType.Trigger:
                        value = animator.GetBool(parameter.name);
                        break;
                    case AnimatorControllerParameterType.Float:
                        value = animator.GetFloat(parameter.name);
                        break;
                    case AnimatorControllerParameterType.Int:
                        value = animator.GetInteger(parameter.name);
                        break;
                    default:
                        continue;
                }

                parameters[parameter] = value;
            }

            animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;

            for (int i = 0; i < animator.layerCount; i++) {
                animator.Play(layerInfo[i].fullPathHash, i, layerInfo[i].normalizedTime);
            }

            foreach (KeyValuePair<AnimatorControllerParameter, object> parameterAndValue in parameters) {
                AnimatorControllerParameter parameter = parameterAndValue.Key;
                object value = parameterAndValue.Value;

                switch (parameter.type) {
                    case AnimatorControllerParameterType.Bool:
                        animator.SetBool(parameter.name, (bool)value);
                        break;
                    case AnimatorControllerParameterType.Float:
                        animator.SetFloat(parameter.name, (float)value);
                        break;
                    case AnimatorControllerParameterType.Int:
                        animator.SetInteger(parameter.name, (int)value);
                        break;
                    case AnimatorControllerParameterType.Trigger:
                        if ((bool)value) {
                            animator.SetTrigger(parameter.name);
                        }

                        break;
                    default:
                        continue;
                }
            }
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
