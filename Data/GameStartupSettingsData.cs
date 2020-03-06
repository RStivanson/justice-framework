using UnityEngine;

namespace JusticeFramework.Data {
    [CreateAssetMenu(menuName = "Justice Framework/Game Setting", order = 150)]
    public class GameStartupSettingsData : ScriptableDataObject {
        public string startScene;
    }
}
