namespace JusticeFramework.Logic {
    public static class EngineSettings {
        // Game tag declarations
        public const string TagItemTypeArmor = "ItemTypeArmor";
        public const string TagItemTypeWeapon = "ItemTypeWeapon";
        public const string TagItemTypeCodex = "ItemTypeCodex";
        public const string TagItemTypePotion = "ItemTypePotion";
        public const string TagItemTypeFood = "ItemTypeFood";
        public const string TagItemTypeAmmo = "ItemTypeAmmo";
        public const string TagItemTypeJewelry = "ItemTypeJewelry";
        public const string TagCraftCatBronze = "CraftCatBronze";
        public const string TagCraftCatIron = "CraftCatIron";
        public const string TagCraftCatSteel = "CraftCatSteel";
        public const string TagCraftCatMithril = "CraftCatMithril";
        public const string TagCraftCatAdamant = "CraftCatAdamant";
        public const string TagCraftCatPotion = "CraftCatPotion";

        // Game setting declarations
        public const string SettingHeaderAdamant = "Adamant";
        public const string SettingHeaderAmmo = "Ammo";
        public const string SettingHeaderArmor = "Armor";
        public const string SettingHeaderBronze = "Bronze";
        public const string SettingHeaderCodexes = "Books";
        public const string SettingHeaderEquipped = "Equipped";
        public const string SettingHeaderFood = "Food";
        public const string SettingHeaderPotion = "Potion";
        public const string SettingHeaderIron = "Iron";
        public const string SettingHeaderJewelry = "Jewelry";
        public const string SettingHeaderMisc = "Misc.";
        public const string SettingHeaderMithril = "Mithril";
        public const string SettingHeaderSteel = "Steel";
        public const string SettingHeaderWeapons = "Weapons";
        public const string SettingIDPlayer = "ActorPlayer";
        public const string SettingMsgItemCrafted = "{0} crafted!";
        public const string SettingMsgNotEnoughIngredients = "Not enough ingredients to craft {0}!";
        public const string SettingMsgUnknownRecipe = "Unnkown recipe.";
        public const string SettingSceneMainMenu = "MainMenu";
        public const string SettingSceneMainScene = "MainScene";
        public const string SettingSceneNewGameStart = "TestingGrounds";
        public const float SettingSpawnAtActorVerticalOffset = 1.0f;
        public const float SettingSpawnAtActorForwardMultipler = 2.0f;

        // Game tag - Game setting pairs
        public static readonly string[][] ContainerCategories = {
            new [] { TagItemTypeArmor, SettingHeaderArmor },
            new [] { TagItemTypeWeapon, SettingHeaderWeapons },
            new [] { TagItemTypeAmmo, SettingHeaderAmmo },
            new [] { TagItemTypeJewelry, SettingHeaderJewelry },
            new [] { TagItemTypeCodex, SettingHeaderCodexes },
            new [] { TagItemTypePotion, SettingHeaderPotion },
            new [] { TagItemTypeFood, SettingHeaderFood },
            new [] { string.Empty, SettingHeaderMisc }
        };

        public static readonly string[][] CraftingCategories = {
            new [] { TagCraftCatBronze, SettingHeaderBronze },
            new [] { TagCraftCatIron, SettingHeaderIron },
            new [] { TagCraftCatSteel, SettingHeaderSteel },
            new [] { TagCraftCatMithril, SettingHeaderMithril },
            new [] { TagCraftCatAdamant, SettingHeaderAdamant },
            new [] { TagCraftCatPotion, SettingHeaderPotion },
            new [] { string.Empty, SettingHeaderMisc }
        };
    }
}
