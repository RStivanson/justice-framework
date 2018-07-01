using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace JusticeFramework.Core.DataStructures.Graph {
	[Serializable]
	public class Graph<T> where T : Node, new() {
		private static int nodeIdCounter = 0;
		
		[SerializeField]
		private List<T> adjacencyList;

		public int NumberOfNodes {
			get { return adjacencyList.Count; }
		}

		public Graph() {
			adjacencyList = new List<T>();
		}
		
		public virtual int AddNode(T node) {
			node.Id = nodeIdCounter++;
			adjacencyList.Add(node);
			return node.Id;
		}

		public virtual bool RemoveNode(int nodeId) {
			bool removed = false;

			for (int i = adjacencyList.Count - 1; i >= 0; i--) {
				if (adjacencyList[i].Id == nodeId) {
					adjacencyList.RemoveAt(i);
					removed = true;
				} else if (adjacencyList[i].RemoveEdge(nodeId)) {
					removed = true;
				}
			}

			return removed;
		}

		public virtual T GetNode(int nodeId) {
			T node = null;
			int index = GetIndex(nodeId);

			if (index != -1) {
				node = adjacencyList[index];
			}
			
			return node;
		}
		
		public virtual bool AddEdge(bool directed, int startNodeId, int endNodeId, float weight = 1) {
			bool added = false;

			if (ContainsNode(startNodeId) && ContainsNode(endNodeId)) {
				T startNode = adjacencyList[GetIndex(startNodeId)];
				T endNode = adjacencyList[GetIndex(endNodeId)];
				
				startNode.AddEdge(endNodeId, weight);

				if (!directed) {
					endNode.AddEdge(startNodeId, weight);
				}
				
				added = true;
			}
			
			return added;
		}
		
		public virtual bool RemoveEdge(int startNodeId, int endNodeId) {
			bool removed = false;
			
			if (ContainsNode(startNodeId) && ContainsNode(endNodeId)) {
				T startNode = adjacencyList[GetIndex(startNodeId)];
				if (startNode.RemoveEdge(endNodeId)) {
				}
				
				T endNode = adjacencyList[GetIndex(endNodeId)];
				if (endNode.RemoveEdge(startNodeId)) {
				}
				
				removed = true;
			}
			
			return removed;
		}
		
		public bool ContainsNode(int nodeId) {
			return GetIndex(nodeId) != -1;
		}
		
		private int GetIndex(int nodeId) {
			return adjacencyList.FindIndex(node => node.Id == nodeId);
		}
		
		private void Clear() {
			adjacencyList.Clear();
		}
	}
}