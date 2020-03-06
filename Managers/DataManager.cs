using JusticeFramework.Collections;
using JusticeFramework.Data;
using JusticeFramework.Interfaces;
using JusticeFramework.Logic;
using JusticeFramework.Managers.Resources;
using System;
using System.Collections.Generic;

namespace JusticeFramework.Managers {
    [Serializable]
    public class DataManager {
        private const string GameSettingDataPath = "Data/GameSettings/";
        private const string AssetDataPath = "Data/AssetData/";
        private const string DialogueDataPath = "Data/Dialogue/";
        private const string QuestDataPath = "Data/Quests/";
        private const string RecipeDataPath = "Data/Recipes/";
        private const string FactionDataPath = "Data/Factions/";

        public AssetDataStore AssetStore {
            get; private set;
        }

        public DialogueDataStore DialogueStore {
            get; private set;
        }

        public QuestDataStore QuestStore {
            get; private set;
        }

        public RecipeDataStore RecipeStore {
            get; private set;
        }

        public FactionDataStore FactionStore {
            get; private set;
        }

        public DataManager() {
            AssetStore = new AssetDataStore();
            DialogueStore = new DialogueDataStore();
            QuestStore = new QuestDataStore();
            RecipeStore = new RecipeDataStore();
            FactionStore = new FactionDataStore();
        }

        public void LoadData() {
            AssetStore.LoadResources(AssetDataPath);
            DialogueStore.LoadResources(DialogueDataPath);
            QuestStore.LoadResources(QuestDataPath);
            RecipeStore.LoadResources(RecipeDataPath);
            FactionStore.LoadResources(FactionDataPath);
        }

        #region Asset Data Methods

        public ScriptableDataObject GetAssetById(string id) {
            return AssetStore.GetById(id);
        }

        public T GetAssetById<T>(string id) where T : ScriptableDataObject {
            return AssetStore.GetById<T>(id);
        }

        public List<ScriptableDataObject> GetAssetsByType<T>() where T : ScriptableDataObject {
            return AssetStore.GetAssetsByType<T>();
        }

        public bool IsAssetLoaded(string id) {
            return AssetStore.Contains(id);
        }

        public bool IsAssetLoaded<T>(string id) where T : ScriptableDataObject {
            return AssetStore.Contains<T>(id);
        }

        #endregion

        #region Dialogue Data Methods

        public List<DialogueTree> GetDialogueForActor(ActorData actorData) {
            List<DialogueTree> results = new List<DialogueTree>(10);

            foreach (DialogueData d in DialogueStore.GetDialogueByTargetId(actorData.Id)) {
                results.Add(new DialogueTree(d));
            }

            foreach (FactionData faction in FactionStore.GetFactionsWithMember(actorData.Id)) {
                foreach (DialogueData d in DialogueStore.GetDialogueByFactionId(faction.Id)) {
                    results.Add(new DialogueTree(d));
                }
            }

            return results;
        }

        #endregion

        #region Quest Data Methods

        /*public QuestSequence GetQuestById(string id) {
            return questManager.GetById(id);
        }

        public string GetQuestDisplayName(string id) {
            return questManager.GetById(id).QuestData.DisplayName;
        }

        public int GetQuestStage(string id) {
            return questManager.GetQuestStage(id);
        }

        public void SetQuestStage(string id, int marker) {
            questManager.SetQuestStage(id, marker);
        }

        public EQuestState GetQuestState(string id) {
            return questManager.GetQuestState(id);
        }

        public bool IsQuestAtStage(string id, int stage) {
            return questManager.GetQuestStage(id) == stage;
        }

        public bool IsQuestAtState(string id, EQuestState state) {
            return questManager.GetQuestState(id) == state;
        }

        public List<QuestSequence> GetInProgressQuests() {
            return questManager.GetQuestsByState(EQuestState.InProgress);
        }

        public List<QuestSequence> GetCompletedQuests() {
            return questManager.GetQuestsByState(EQuestState.Completed);
        }*/

        #endregion

        #region Recipe Data Methods

        public List<RecipeData> GetRecipes() {
            return RecipeStore.GetAll();
        }

        public RecipeData GetRecipeById(string id) {
            return RecipeStore.GetById(id);
        }

        public List<RecipeData> GetRecipesWithMatchingIngredients(IContainer container) {
            return RecipeStore.GetAllWithMatchingIngredients(container.Inventory);
        }
        
        public List<RecipeData> GetRecipesWithMatchingIngredients(Inventory inventory) {
            return RecipeStore.GetAllWithMatchingIngredients(inventory);
        }

        #endregion  
    }
}
