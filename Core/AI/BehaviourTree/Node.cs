using System;
using UnityEngine;

namespace JusticeFramework.Core.AI.BehaviourTree {
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

		public ENodeStatus Tick(BehaviourTree.Context tick) {
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
		
		protected virtual ENodeStatus OnTick(BehaviourTree.Context tick) {
			return ENodeStatus.Success;
		}
		
        protected virtual void Cancel(BehaviourTree.Context tick) {
        }

		protected virtual void Open(BehaviourTree.Context tick) {
		}

		protected virtual void Close(BehaviourTree.Context tick, ENodeStatus status) {
		}
	}
}