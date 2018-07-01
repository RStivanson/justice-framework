using System;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeFramework.Core.DataStructures.Graph {
	[Serializable]
	public class Node {
		[SerializeField]
		private int id;

		[SerializeField]
		private List<Edge> edges;

		public int Id {
			get { return id; }
			set { id = value; }
		}

		public List<Edge> Edges {
			get { return edges; }
		}
		
		public Node() : this(0) {
		}

		public Node(int id) {
			this.id = id;
			edges = new List<Edge>();
		}

		public bool AddEdge(int endNodeId, float weight = 1) {
			bool added = false;
			
			if (endNodeId != id && !ContainsEdge(endNodeId)) {
				edges.Add(new Edge(id, endNodeId, weight));
				added = true;
			}
			
			return added;
		}

		public bool RemoveEdge(int endNodeId) {
			bool removed = false;
			
			if (endNodeId != id) {
				int edgeIndex = GetEdgeIndex(endNodeId);
				
				if (edgeIndex != -1) {
					edges.RemoveAt(edgeIndex);
					removed = true;
				}
			}

			return removed;
		}

		public bool UpdateEdge(int endNodeId, int newEndNodeId, bool keepWeight, float weight = 0) {
			bool updated = false;
			
			if (endNodeId != id) {
				int edgeIndex = GetEdgeIndex(endNodeId);
				
				if (edgeIndex != -1) {
					float newWeight = keepWeight ? edges[edgeIndex].Weight : weight;
					
					edges.Add(new Edge(id, newEndNodeId, newWeight));
					edges.RemoveAt(edgeIndex);
					
					updated = true;
				}
			}
	
			return updated;
		}

		public bool ContainsEdge(int endNodeId) {
			return GetEdgeIndex(endNodeId) != -1;
		}

		private int GetEdgeIndex(int endNodeId) {
			return edges.FindIndex(edge => edge.EndNodeId == endNodeId);
		}
	}
}