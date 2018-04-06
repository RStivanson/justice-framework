using System;
using JusticeFramework.Data.Collections;

namespace JusticeFramework.Data.Models {
	/// <inheritdoc />
	/// <summary>
	/// Model class for all actors such as the player, and all NPCs.
	/// </summary>
	[Serializable]
	public class ActorModel : WorldObjectModel {
		/// <summary>
		/// The health of this actor before any modifiers
		/// </summary>
		public int baseHealth = 100;

		/// <summary>
		/// Determines if this actor can die
		/// </summary>
		public bool isInvincible = false;
		
		/// <summary>
		/// Items stored by this container
		/// </summary>
		public ItemList inventory = null;

		/// <summary>
		/// Determines how the actor will react to stronger opponents in combat
		/// </summary>
		public EBattleConfidence battleConfidence = EBattleConfidence.Average;

		/// <summary>
		/// Determines how aggressive the actor is
		/// </summary>
		public EAggression aggression = EAggression.Aggressive;

		/// <summary>
		/// Determines how the actor reacts to crime
		/// </summary>
		public EMorals morals = EMorals.NoCrime;

		/// <summary>
		/// Determines where the interest distance is calculated against
		/// </summary>
		public EInterestOrigin interestOrigin = EInterestOrigin.Self;
		
		/// <summary>
		/// The distance away where this actor will lose interest in pursuing its target. never = -1
		/// </summary>
		public float loseInterestDistance = 50;
	}
}