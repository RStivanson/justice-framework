using JusticeFramework;
using JusticeFramework.Components;
using JusticeFramework.Core;
using JusticeFramework.Core.AI.BehaviourTree;
using JusticeFramework.Core.Interfaces;
using JusticeFramework.Core.Managers;
using JusticeFramework.Core.Models.Settings;
using JusticeFramework.UI.Views;
using JusticeFramework.Utility.Extensions;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : GameManager {
	private BehaviourTree tree;
	public AudioClip ambientMusic;
	
	protected override void OnInitialized() {
		SceneManager.LoadScene(SystemConstants.SettingMainMenuScene, LoadSceneMode.Additive);
		UiManager.UI.OpenWindow<MainMenuView>();
	}

	private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (IsPlaying) {
                if (UiManager.UI.Peek() is HudView) {
                    UiManager.UI.OpenWindow<SystemMenuView>();
                } else {
                    UiManager.UI.CloseTop();
                }
            } else {
                if (!(UiManager.UI.Peek() is MainMenuView)) {
                    UiManager.UI.CloseTop();
                }
            }
        }

        if (!IsPlaying) {
			return;
		}

		if (Input.GetKeyDown(KeyCode.K)) {
			Cursor.lockState = CursorLockMode.Locked;
		}

        if (Input.GetKeyDown(KeyCode.L)) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetKeyDown(KeyCode.U)) {
            Actor player = Player as Actor;
            Actor.Unequip(player, player.Inventory, EEquipSlot.Mainhand);
        }

        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (UiManager.UI.Peek() is HudView) {
                ContainerView view = UiManager.UI.OpenWindow<ContainerView>();
                view.View(Player, null, targetMask: EContainerViewMask.Items);
            }
        }

        if (Input.GetKeyDown(KeyCode.F)) {
            if (UiManager.UI.Peek() is HudView) {
                CraftingView crafting = UiManager.UI.OpenWindow<CraftingView>();
                crafting.View(Player);
            }
        }

        if (Input.GetKeyDown(KeyCode.BackQuote)) {
            if (UiManager.UI.Peek().NotType<ConsoleView>()) {
                ConsoleView view = UiManager.UI.OpenWindow<ConsoleView>();
                view.SetCommandLibrary(CommandLibrary);
            }
        }

        gameTime = gameTime.AddSeconds(15);
	}

    #region Event Callbacks

    protected override void OnLevelLoaded() {
        base.OnLevelLoaded();

        // Close the loading screen, and open the HUD
        UiManager.UI.CloseTop(true);
        UiManager.UI.OpenWindow<HudView>().SetPlayer(Player as Actor);
    }

    protected override void OnQuestUpdated(string id, int marker) {
        base.OnQuestUpdated(id, marker);

        string questDisplayName = QuestManager.GetQuestDisplayName(id);
        Notify($"{questDisplayName} updated");
    }

    #endregion

    public static void Notify(string notification) {
        HudView hud = UiManager.UI.GetWindow<HudView>();

        if (hud != null) {
            hud.ShowNotification(notification);
        }
    }

    #region Game State Methods

    public override void BeginGame() {
		LoadLevel("overworld", OnPostBeginGame);	
	}

	private void OnPostBeginGame() {
		gameTime = new DateTime(1, 1, 1, 7, 0, 0);

        Transform playerTrans = Player.Transform;

        playerTrans.gameObject.SetActive(true);
        playerTrans.position = new Vector3(-719.5f, 50, -53.25f);

        ambientAudioSource.clip = ambientMusic;
        ambientAudioSource.Play();
		CommandLibrary.SetInteractionController(playerTrans.GetComponent<IInteractionController>());
		
		Unpause();
	}

	public override void EndGame() {
		IsPlaying = false;
			
		Destroy(Player.Transform.gameObject);
	}

#endregion
}
