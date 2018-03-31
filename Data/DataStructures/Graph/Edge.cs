using System;
using UnityEngine;

namespace JusticeFramework.Data.DataStructures.Graph {
	[Serializable]
	public class Edge {
		[SerializeField]
		private int startNodeId;

		[SerializeField]
		private int endNodeId;

		[SerializeField]
		private float weight;

		public int StartNodeId {
			get { return startNodeId; }
		}

		public int EndNodeId {
			get { return endNodeId; }
		}

		public float Weight {
			get { return weight; }
		}
		
		public Edge(int startNodeId, int endNodeId, float weight = 1) {
			this.startNodeId = startNodeId;
			this.endNodeId = endNodeId;
			this.weight = weight;
		}

		public static bool operator <(Edge left, Edge right) {
			return left.weight < right.weight;
		}
		
		public static bool operator <=(Edge left, Edge right) {
			return left.weight <= right.weight;
		}
		
		public static bool operator >(Edge left, Edge right) {
			return left.weight > right.weight;
		}
		
		public static bool operator >=(Edge left, Edge right) {
			return left.weight >= right.weight;
		}
		
		public static bool operator ==(Edge left, Edge right) {
			if (left == null || right == null) {
				return false;
			}
			
			return left.weight == right.weight;
		}
		
		public static bool operator !=(Edge left, Edge right) {
			if (left == null || right == null) {
				return false;
			}
			
			return left.weight != right.weight;
		}
	}
}