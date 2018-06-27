using JusticeFramework.Core.Interfaces;
using JusticeFramework.Core.Managers;
using System;
using UnityEngine;

namespace JusticeFramework.Core {
    /// <summary>
    /// Data class used to signifiy conditions for execution based on game data
    /// </summary>
    [Serializable]
    public class Condition {
        /// <summary>
        /// The method used to evaluate the condition
        /// </summary>
        [SerializeField]
        private EConditionMethod conditionMethod;

        /// <summary>
        /// The type of equality check to perform
        /// </summary>
        [SerializeField]
        private EEqualityType equalityType;

        /// <summary>
        /// The value to compare against
        /// </summary>
        [SerializeField]
        private string stringValue = "";

        /// <summary>
        /// The value to compare against
        /// </summary>
        [SerializeField]
        private float floatValue = 1;

        /// <summary>
        /// Flag indicating if the condition should target self instead of the target
        /// </summary>
        [SerializeField]
        private bool targetSelf = false;

        /// <summary>
        /// Evaluates the conditions
        /// </summary>
        /// <returns>Returns true if the conition is true, false otherwise</returns>
        public bool Evaluate(IEntity self, IEntity target) {
            bool result = false;

            switch (conditionMethod) {
                case EConditionMethod.IsId:
                    if (targetSelf) {
                        result = Compare(self.Id);
                    } else {
                        result = Compare(target.Id);
                    }
                    break;
                case EConditionMethod.HasItem:
                    IContainer container = null;

                    if (targetSelf) {
                        container = self as IContainer;
                    } else {
                        container = target as IContainer;
                    }

                    if (container != null) {
                        result = Compare(container.GetQuantity(stringValue));
                    }
                    break;
                case EConditionMethod.GetQuestStage:
                    result = Compare(GameManager.QuestManager.GetQuestStage(stringValue));
                    break;
                case EConditionMethod.GetQuestCompleted:
                    result = Compare(GameManager.QuestManager.IsQuestAtState(stringValue, EQuestState.Completed));
                    break;
            }

            return result;
        }

        private bool Compare(string targetValue) {
            bool result = false;

            switch (equalityType) {
                case EEqualityType.Equal:
                    result = targetValue.Equals(stringValue);
                    break;
                case EEqualityType.NotEqual:
                    result = !targetValue.Equals(stringValue);
                    break;
            }

            return result;
        }

        private bool Compare(bool targetValue) {
            return Compare(targetValue ? 1 : 0);
        }

        private bool Compare(float targetValue) {
            bool result = false;

            switch (equalityType) {
                case EEqualityType.Equal:
                    result = targetValue == floatValue;
                    break;
                case EEqualityType.NotEqual:
                    result = targetValue != floatValue;
                    break;
                case EEqualityType.LessThanOrEqual:
                    result = targetValue <= floatValue;
                    break;
                case EEqualityType.LessThan:
                    result = targetValue < floatValue;
                    break;
                case EEqualityType.GreaterThanOrEqual:
                    result = targetValue >= floatValue;
                    break;
                case EEqualityType.GreaterThan:
                    result = targetValue > floatValue;
                    break;
            }

            return result;
        }
    }
}
