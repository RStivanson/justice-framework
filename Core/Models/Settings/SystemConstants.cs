using UnityEngine;

namespace JusticeFramework.Core.Models.Settings {
    public static class SystemConstants {
        public const string AnimatorIdleState = "Idle";
        public const string AnimatorCombatIdleState = "CombatIdle";
        public const string AnimatorWalkState = "Walk";
        public const string AnimatorAttackState = "Attack";
        public const string AnimatorIsWalkingParam = "IsWalking";
        public const string AnimatorIsInCombatParam = "IsInCombat";
        public const string AnimatorAttackParam = "Attack";

        public const string AssetDataPlayerId = "ActorPlayer";

        public const string ItemCurrencyId = "ItemGold";

        public const string InputMouseX = "Mouse X";
        public const string InputMouseY = "Mouse Y";
        public const string InputHorizontal = "Horizontal";
        public const string InputVertical = "Vertical";
        
        public static readonly string SavePath = Application.persistentDataPath;
        public const string SettingsFileName = "settings.json";

        public const string SettingMainMenuScene = "MainMenu";
        public const string SettingMainScene = "MainScene";
    }
}
