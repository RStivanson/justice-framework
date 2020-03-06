using JusticeFramework.Components;
using JusticeFramework.Core;
using JusticeFramework.Core.Extensions;
using JusticeFramework.Core.UI;
using JusticeFramework.Data;
using JusticeFramework.Logic;
using JusticeFramework.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JusticeFramework.UI.Views {
    public delegate void OnDialogueResponseAction(DialogueResponseData response);

    [Serializable]
	public class DialogueView : Window {
		private const float VoiceoverTimeBuffer = 0.25f;

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
        private List<DialogueTree> dialogueTrees;
		
		public void SetTarget(Actor target) {
			currentTarget = target;			
			RefreshDialogueData();
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
            dialogueTrees = null;
			
			SetResponseListVisible();
		}

        private void RefreshDialogueData() {
            dialogueTrees = GameManager.DataManager.GetDialogueForActor(currentTarget.GetData<ActorData>());
            RefreshResponseButtonList();
        }

        private void RefreshResponseButtonList() {
            if (dialogueTrees == null) {
                return;
            }

            int buttonIndex = 0;
            int initialChildCount = responsesContainer.childCount;
            IDataObject self = GameManager.GetPlayer();

            foreach (DialogueTree dialogueTree in dialogueTrees) {
                DialogueTopicData currentTopic = dialogueTree.GetCurrentTopic();

                if (!Dialogue.MeetsConditions(currentTopic, self, currentTarget)) {
                    continue;
                }

                UnityAction callback = () => {
                    OnTopicSelected(dialogueTree, currentTopic);
                };

                if (buttonIndex < initialChildCount) {
                    UpdateResponseButton(responsesContainer.GetChild(buttonIndex).gameObject, currentTopic.DisplayText, callback);
                } else {
                    CreateNewResponseButton(currentTopic.DisplayText, callback);
                }

                buttonIndex++;
            }

            responsesContainer.DestroyAllChildren(buttonIndex);
        }

        private void UpdateResponseButton(GameObject btn, string text, UnityAction onClick) {
            btn.GetComponent<Button>().onClick.RemoveAllListeners();
            btn.GetComponent<Button>().onClick.AddListener(onClick);
            btn.GetComponentInChildren<Text>().text = text;
        }

        private void CreateNewResponseButton(string text, UnityAction onClick) {
            GameObject spawnedObject = Instantiate(responseButtonPrefab, responsesContainer, false);
            spawnedObject.GetComponent<Button>().onClick.AddListener(onClick);
            spawnedObject.GetComponentInChildren<Text>().text = text;
        }

        private DialogueResponseData SelectResponse(DialogueTopicData topic, IDataObject self) {
            List<DialogueResponseData> eligibleResponses = topic.DialogueResponses.Where(x => Dialogue.MeetsConditions(x, self, currentTarget)).ToList();
            DialogueResponseData result = null;

            switch (topic.ResponseSelectMethod) {
                case EResponseSelectMethod.Random:
                    int index = UnityEngine.Random.Range(0, eligibleResponses.Count - 1);
                    result = eligibleResponses[index];
                    break;
                case EResponseSelectMethod.FirstMatch:
                default:
                    result = eligibleResponses[0];
                    break;
            }

            return result;
        }

        private void OnTopicSelected(DialogueTree parent, DialogueTopicData topic) {
            IDataObject self = GameManager.GetPlayer();
            DialogueResponseData response = SelectResponse(topic, self);

            // Process the topic's events and update the dialogue tree
            GameEvent.ExecuteAll(topic.GameEventData, self, currentTarget);
            parent.SetCurrentTopic(response);

            if (topic.IsExitTrigger) {
                Close();
            }

            // Display the selected response
			npcText.text = response.DisplayText;
			float switchTime = 3;
			if (response.VoiceoverClip != null) {
				switchTime = response.VoiceoverClip.length + VoiceoverTimeBuffer;
				currentTarget.PlaySound(response.VoiceoverClip, EAudioType.Dialogue);
			}

			SetNpcTextVisible();
            GameEvent.ExecuteAll(response.GameEventData, self, currentTarget);
			StartCoroutine(OnSwitchTimeUp(switchTime, response));
		}

		private IEnumerator OnSwitchTimeUp(float switchTime, DialogueResponseData response) {
			float currentTime = 0;

			while (currentTime < switchTime) {
				currentTime += Time.unscaledDeltaTime;
				yield return 0;
			}

            if (response.IsExitTrigger) {
				Close();
			} else {
				RefreshDialogueData();
				SetResponseListVisible();
			}
		}
	}
}