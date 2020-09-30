// Copyright © Sascha Graeff/13Pixels.

namespace ThirteenPixels.Pooling
{
    using UnityEngine;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the GameObject pool for the clones of one specific prefab.
    /// Getting and returning objects to and from it activates and deactivates them.
    /// </summary>
    internal class Pool
    {
        private struct Item
        {
            public GameObject instance;
            public IPoolingBehaviour poolingBehaviour;

            public Item(GameObject instance, bool hasPoolingBehaviour)
            {
                this.instance = instance;
                poolingBehaviour = hasPoolingBehaviour ? instance.GetComponent<IPoolingBehaviour>() : null;
            }

            public void SetActive(bool active)
            {
                if (poolingBehaviour != null)
                {
                    try
                    {
                        poolingBehaviour.OnPoolingSetActive(active);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
                else
                {
                    instance.SetActive(active);
                }
            }
        }

        // The prefab for the pool objects.
        private GameObject prefab;
        private bool prefabHasPoolingBehaviour;
        private bool prefabHasPoolReturner;
        // Amount of objects spawned by the pool, active or inactive.
        public int count { get; private set; }
        // All inactive/available objects in the pool that have never been activated before.
        // The first time they are used, they have to additionally have gameObject.SetActive(true) called,
        // as the prefab was deactivated before instantiating them, regardless of it having a PoolingBehaviour.
        // This is to not trigger an OnEnable event when instantiating directly into the pool.
        private Queue<Item> freshObjects = new Queue<Item>();
        // All inactive/available objects in the pool, ready to be taken out.
        private Queue<Item> availableObjects = new Queue<Item>();
        // All clones created by this pool that are currently active in the scene.
        private Dictionary<GameObject, Item> activeObjects = new Dictionary<GameObject, Item>();
        // All objects managed by the pool, regardnless of their state.
        private HashSet<GameObject> allObjects = new HashSet<GameObject>();

        /// <summary>
        /// Creates a new pool and fills it with a certain amount of clones of the given <paramref name="prefab"/>.
        /// <param name="prefab">The prefab for the objects in this pool.</param>
        /// <param name="initialCount">
        /// The amount of initially created objects.
        /// Might be 0.
        /// </param>
        /// </summary>
        public Pool(GameObject prefab, int initialCount)
        {
            if (initialCount < 0)
            {
                initialCount = 0;
            }

            InitializePool(prefab);

            if (initialCount > 0)
            {
                InstantiateBatch(initialCount);
            }
        }

        /// <summary>
        /// Creates a new pool, fills it with a certain amount of clones of the given <paramref name="prefab"/>,
        /// and passes one of the clones back via <paramref name="firstObject"/>.
        /// <param name="prefab">The prefab for the objects in this pool.</param>
        /// <param name="initialCount">
        /// The amount of initially created objects.
        /// Will be handled as at least 1.
        /// </param>
        /// <param name="firstObject">
        /// The first object of the pool.
        /// The referenced object will be active.
        /// </param>
        /// </summary>
        public Pool(GameObject prefab, int initialCount, out GameObject firstObject)
        {
            if (initialCount <= 0)
            {
                initialCount = 1;
            }

            InitializePool(prefab);

            firstObject = InstantiateBatchAndReturnOneActive(initialCount);
        }

        /// <summary>
        /// Creates a new pool, fills it with a certain amount of clones of the given <paramref name="prefab"/>,
        /// and passes one of the clones back via <paramref name="firstObject"/>,
        /// which has the given <paramref name="position"/> and <paramref name="rotation"/> applied.
        /// <param name="prefab">The prefab for the objects in this pool.</param>
        /// <param name="initialCount">
        /// The amount of initially created objects.
        /// Will be handled as at least 1.
        /// </param>
        /// <param name="firstObject">
        /// The first object of the pool.
        /// The referenced object will be active.
        /// </param>
        /// </summary>
        public Pool(GameObject prefab, int initialCount, out GameObject firstObject, Vector3 position, Quaternion rotation)
        {
            if (initialCount <= 0)
            {
                initialCount = 1;
            }

            InitializePool(prefab);

            firstObject = InstantiateBatchAndReturnOneActive(initialCount, position, rotation);
        }

        private void InitializePool(GameObject prefab)
        {
            this.prefab = prefab;

            prefabHasPoolingBehaviour = prefab.GetComponent<IPoolingBehaviour>() != null;
            prefabHasPoolReturner = prefab.GetComponent<PoolReturner>() != null;
        }

        /// <summary>
        /// Instantiates one active GameObject to return, and amount - 1 inactive ones for the pool.
        /// </summary>
        private GameObject InstantiateBatchAndReturnOneActive(int amount)
        {
            InstantiateBatch(amount - 1);

            var active = Instantiate();
            activeObjects.Add(active.instance, active);
            active.SetActive(true);
            return active.instance;
        }

        /// <summary>
        /// Instantiates one active GameObject to return, and amount - 1 inactive ones for the pool.
        /// </summary>
        private GameObject InstantiateBatchAndReturnOneActive(int amount, Vector3 position, Quaternion rotation)
        {
            InstantiateBatch(amount - 1);

            var active = Instantiate(position, rotation);
            active.SetActive(true);
            activeObjects.Add(active.instance, active);
            return active.instance;
        }

        /// <summary>
        /// Instantiates amount inactive objects for the pool.
        /// </summary>
        internal void InstantiateBatch(int amount)
        {
            count += amount;

            var prefabWasEnabled = prefab.activeSelf;
            if (prefabWasEnabled)
            {
                prefab.SetActive(false);
            }

            for (var i = 0; i < amount; i++)
            {
                freshObjects.Enqueue(Instantiate());
            }

            if (prefabWasEnabled)
            {
                prefab.SetActive(true);
            }
        }

        /// <summary>
        /// Instantiates as many inactive objects as needed to get the pool to the given target count.
        /// </summary>
        public void Fill(int target)
        {
            var amount = target - count;
            if (amount > 0)
            {
                InstantiateBatch(amount);
            }
        }

        /// <summary>
        /// Returns an active clone of the pool's prefab.
        /// Instantiates clones if the pool is empty.
        /// The amount of instantiated clones is the current pool object count.
        /// </summary>
        public GameObject GetObject()
        {
            Item result;
            if (availableObjects.Count > 0)
            {
                result = availableObjects.Dequeue();
            }
            else if (freshObjects.Count > 0)
            {
                result = DequeueFreshObject();
            }
            else
            {
                result = Instantiate();
            }

            activeObjects.Add(result.instance, result);
            result.SetActive(true);

            return result.instance;
        }

        /// <summary>
        /// Returns an active clone of the pool's prefab.
        /// Instantiates clones if the pool is empty.
        /// The amount of instantiated clones is the current pool object count.
        /// </summary>
        public GameObject GetObject(Vector3 position, Quaternion rotation)
        {
            Item result;
            if (availableObjects.Count > 0)
            {
                result = availableObjects.Dequeue();
            }
            else if (freshObjects.Count > 0)
            {
                result = DequeueFreshObject();
            }
            else
            {
                result = Instantiate(position, rotation);
            }

            var transform = result.instance.transform;
            transform.position = position;
            transform.rotation = rotation;
            activeObjects.Add(result.instance, result);
            result.SetActive(true);

            return result.instance;
        }

        private Item DequeueFreshObject()
        {
            var result = freshObjects.Dequeue();
            // Activate the fresh object since it was spawned inactive.
            // Do this now to have Awake called consistently before SetActive.
            result.instance.SetActive(true);
            return result;
        }

        /// <summary>
        /// Returns a clone to the pool.
        /// Assumes that the given GameObject actually is an instance of the pool's prefab.
        /// </summary>
        public void ReturnObject(GameObject go)
        {
            // TODO Replace with activeObjects.Remove(go, out var item) once Unity supports .NET core 2.0
            if (activeObjects.TryGetValue(go, out var item))
            {
                activeObjects.Remove(go);
                item.SetActive(false);
                availableObjects.Enqueue(item);
            }
            else if (!allObjects.Contains(go))
            {
                throw new System.ArgumentException("The object you tried to return to a pool was not instantiated by the pool.");
            }
        }

        /// <summary>
        /// "Cleans" the pool of all destroyed objects.
        /// </summary>
        public void Clean()
        {
            Clean(ref freshObjects);
            Clean(ref availableObjects);
            Clean(ref activeObjects);
            Clean(ref allObjects);

            count = freshObjects.Count + availableObjects.Count + activeObjects.Count;
        }

        private void Clean(ref HashSet<GameObject> collection)
        {
            var newCollection = new HashSet<GameObject>();
            foreach (var go in collection)
            {
                if (go)
                {
                    newCollection.Add(go);
                }
            }
            collection = newCollection;
        }

        private static void Clean(ref Queue<Item> collection)
        {
            var newCollection = new Queue<Item>();
            foreach (var item in collection)
            {
                if (item.instance)
                {
                    newCollection.Enqueue(item);
                }
            }
            collection = newCollection;
        }

        private static void Clean(ref Dictionary<GameObject, Item> collection)
        {
            var newCollection = new Dictionary<GameObject, Item>();
            foreach (var item in collection)
            {
                if (item.Key)
                {
                    newCollection.Add(item.Key, item.Value);
                }
            }
            collection = newCollection;
        }

        private Item Instantiate()
        {
            var go = Object.Instantiate(prefab);
            var item = Initialize(go);
            return item;
        }

        private Item Instantiate(Vector3 position, Quaternion rotation)
        {
            var go = Object.Instantiate(prefab, position, rotation);
            var item = Initialize(go);
            return item;
        }

        private Item Initialize(GameObject go)
        {
#if UNITY_EDITOR
            if (!Pooling.objectsAreVisibleInHierarchy)
            {
                go.hideFlags = HideFlags.HideInHierarchy;
            }
#endif
            if (prefabHasPoolReturner)
            {
                go.GetComponent<PoolReturner>().prefab = prefab;
            }

            allObjects.Add(go);

            return new Item(go, prefabHasPoolingBehaviour);
        }

#if UNITY_EDITOR
        internal void SetEditorObjectVisibility(bool visible)
        {
            var hideFlags = visible ? HideFlags.None : HideFlags.HideInHierarchy;

            foreach (var go in allObjects)
            {
                go.hideFlags = hideFlags;
            }
        }

        internal int activeObjectCount
        {
            get { return activeObjects.Count; }
        }

        internal int inactiveObjectCount
        {
            get { return freshObjects.Count + availableObjects.Count; }
        }
#endif
    }
}
