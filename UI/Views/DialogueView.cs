using JusticeFramework.Components;
using JusticeFramework.Core.Managers;
using JusticeFramework.Core.Models.Dialogue;
using JusticeFramework.Core.UI;
using JusticeFramework.Utility.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JusticeFramework.UI.Views {
    [Serializable]
	public class DialogueView : Window {
		private const float VOICEOVER_TIME_BUFFER = 0.25f;

		[SerializeField]
		private Text npcText;

		[SerializeField]
		private Transform responsesContainer;

		[SerializeField]
		private GameObject responseListParent;

		[SerializeField]
		private GameObject responseButtonPrefab;

		[SerializeField]
		private Actor currentTarget;

		[SerializeField]
		[HideInInspector]
		private List<Conversation> currentConversations;
		
		public void SetTarget(Actor target) {
			currentTarget = target;
			currentConversations = GameManager.DialogueManager.GetDialogue(currentTarget.Id);
			
			BuildResponseList();
		}
		
		private void SetResponseListVisible() {
			responseListParent.gameObject.SetActive(true);
			npcText.gameObject.SetActive(false);
		}
		
		private void SetNpcTextVisible() {
			responseListParent.gameObject.SetActive(false);
			npcText.gameObject.SetActive(true);
		}
		
		protected override void OnShow() {
			currentTarget = null;
			currentConversations = null;
			
			SetResponseListVisible();
		}

        #region Response List

        private void BuildResponseList() {
            responsesContainer.DestroyAllChildren();

            currentConversations = GameManager.DialogueManager.GetDialogue(currentTarget.Id);
            PopulateResponseList();
        }

        private void PopulateResponseList() {
            if (currentConversations == null) {
                return;
            }

            foreach (Conversation conversation in currentConversations) {
                foreach (Branch branch in conversation.branches) {
                    Branch tmpBranch = branch;
                    Topic topic = conversation.GetTopic(tmpBranch);

                    if (topic.MeetsConditions(GameManager.Player, currentTarget)) {
                        AddResponse(topic.TopicText, delegate { OnTopicClicked(topic, tmpBranch); });
                    }
                }
            }
        }

        private void AddResponse(string dialogueString, UnityAction onClick) {
            GameObject spawnedObject = Instantiate(responseButtonPrefab);

            spawnedObject.GetComponent<Button>().onClick.AddListener(onClick);
            spawnedObject.GetComponentInChildren<Text>().text = dialogueString;

            spawnedObject.transform.SetParent(responsesContainer, false);
        }

        #endregion

        #region Events Callbacks

        private void OnTopicClicked(Topic topic, Branch branch) {
            Response response = null;
            for (int i = 0; i < topic.Responses.Count; i++) {
                if (topic.Responses[i].MeetsConditions(GameManager.Player, currentTarget)) {
                    response = topic.Responses[i];
                }
            }

			float switchTime = 3;
			if (response.Voiceover != null) {
				switchTime = response.Voiceover.length + VOICEOVER_TIME_BUFFER;
				currentTarget.PlaySound(response.Voiceover);
			}

			npcText.text = response.ResponseText;
			
			branch.Advance(response);
			if (!response.IsExitTrigger) {;
				BuildResponseList();
			}

			SetNpcTextVisible();
			StartCoroutine(OnSwitchTimeUp(switchTime, response));
		}

		private IEnumerator OnSwitchTimeUp(float switchTime, Response response) {
			float currentTime = 0;

			while (currentTime < switchTime) {
				currentTime += Time.unscaledDeltaTime;
				yield return 0;
			}

            response.ProcessEvents(currentTarget, GameManager.Player);

			if (response.IsExitTrigger) {
				Close();
			} else {
				BuildResponseList();
				SetResponseListVisible();
			}
		}

#endregion
	}
}
