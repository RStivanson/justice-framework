using System.Collections.Generic;
using UnityEngine;

namespace JusticeFramework.Core
{
    /// <summary>
    /// A container class that stores references to MonoBehaviour objects for reuse.
    /// </summary>
    /// <typeparam name="T">The type of objects this store holds.</typeparam>
    public class MonoObjectPool<T> : MonoBehaviour where T : Component
    {
        /// <summary>
        /// The object prefab that this pool represents
        /// </summary>
        [SerializeField]
        private T prefab;

        /// <summary>
        /// The number of objects this pool can have available/in-use at one time.
        /// </summary>
        [SerializeField]
        private int size;

        /// <summary>
        /// A list of references to object who are available for use.
        /// </summary>
        private List<T> freeObjects;

        /// <summary>
        /// A list of references to objects who are currently checked out for use.
        /// </summary>
        private List<T> usedObjects;

        /// <summary>
        /// Gets the size of the pool.
        /// </summary>
        public int Size {
            get { return size; }
        }

        private void Awake() {
            // Set up the lists
            freeObjects = new List<T>(size);
            usedObjects = new List<T>(size);

            // Initialize the pool of objects
            Transform myTransform = transform;
            T poolObject;
            for (int i = 0; i < size; i++) {
                poolObject = Instantiate(prefab, Vector3.zero, Quaternion.identity, myTransform);
                poolObject.gameObject.SetActive(false);
                freeObjects.Add(poolObject);
            }
        }

        /// <summary>
        /// Gets a pooled object from the free pool.
        /// </summary>
        /// <returns>A pooled object, null if there are none left.</returns>
        public T GetObject() {
            T poolObject = null;
            int fCount = freeObjects.Count;

            if (fCount > 0) {
                poolObject = freeObjects[fCount - 1];
                freeObjects.RemoveAt(fCount - 1);
                usedObjects.Add(poolObject);
            }

            return poolObject;
        }

        /// <summary>
        /// Returns the pooled object back to the unused pool
        /// </summary>
        /// <param name="poolObject">The object to make available for use.</param>
        public void FreeObject(T poolObject) {
            // Reassign this object to the free list
            usedObjects.Remove(poolObject);
            freeObjects.Add(poolObject);

            // Reparent the object to us
            Transform tObjTransform = poolObject.transform;
            tObjTransform.SetParent(transform);
            tObjTransform.localPosition = Vector3.zero;
            poolObject.gameObject.SetActive(false);
        }
    }
}
