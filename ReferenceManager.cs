using System;
using System.Collections.Generic;
using JusticeFramework.Components;
using UnityEngine;

namespace JusticeFramework {
	public delegate void OnCellLoad();
	public delegate void OnCellUnload();

	[Serializable]
	public class ReferenceManager {
		public const int CellSize = 256;
		public const int HalfCellSize = CellSize / 2;

		[SerializeField]
		private List<Reference> references;
		
		public int ReferenceCount {
			get {
				return references.Count;
			}
		}

		public ReferenceManager() {
			Clear();
		}
		
		public void Clear() {
			references = new List<Reference>();
		}



		public void RegisterReference(Reference toRegister) {
			references.Add(toRegister);
		}

		public void UnregisterReference(Reference toUnregister) {
			references.Remove(toUnregister);
		}





		public static Vector2 GetCellCoord(Vector3 worldPosition) {
			return new Vector2(FloatToCellCoordinate(worldPosition.x), FloatToCellCoordinate(worldPosition.z));
		}

		private static int FloatToCellCoordinate(float position) {
			return Mathf.FloorToInt((position + HalfCellSize) / CellSize);
		}

		private static float ToCenterPositon(float position) {
			return (position * CellSize) - HalfCellSize;
		}
	}
}