using JusticeFramework.Core.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeFramework.Core.Models.Dialogue {
	[Serializable]
	public class Topic {
		[SerializeField]
		private readonly int id;
		
		[SerializeField]
		private bool isDefault;
		
		[SerializeField]
		private string text;
		
		[SerializeField]
		private List<Response> responses;

        [SerializeField]
        private List<Condition> conditions;

		public int Id {
			get { return id; }
		}

		public bool IsDefault {
			get { return isDefault; }
			set { isDefault = value; }
		}

		public string TopicText {
			get { return text; }
			set { text = value; }
		}

		public List<Response> Responses {
			get { return responses; }
			set { responses = value; }
		}
		
		public Topic(int id) {
			this.id = id;
			
			isDefault = false;
			text = string.Empty;
			responses = new List<Response>();
        }

        /// <summary>
        /// Determines if the conditions for this response are met
        /// </summary>
        /// <param name="target">The target to check against</param>
        /// <returns>Return true if the conditions are met or empty, false otherwise</returns>
        public bool MeetsConditions(IEntity self, IEntity target) {
            bool result = true;

            // If the target is not null and the conditions arent null
            if (target != null && conditions != null) {
                // Evaluate each condition
                foreach (Condition condition in conditions) {
                    result &= condition.Evaluate(self, target);
                }

                // Validate that at least one response on this topic meets the conditions
                bool atLeastOneResponseMet = false;
                for (int i = 0; i < responses.Count && !atLeastOneResponseMet; i++) {
                    atLeastOneResponseMet = responses[i].MeetsConditions(self, target);
                }

                result &= atLeastOneResponseMet;
            }

            return result;
        }
    }
}