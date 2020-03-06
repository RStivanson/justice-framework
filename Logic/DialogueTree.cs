using JusticeFramework.Data;

namespace JusticeFramework.Logic {
    /// <summary>
    /// Runtime dialogue structure. This class handles traversing and storing position for associated dialogue data.
    /// </summary>
    public class DialogueTree {
        /// <summary>
        /// The dialogue that this tree represents.
        /// </summary>
        private DialogueData data;

        /// <summary>
        /// The index of the topic that this tree is currently at.
        /// </summary>
        private int currentTopicIndex;

        public DialogueTree(DialogueData dialogueData) {
            data = dialogueData;
            Reset();
        }

        /// <summary>
        /// Rests this dialogue tree back to the starting topic.
        /// </summary>
        public void Reset() {
            currentTopicIndex = 0;
        }

        /// <summary>
        /// Gets the topic this tree is currently pointing at.
        /// </summary>
        /// <returns>Returns the current topic.</returns>
        public DialogueTopicData GetCurrentTopic() {
            return data.Topics[currentTopicIndex];
        }

        /// <summary>
        /// Sets the trees current response based on the supplied dialogue response.
        /// </summary>
        /// <param name="response">The response to follow</param>
        public DialogueTopicData SetCurrentTopic(DialogueResponseData response) {
            currentTopicIndex = response.LinkToTopicIndex;
            return GetCurrentTopic();
        }
    }
}
