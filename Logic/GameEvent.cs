using JusticeFramework.Core;
using JusticeFramework.Data;
using JusticeFramework.Interfaces;
using JusticeFramework.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeFramework.Logic {
    /// <summary>
    /// Handles the logic for executing all defined game events.
    /// TODO : This should be replaced with LUA scripts instead, or something of the like.
    /// </summary>
    public static class GameEvent {
        /// <summary>
        /// Executes all events in the given set.
        /// </summary>
        /// <param name="gameEventList">The list of game events to execute.</param>
        /// <param name="self">The originating source of the event.</param>
        /// <param name="target">The target of the event. Likely the player.</param>
        public static void ExecuteAll(IEnumerable<GameEventData> gameEventList, IDataObject self, IDataObject target) {
            foreach (GameEventData gameEventData in gameEventList) {
                Execute(gameEventData, self, target);
            }
        }

        /// <summary>
        /// Executes the given event.
        /// </summary>
        /// <param name="gameEvent">The event to execute.</param>
        /// <param name="self">The originating source of the event.</param>
        /// <param name="target">The target of the event. Likely the player.</param>
        public static void Execute(GameEventData gameEvent, IDataObject self, IDataObject target) {
            IDataObject realTarget = gameEvent.ShouldTargetSelf ? self : target;

            switch (gameEvent.EventType) {
                case EGameEventType.SetQuestStage:
                    ProcessSetQuestStage(gameEvent, realTarget);
                    break;
                case EGameEventType.GiveItem:
                    ProcessGiveItem(gameEvent, realTarget);
                    break;
                case EGameEventType.TakeItem:
                    ProcessTakeItem(gameEvent, realTarget);
                    break;
#if UNITY_EDITOR
                default:
                    Debug.Log("GameEvent - Unknown event type: " + gameEvent.EventType);
                    break;
#endif
            }
        }

        /// <summary>
        /// Sets the stage of the given quest to the new stage marker.
        /// </summary>
        /// <param name="gameEvent">The event data being executed.</param>
        /// <param name="target">The target of the event.</param>
        public static void ProcessSetQuestStage(GameEventData gameEvent, IDataObject target) {
            //GameManager.DataManager.SetQuestStage(gameEvent.TargetId, gameEvent.Value);
        }

        /// <summary>
        /// Gives the target the item defined by the event, if the target is a conainter.
        /// </summary>
        /// <param name="gameEvent">The event data being executed.</param>
        /// <param name="target">The target of the event.</param>
        public static void ProcessGiveItem(GameEventData gameEvent, IDataObject target) {
            if (target is IContainer container) {
                container.Inventory.Add(gameEvent.TargetId, gameEvent.Value);
            }
        }

        /// <summary>
        /// Takes the item defined by the event from the target, if the target is a conainter.
        /// </summary>
        /// <param name="gameEvent">The event data being executed.</param>
        /// <param name="target">The target of the event.</param>
        public static void ProcessTakeItem(GameEventData gameEvent, IDataObject target) {
            if (target is IContainer container) {
                container.Inventory.Remove(gameEvent.TargetId, gameEvent.Value);
            }
        }
    }
}
