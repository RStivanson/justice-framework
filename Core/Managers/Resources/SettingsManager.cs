using JusticeFramework.Core.Models.Settings;
using System;

namespace JusticeFramework.Core.Managers.Resources {
    [Serializable]
    public class SettingsManager : ResourceManager<Setting> {
        private string DataPath = "Data/Settings";

        public override void LoadResources() {
            LoadResources(DataPath);
        }
    }
}
