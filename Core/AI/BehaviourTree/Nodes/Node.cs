using System;
using UnityEngine;

namespace JusticeFramework.Core.AI.BehaviourTree.Nodes {
	[Serializable]
	public class Node {
		private static int nodeIdCounter = 0;

		private bool isOpened;

		public int Id {
            get;
            private set;
		}
		
		public int TickCount {
            get;
            private set;
		}
		
		public Node() {
			Id = nodeIdCounter++;
            TickCount = 0;
			isOpened = false;
		}

		public ENodeStatus Tick(TickState tick) {
			TickCount++;
			
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
		
        protected virtual void Cancel(TickState tick) {
        }

		protected virtual void Open(TickState tick) {
		}

		protected virtual void Close(TickState tick, ENodeStatus status) {
		}
	}
}