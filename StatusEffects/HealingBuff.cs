using JusticeFramework.Components;
using JusticeFramework.Data;
using System;
using UnityEngine;

namespace JusticeFramework.StatusEffects {
    [Serializable]
    public class HealingBuff : StatusEffect {
        [SerializeField]
        private Actor actor;

        public HealingBuff(StatusEffectData model, GameObject target) : base(model, target) {
            actor = target.GetComponent<Actor>();
        }

        protected override void Activate() {
            actor.Heal(null, model.Modifier);
        }

        protected override void Disolve() {
        }
    }
}
