using JusticeFramework.Controllers;
using JusticeFramework.Data;
using System;
using UnityEngine;

namespace JusticeFramework.StatusEffects {
    [Serializable]
    public class SpeedBuff : StatusEffect {
        [SerializeField]
        private InputController controller;

        public SpeedBuff(StatusEffectData model, GameObject target) : base(model, target) {
            controller = target.GetComponent<InputController>();
        }
        
        protected override void Activate() {
            controller.crouchSpeedMod += model.Modifier;
            controller.walkSpeedMod += model.Modifier;
            controller.runSpeedMod += model.Modifier;
        }

        protected override void Disolve() {
            controller.crouchSpeedMod -= model.Modifier;
            controller.walkSpeedMod -= model.Modifier;
            controller.runSpeedMod -= model.Modifier;
        }
    }
}
