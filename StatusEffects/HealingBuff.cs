using JusticeFramework.Components;
using JusticeFramework.Core.Models;
using JusticeFramework.Core.StatusEffects;
using System;
using UnityEngine;

namespace JusticeFramework.StatusEffects {
    [Serializable]
    public class HealingBuff : StatusEffect {
        [SerializeField]
        private Actor actor;

        public HealingBuff(StatusEffectModel model, GameObject target) : base(model, target) {
            actor = target.GetComponent<Actor>();
        }

        protected override void Activate() {
            actor.Heal(null, model.modifier);
        }

        protected override void Disolve() {
        }
    }
}
