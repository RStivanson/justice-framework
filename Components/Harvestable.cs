using JusticeFramework.Data;
using JusticeFramework.Events;
using JusticeFramework.Interfaces;
using JusticeFramework.Logic;
using System;
using UnityEngine;

namespace JusticeFramework.Components {
    [Serializable]
	[RequireComponent(typeof(CapsuleCollider))]
	public class Harvestable : WorldObject {
		[SerializeField]
		private SkinnedMeshRenderer thisRenderer;

		[SerializeField]
		private bool canHarvest = true;

		[SerializeField]
		public float respawnTimer = -1;

		public override EInteractionType InteractionType {
			get { return EInteractionType.Take; }
		}

		public bool CanBeHarvested {
			get { return canHarvest; }
		}

		private void Update() {
			if (respawnTimer < 0) {
				return;
			}
			
			respawnTimer += Time.deltaTime;

            HarvestableData data = dataObject as HarvestableData;
			if (respawnTimer >= data.RespawnTimeInSeconds) {
				respawnTimer = -1;
				canHarvest = true;
				OnReferenceStateChanged();
			}
		}

		public void Harvest(IContainer container) {
            HarvestableData data = dataObject as HarvestableData;
            container.Inventory.Add(data.HarvestOutput.itemData.Id, data.HarvestOutput.quantity);
			
			canHarvest = false;
			respawnTimer = 0;
			OnReferenceStateChanged();
        }

        protected override Logic.Action OnActivate(IWorldObject instance) {
            HarvestableData data = dataObject as HarvestableData;
            return new ActionHarvest(this, data.HarvestSound, EAudioType.SoundEffect, 1);
        }
    }
}