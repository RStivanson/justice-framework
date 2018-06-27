using JusticeFramework.Core.Managers;
using JusticeFramework.Core.Interfaces;
using System;
using UnityEngine;

namespace JusticeFramework.Core {
    /// <summary>
    /// Class that holds functionality and data regarding a game event such as advancing quests and giving items
    /// </summary>
    [Serializable]
    public class GameEvent {
        /// <summary>
        /// The type of event
        /// </summary>
        [SerializeField]
        private EGameEventType eventType;

        /// <summary>
        /// The item to give the target
        /// </summary>
        [SerializeField]
        private string itemId;

        /// <summary>
        /// The amount of the item to give the target
        /// </summary>
        [SerializeField]
        private int itemQuantity;

        /// <summary>
        /// The quest Id to advance
        /// </summary>
        [SerializeField]
        private string questId;

        /// <summary>
        /// The stage to advance the quest to
        /// </summary>
        [SerializeField]
        private int questStage;

        /// <summary>
        /// Processes the event against the given target
        /// </summary>
        /// <param name="target">The actor to target</param>
		public void Execute(IActor self, IActor target) {
            switch (eventType) {
                case EGameEventType.AdvanceQuest:
                    ProcessAdvanceQuest(target);
                    break;
                case EGameEventType.GiveItem:
                    ProcessGiveItem(target);
                    break;
                case EGameEventType.TakeItem:
                    ProcessTakeItem(target);
                    break;
#if UNITY_EDITOR
                default:
                    Debug.Log("GameEvent - Unknown event type: " + eventType);
                    break;
#endif
            }
        }

        /// <summary>
        /// Processes the give item event on the target
        /// </summary>
        /// <param name="target">The actor to target</param>
        private void ProcessGiveItem(IActor target) {
            target.GiveItem(itemId, itemQuantity);
        }

        /// <summary>
        /// Processes the take item event on the target
        /// </summary>
        /// <param name="target">The actor to target</param>
        private void ProcessTakeItem(IActor target) {
            target.TakeItem(itemId, itemQuantity);
        }

        /// <summary>
        /// Processes the advance quest on the target
        /// </summary>
        /// <param name="target">The actor to target</param>
        private void ProcessAdvanceQuest(IActor target) {
            GameManager.QuestManager.SetQuestStage(questId, questStage);
        }
    }
}