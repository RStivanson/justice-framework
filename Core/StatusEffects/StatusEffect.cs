using JusticeFramework.Core.Models;
using System;
using UnityEngine;

namespace JusticeFramework.Core.StatusEffects {
    /// <summary>
    /// Class that holds base functionality for all buffs and debuffs
    /// </summary>
    [Serializable]
    public abstract class StatusEffect {
        /// <summary>
        /// The time that this buff will last for
        /// </summary>
        [SerializeField]
        protected StatusEffectModel model;

        /// <summary>
        /// The gameobject being targeted by this effect
        /// </summary>
        [SerializeField]
        protected GameObject target;

        /// <summary>
        /// The remaining time until the buff is disolved
        /// </summary>
        [SerializeField]
        protected float remainingTime;

        /// <summary>
        /// The time elapsed between ticks
        /// </summary>
        [SerializeField]
        private float timeBetweenTicks;

        /// <summary>
        /// Flag indicating if this status effect has ticked once
        /// </summary>
        [SerializeField]
        private bool hasTicked;
        
        public float RemainingTime {
            get { return remainingTime; }
        }
        
        public StatusEffect(StatusEffectModel model, GameObject target) {
            this.model = model;
            this.target = target;

            remainingTime = model.duration;
            timeBetweenTicks = model.tickInterval;
            hasTicked = false;
        }

        public virtual bool Tick(float deltaTime) {
            bool valid = true;
            bool shouldTick = false;

            remainingTime -= deltaTime;
            timeBetweenTicks += deltaTime;

            if (timeBetweenTicks >= model.tickInterval) {
                shouldTick = true;
                timeBetweenTicks = 0;
            }

            if ((shouldTick && !model.isSingleTick) || (model.isSingleTick && !hasTicked)) {
                hasTicked = true;
                Activate();
            }

            if (remainingTime <= 0 && !model.isPersistant) {
                Disolve();
                valid = false;
            }

            return valid;
        }

        private void OnTick() {
            Activate();

            if (model.isSingleTick || (remainingTime <= 0 && !model.isPersistant)) {
                Disolve();
            }
        }

        protected abstract void Activate();

        protected abstract void Disolve();
    }
}
