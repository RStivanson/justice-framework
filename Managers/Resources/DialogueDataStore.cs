using JusticeFramework.Core;
using JusticeFramework.Core.Extensions;
using JusticeFramework.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeFramework.Managers
{
    [Serializable]
    public class DialogueDataStore : ResourceStore<DialogueData> {
        /// <summary>
        /// A dictionary of dialogue trees mapped by faction ID.
        /// </summary>
        [SerializeField]
        private Dictionary<string, List<DialogueData>> factionDialogueByTargetId;

        /// <summary>
        /// A dictionary of dialogue trees mapped by actor ID.
        /// </summary>
        [SerializeField]
        private Dictionary<string, List<DialogueData>> dialogueByTargetId;

        /// <inheritdoc />
        public DialogueDataStore() : base() {
            factionDialogueByTargetId = new Dictionary<string, List<DialogueData>>();
            dialogueByTargetId = new Dictionary<string, List<DialogueData>>();
        }

        /// <inheritdoc />
        protected override void OnPreProcess() {
            factionDialogueByTargetId.Clear();
            dialogueByTargetId.Clear();
        }

        /// <inheritdoc />
        protected override void OnResourceProcessed(DialogueData model) {
            if (model.TargetFaction != null) {
                factionDialogueByTargetId.AddToList(model.TargetFaction.Id, model);
            }

            if (model.TargetActor != null) {
                dialogueByTargetId.AddToList(model.TargetActor.Id, model);
            }
        }

        /// <summary>
        /// Gets the dialogue associated with the given target actor ID.
        /// </summary>
        /// <param name="id">The ID of the target actor.</param>
        /// <returns>Returns a list of all dialogue associated to the given target actor, or an empty list if none.</returns>
        public List<DialogueData> GetDialogueByTargetId(string id) {
            dialogueByTargetId.TryGetValue(id, out List<DialogueData> tempList);
            return tempList ?? new List<DialogueData>();
        }

        /// <summary>
        /// Gets the dialogue associated with the given faction ID.
        /// </summary>
        /// <param name="id">The ID of the faction.</param>
        /// <returns>Returns a list of all dialogue associated to the given faction, or an empty list if none.</returns>
        public List<DialogueData> GetDialogueByFactionId(string id) {
            factionDialogueByTargetId.TryGetValue(id, out List<DialogueData> tempList);
            return tempList ?? new List<DialogueData>();
        }
    }
}