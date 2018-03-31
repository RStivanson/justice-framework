using System;
using JusticeFramework.Data.Models;
using JusticeFramework.Data;
using JusticeFramework.Data.Events;
using JusticeFramework.Data.Interfaces;
using JusticeFramework.Interfaces;
using JusticeFramework.Utility.Extensions;
using UnityEngine;

namespace JusticeFramework.Components {
	[Serializable]
	[RequireComponent(typeof(CapsuleCollider))]
	public class Flower : Reference, IFlower {
#region Variables

		[SerializeField]
		private FlowerModel flowerModel;

		[SerializeField]
		private SkinnedMeshRenderer thisRenderer;

		[SerializeField]
		private bool canHarvest = true;

		[SerializeField]
		public float respawnTimer = -1;

#endregion

#region Properties

		private FlowerModel FlowerModel {
			get { return model as FlowerModel; }
		}
		
		public override EInteractionType InteractionType {
			get { return EInteractionType.Take; }
		}

		public bool CanBeHarvested {
			get { return canHarvest; }
		}

		public string GivesId {
			get { return flowerModel.harvestData.id; }
		}
		
		public int GivesQuantity {
			get { return flowerModel.harvestData.quantity; }
		}

		public int RespawnTimeInSeconds {
			get { return flowerModel.respawnTimeInSeconds; }
		}

		public AudioClip HarvestSound {
			get { return flowerModel.harvestSound; }
		}

#endregion

		private void Update() {
			if (respawnTimer < 0) {
				return;
			}
			
			respawnTimer += Time.deltaTime;
			
			if (respawnTimer >= flowerModel.respawnTimeInSeconds) {
				respawnTimer = -1;
				canHarvest = true;
				OnReferenceStateChanged();
			}
		}

		public void Harvest(IContainer container) {
			container.GiveItem(GivesId, GivesQuantity);
			
			canHarvest = false;
			respawnTimer = 0;
			OnReferenceStateChanged();
		}
		
		public override void Activate(object sender, ActivateEventArgs e) {
			if (e?.Activator != null) {
				return;
			}
						
			if (e?.ActivatedBy is IContainer && CanBeHarvested) {
				Harvest((IContainer)e?.ActivatedBy);
			}
		}
	}
}