using JusticeFramework.Core;
using JusticeFramework.Core.Extensions;
using JusticeFramework.Data;
using JusticeFramework.Interfaces;
using JusticeFramework.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeFramework.Logic {
    public static class Condition {
        /// <summary>
        /// Evaluates the condition type.
        /// </summary>
        /// <returns>Returns true if the condition is passes, false otherwise.</returns>
        public static bool EvaluateAll(IEnumerable<ConditionData> data, IDataObject self, IDataObject target) {
            foreach (ConditionData conditionData in data) {
                if (!Evaluate(conditionData, self, target)) {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Evaluates the condition type.
        /// </summary>
        /// <returns>Returns true if the condition is passes, false otherwise.</returns>
        public static bool Evaluate(ConditionData data, IDataObject self, IDataObject target) {
            bool result = false;
            IDataObject realTarget = data.ShouldTargetSelf ? self : target;

            switch (data.ConditionType) {
                case EConditionType.GetIsId:
                    result = PerformComparison(data.EqualityType, realTarget.Id, data.StringValue);
                    break;
                case EConditionType.GetHasItem:
                    if (realTarget is IContainer container) {
                        result = PerformComparison(data.EqualityType, container.Inventory.GetQuantity(data.StringValue), data.FloatValue);
                    }
                    break;
                case EConditionType.GetQuestStage:
                    result = false;//PerformComparison(data.EqualityType, GameManager.DataManager.GetQuestStage(data.StringValue), data.FloatValue);
                    break;
                case EConditionType.GetQuestCompleted:
                    result = false;// PerformComparison(data.EqualityType, GameManager.DataManager.IsQuestAtState(data.StringValue, EQuestState.Completed), data.FloatValue);
                    break;
                default:
                    Debug.Log("Condition - Unknown condition type: " + data.ConditionType);
                    break;
            }

            return result;
        }

        /// <summary>
        /// Performs a comparision check on the given string values.
        /// </summary>
        /// <param name="equalityType">Type type of equality check to perform</param>
        /// <param name="calculatedValue">The calculated value of this condition</param>
        /// <param name="targetValue">The value we are comparing the calculated value against</param>
        /// <returns>Returns true if the values match the given equality type, false otherwise.</returns>
        private static bool PerformComparison(EEqualityType equalityType, string calculatedValue, string targetValue) {
            bool result = false;

            switch (equalityType) {
                case EEqualityType.Equal:
                    result = calculatedValue.EqualsOrdinal(targetValue);
                    break;
                case EEqualityType.NotEqual:
                    result = !calculatedValue.EqualsOrdinal(targetValue);
                    break;
            }

            return result;
        }

        /// <summary>
        /// Performs a comparision check on the given bool values.
        /// </summary>
        /// <param name="equalityType">Type type of equality check to perform</param>
        /// <param name="calculatedValue">The calculated value of this condition</param>
        /// <param name="targetValue">The value we are comparing the calculated value against</param>
        /// <returns>Returns true if the values match the given equality type, false otherwise.</returns>
        private static bool PerformComparison(EEqualityType equalityType, bool calculatedValue, float targetValue) {
            bool result = false;

            switch (equalityType) {
                case EEqualityType.Equal:
                    result = calculatedValue == (targetValue == 1);
                    break;
                case EEqualityType.NotEqual:
                    result = calculatedValue != (targetValue == 1);
                    break;
            }

            return result;
        }

        /// <summary>
        /// Performs a comparision check on the given float values.
        /// </summary>
        /// <param name="equalityType">Type type of equality check to perform</param>
        /// <param name="calculatedValue">The calculated value of this condition</param>
        /// <param name="targetValue">The value we are comparing the calculated value against</param>
        /// <returns>Returns true if the values match the given equality type, false otherwise.</returns>
        private static bool PerformComparison(EEqualityType equalityType, float calculatedValue, float targetValue) {
            bool result = false;

            switch (equalityType) {
                case EEqualityType.Equal:
                    result = calculatedValue == targetValue;
                    break;
                case EEqualityType.NotEqual:
                    result = calculatedValue != targetValue;
                    break;
                case EEqualityType.LessThanOrEqual:
                    result = calculatedValue <= targetValue;
                    break;
                case EEqualityType.LessThan:
                    result = calculatedValue < targetValue;
                    break;
                case EEqualityType.GreaterThanOrEqual:
                    result = calculatedValue >= targetValue;
                    break;
                case EEqualityType.GreaterThan:
                    result = calculatedValue > targetValue;
                    break;
            }

            return result;
        }
    }
}
