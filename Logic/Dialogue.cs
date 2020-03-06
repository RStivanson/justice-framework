using JusticeFramework.Core;
using JusticeFramework.Data;

namespace JusticeFramework.Logic {
    public static class Dialogue {
        public static bool MeetsConditions(DialogueTopicData data, IDataObject self, IDataObject target) {
            bool result = true;

            // Does this topic meet the conditions?
            foreach (ConditionData conditionData in data.ConditionData) {
                result &= Condition.Evaluate(conditionData, self, target);
            }

            return result;
        }

        public static bool MeetsConditionsAndAtleastOneResponse(DialogueTopicData data, IDataObject self, IDataObject target) {
            bool result = true;

            // Does this topic meet the conditions?
            foreach (ConditionData conditionData in data.ConditionData) {
                result &= Condition.Evaluate(conditionData, self, target);
            }

            // Does at least one response all meet their conditions?
            bool atLeastOneResponseMet = false;
            for (int i = 0; i < data.DialogueResponses.Length && !atLeastOneResponseMet; i++) {
                atLeastOneResponseMet = MeetsConditions(data.DialogueResponses[i], self, target);
            }

            return result && atLeastOneResponseMet;
        }

        public static bool MeetsConditions(DialogueResponseData data, IDataObject self, IDataObject target) {
            bool result = true;

            foreach (ConditionData conditionData in data.ConditionData) {
                result &= Condition.Evaluate(conditionData, self, target);
            }

            return result;
        }
    }
}
