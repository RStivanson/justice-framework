using System;
using System.Collections;
using System.Collections.Generic;
using JusticeFramework.Components;
using JusticeFramework.Data.Dialogue;
using JusticeFramework.Utility.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
		[HideInInspector]
		private Actor currentTarget;

		[SerializeField]
		[HideInInspector]
		private List<Conversation> currentConversations;
		
		public void SetTarget(Actor target) {
			currentTarget = target;
			currentConversations = GameManager.DialogueManager.GetDialogue(currentTarget.Id);
			
			BuildResponseList();
		}
		
		private void BuildResponseList() {
			responsesContainer.DestroyAllChildren();

			currentConversations = GameManager.DialogueManager.GetDialogue(currentTarget.Id);

			if (currentConversations != null) {
				foreach (Conversation conversation in currentConversations) {
					foreach (Branch branch in conversation.branches) {
						Branch tmpBranch = branch;
						Topic topic = conversation.GetTopic(tmpBranch);

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
		
#region Events Callbacks

		private void OnTopicClicked(Topic topic, Branch branch) {
			Response response = topic.Responses[Random.Range(0, topic.Responses.Count)];

			float switchTime = 3;
			if (response.Voiceover != null) {
				switchTime = response.Voiceover.length + VOICEOVER_TIME_BUFFER;
				currentTarget.PlaySound(response.Voiceover);
			}

			npcText.text = response.ResponseText;
			
			// TODO : Add dialogue events
			//response.ProcessEvents(GameManager.Instance.Player);
			
			branch.Advance(response);
			if (!response.IsExitTrigger) {;
				BuildResponseList();
			}

			SetNpcTextVisible();
			StartCoroutine(OnSwitchTimeUp(switchTime, response.IsExitTrigger));
		}

		private IEnumerator OnSwitchTimeUp(float switchTime, bool exitOnSwitch) {
			float currentTime = 0;

			while (currentTime < switchTime) {
				currentTime += Time.unscaledDeltaTime;
				yield return 0;
			}

			if (exitOnSwitch) {
				Close();
			} else {
				BuildResponseList();
				SetResponseListVisible();
			}
		}

#endregion
	}
}
