﻿using JusticeFramework.Assets.JusticeFramework.Utility.Extensions;
using JusticeFramework.Core.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeFramework.Core.Managers.Resources {
    [Serializable]
    public class AssetManager : ResourceManager<WorldObjectModel> {
        private const string DataPath = "Data/AssetData/";

        [SerializeField]
        private Dictionary<Type, List<WorldObjectModel>> assetsByType;

        public AssetManager() : base() {
            assetsByType = new Dictionary<Type, List<WorldObjectModel>>();
        }

        public override void LoadResources() {
            LoadResources(DataPath);
        }

        protected override void OnPostProcessStart() {
            assetsByType.Clear();
        }

        protected override void OnResourcePostProcessed(WorldObjectModel model) {
            assetsByType.AddToList(model.GetType(), model);
        }

        public List<WorldObjectModel> GetAssetsByType<T>() where T : WorldObjectModel {
            List<WorldObjectModel> assets;
            assetsByType.TryGetValue(typeof(T), out assets);
            return assets;
        }
    }
}