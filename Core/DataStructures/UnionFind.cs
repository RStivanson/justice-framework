using System;
using UnityEngine;

namespace JusticeFramework.Core.DataStructures {
	[Serializable]
	public class UnionFind {
		[SerializeField]
		private int[] parents;

		[SerializeField]
		private int[] treeSize;

		private UnionFind(int size) {
			parents = new int[size];
			treeSize = new int[size];
			
			for (int i = 0; i < size; i++) {
				parents[i] = i;
				treeSize[i] = i;
			}
		}
		
		public bool Union(int idOne, int idTwo) {
			bool unioned = false;
	
			idOne = Find(idOne);
			idTwo = Find(idTwo);
	
			if (idOne != idTwo) {
				if (treeSize[idOne] < treeSize[idTwo]) {
					parents[idOne] = idTwo;
					treeSize[idTwo] += treeSize[idOne];
				} else {
					parents[idTwo] = idOne;
					treeSize[idOne] += treeSize[idTwo];
				}
		
				unioned = true;
			}
	
			return unioned;
		}
		
		public int Find(int id) {
			int root = id;
	
			while (root != parents[root]) {
				root = parents[root];
			}
	
			return root;
		}
	}
}