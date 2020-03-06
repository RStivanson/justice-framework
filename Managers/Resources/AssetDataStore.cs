using JusticeFramework.Core;
using JusticeFramework.Core.Extensions;
using JusticeFramework.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeFramework.Managers.Resources {
    [Serializable]
    public class AssetDataStore : ResourceStore<ScriptableDataObject> {
        [SerializeField]
        private Dictionary<Type, List<ScriptableDataObject>> assetsByType;

        public AssetDataStore() : base() {
            assetsByType = new Dictionary<Type, List<ScriptableDataObject>>();
        }

        protected override void OnPreProcess() {
            assetsByType.Clear();
        }

        protected override void OnResourceProcessed(ScriptableDataObject model) {
            assetsByType.AddToList(model.GetType(), model);
        }

        public List<ScriptableDataObject> GetAssetsByType<T>() where T : ScriptableDataObject {
            List<ScriptableDataObject> assets;
            assetsByType.TryGetValue(typeof(T), out assets);
            return assets;
        }
    }
}
