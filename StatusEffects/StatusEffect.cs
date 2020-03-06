using JusticeFramework.Data;
using System;
using UnityEngine;

namespace JusticeFramework.StatusEffects {
    /// <summary>
    /// Class that holds base functionality for all buffs and debuffs
    /// </summary>
    [Serializable]
    public abstract class StatusEffect {
        /// <summary>
        /// The time that this buff will last for
        /// </summary>
        [SerializeField]
        protected StatusEffectData model;

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
        
        public StatusEffect(StatusEffectData model, GameObject target, bool tickOnCreate = false) {
            this.model = model;
            this.target = target;

            remainingTime = model.DurationInSeconds;
            timeBetweenTicks = model.TickIntervalInSeconds;
            hasTicked = false;

            if (tickOnCreate) {
                Tick(0, true);
            }
        }

        public bool Tick(float deltaTime) {
            return Tick(deltaTime, false);
        }

        protected bool Tick(float deltaTime, bool force = false) {
            bool valid = true;
            bool shouldTick = false;

            remainingTime -= deltaTime;
            timeBetweenTicks += deltaTime;

            if (timeBetweenTicks >= model.TickIntervalInSeconds || force) {
                shouldTick = true;
                timeBetweenTicks = 0;
            }

            if (shouldTick) {
                hasTicked = true;
                Activate();
            }
            
            if (remainingTime <= 0 && !model.IsPersistent) {
                Disolve();
                valid = false;
            }

            return valid;
        }

        protected abstract void Activate();

        protected abstract void Disolve();
    }
}
