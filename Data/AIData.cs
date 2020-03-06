using UnityEngine;

namespace JusticeFramework.Data {
    [CreateAssetMenu(menuName = "Justice Framework/AI Data", order = 50)]
    public class AiData : ScriptableDataObject {
        /// <summary>
		/// Determines how the AI will react to opponents in combat.
		/// </summary>
		[SerializeField]
        private EBattleConfidence battleConfidence = EBattleConfidence.Average;

        /// <summary>
        /// Determines how aggressive the AI is.
        /// </summary>
        [SerializeField]
        private EAggression aggression = EAggression.Aggressive;

        /// <summary>
        /// Determines how the AI reacts to crime.
        /// </summary>
        [SerializeField]
        private EMorals morals = EMorals.NoCrime;

        /// <summary>
        /// Determines where the interest distance is calculated against.
        /// </summary>
        [SerializeField]
        private EInterestOrigin interestOrigin = EInterestOrigin.Self;

        /// <summary>
        /// The distance away where this AI will lose interest in pursuing its target. Never = -1.
        /// </summary>
        [SerializeField]
        private float loseInterestDistanceSqr = 50;

        /// <summary>
        /// Gets the confidence level. Defines how the AI will react to opponent in combat.
        /// </summary>
        public EBattleConfidence Confidence {
            get { return battleConfidence; }
        }

        /// <summary>
        /// Gets the aggression level. Defines how the AI will react to other AI when not in combat.
        /// </summary>
        public EAggression Aggression {
            get { return aggression; }
        }

        /// <summary>
        /// Gets the morals level. Defines how the AI will react to witnessing crime.
        /// </summary>
        public EMorals Morals {
            get { return morals; }
        }

        /// <summary>
        /// Gets where the AI's origin of interest is calculated against.
        /// </summary>
        public EInterestOrigin InterestOrigin {
            get { return interestOrigin; }
        }

        /// <summary>
        /// Gets the square distance from the origin that AI should lose interest.
        /// </summary>
        public float LoseInterestDistanceSqr {
            get { return loseInterestDistanceSqr; }
        }
    }
}
