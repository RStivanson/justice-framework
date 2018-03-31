using System;
using UnityEngine;

namespace JusticeFramework.Data.AI.BehaviourTree.Nodes {
	[Serializable]
	public class Node {
		private static int nodeIdCounter = 0;

		private int id;
		private int tickCount;
		private bool isOpened;

		public int Id {
			get { return id; }
		}
		
		public int TickCount {
			get { return tickCount; }
		}
		
		public Node() {
			id = nodeIdCounter++;
			tickCount = 0;
			isOpened = false;
		}

		public ENodeStatus Tick(TickState tick) {
			tickCount++;
			
			if (!isOpened) {
				isOpened = true;
				Open(tick);
			}

#if UNITY_EDITOR
			if (tick.debug) {
				Debug.Log(GetType());
			}
#endif
			
			ENodeStatus status = OnTick(tick);

			if (isOpened && status != ENodeStatus.Running) {
				isOpened = false;
				Close(tick, status);
			}
			
			return status;
		}
		
		protected virtual ENodeStatus OnTick(TickState tick) {
			return ENodeStatus.Success;
		}
		
		protected virtual void Open(TickState tick) {
		}

		protected virtual void Close(TickState tick, ENodeStatus status) {
		}
	}
}