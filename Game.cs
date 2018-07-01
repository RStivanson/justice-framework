using JusticeFramework.Components;
using JusticeFramework.Core.AI.BehaviourTree;
using JusticeFramework.Core.Interfaces;
using JusticeFramework.Core.Managers;
using JusticeFramework.UI.Views;
using JusticeFramework.Utility.Extensions;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : GameManager {
	private bool isConsoleOpen = false;

	private BehaviourTree tree;
	public AudioClip ambientMusic;
	
	protected override void OnInitialized() {
		SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
		UiManager.UI.OpenWindow<MainMenuView>();
	}

	private void Update() {
		if (!IsPlaying) {
			return;
		}

		if (Input.GetKeyDown(KeyCode.H)) {
			QualitySettings.vSyncCount = QualitySettings.vSyncCount == 0 ? 1 : 0;
		}

		if (Input.GetKeyDown(KeyCode.J)) {
			Application.targetFrameRate = Application.targetFrameRate == 60 ? 0 : 60;
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
            player.Unequip(JusticeFramework.Core.EEquipSlot.Mainhand);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (IsPlaying && UiManager.UI.Peek() is HudView) {
                UiManager.UI.OpenWindow<SystemMenuView>();
            } else {
                UiManager.UI.CloseTop();
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (IsPlaying && UiManager.UI.Peek() is HudView) {
                ContainerView view = UiManager.UI.OpenWindow<ContainerView>();
                view.View(Player, null);
            }
        }

        if (Input.GetKeyDown(KeyCode.BackQuote)) {
            if (UiManager.UI.Peek().NotType<ConsoleView>()) {
                ConsoleView view = UiManager.UI.OpenWindow<ConsoleView>();
                view.SetCommandLibrary(GameManager.CommandLibrary);
            }
        }

        gameTime = gameTime.AddSeconds(15);
	}

    protected override void OnLevelLoaded() {
        // Close the loading screen, and open the HUD
        UiManager.UI.CloseTop(true);
        UiManager.UI.OpenWindow<HudView>().SetPlayer(Player as Actor);
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

        GetComponent<AudioSource>().clip = ambientMusic;
		GetComponent<AudioSource>().Play();
		CommandLibrary.SetInteractionController(playerTrans.GetComponent<IInteractionController>());
		
		Unpause();
	}

	public override void EndGame() {
		IsPlaying = false;
			
		Destroy(Player.Transform.gameObject);
	}

#endregion
}
