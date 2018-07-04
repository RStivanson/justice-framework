using JusticeFramework.Core.Models.Settings;
using System;

namespace JusticeFramework.Core.Managers.Resources {
    [Serializable]
    public class SettingsManager : ResourceManager<Setting> {
        private string DataPath = "Data/Settings";

        public override void LoadResources() {
            LoadResources(DataPath, true);
        }

        public int GetInt(string id) {
            return (int)GetById(id).FloatValue;
        }

        public float GetFloat(string id) {
            return GetById(id).FloatValue;
        }

        public string GetString(string id) {
            return GetById(id).StringValue;
        }
    }
}
