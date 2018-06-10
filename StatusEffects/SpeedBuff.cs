using JusticeFramework.Core.Controllers;
using JusticeFramework.Core.Models;
using JusticeFramework.Core.StatusEffects;
using System;
using UnityEngine;

namespace JusticeFramework.StatusEffects {
    [Serializable]
    public class SpeedBuff : StatusEffect {
        [SerializeField]
        private InputController controller;

        public SpeedBuff(StatusEffectModel model, GameObject target) : base(model, target) {
            controller = target.GetComponent<InputController>();
        }
        
        protected override void Activate() {
            controller.crouchSpeedMod += model.modifier;
            controller.walkSpeedMod += model.modifier;
            controller.runSpeedMod += model.modifier;
        }

        protected override void Disolve() {
            controller.crouchSpeedMod -= model.modifier;
            controller.walkSpeedMod -= model.modifier;
            controller.runSpeedMod -= model.modifier;
        }
    }
}
