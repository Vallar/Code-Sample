// Copyright © Sascha Graeff/13Pixels.

namespace ThirteenPixels.Pooling
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using System.Collections.Generic;

    public static class Pooling
    {
        private const int defaultInitialPoolSize = 5;
        private static Dictionary<GameObject, Pool> pools = new Dictionary<GameObject, Pool>();
        internal static bool objectsAreVisibleInHierarchy { private set; get; }

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            SceneManager.sceneUnloaded += _ => CleanPools();
        }

        #region GetObject
        /// <summary>
        /// Returns an active clone of the <paramref name="prefab"/>, taken from its pool if available.
        /// </summary>
        public static GameObject GetObject(GameObject prefab, int initialPoolSize = defaultInitialPoolSize)
        {
            Pool pool;
            if (pools.TryGetValue(prefab, out pool))
            {
                return pool.GetObject();
            }
            else
            {
                GameObject result;
                pool = new Pool(prefab, initialPoolSize, out result);
                pools.Add(prefab, pool);
                return result;
            }
        }

        /// <summary>
        /// Returns an active clone of the <paramref name="prefab"/>, taken from its pool if available.
        /// Applies the given position and rotation to the object.
        /// </summary>
        public static GameObject GetObject(GameObject prefab,
                                           Vector3 position,
                                           Quaternion rotation,
                                           int initialPoolSize = defaultInitialPoolSize)
        {
            Pool pool;
            if (pools.TryGetValue(prefab, out pool))
            {
                return pool.GetObject(position, rotation);
            }
            else
            {
                GameObject result;
                pool = new Pool(prefab, initialPoolSize, out result, position, rotation);
                pools.Add(prefab, pool);
                return result;
            }
        }

        /// <summary>
        /// Returns an active clone of the <paramref name="prefab"/>, taken from its pool if available.
        /// </summary>
        public static T GetObject<T>(T prefab, int initialPoolSize = defaultInitialPoolSize) where T : Component
        {
            return GetObject(prefab.gameObject, initialPoolSize).GetComponent<T>();
        }

        /// <summary>
        /// Returns an active clone of the <paramref name="prefab"/>, taken from its pool if available.
        /// Applies the given position and rotation to the object.
        /// </summary>
        public static T GetObject<T>(T prefab,
                                     Vector3 position,
                                     Quaternion rotation,
                                     int initialPoolSize = defaultInitialPoolSize) where T : Component
        {
            var result = GetObject(prefab, initialPoolSize);
            var transform = result.transform;
            transform.position = position;
            transform.rotation = rotation;
            return result;
        }
        #endregion

        #region ReturnObject
        /// <summary>
        /// Disables <paramref name="instance"/>, an active clone of the <paramref name="prefab"/>,
        /// and returns it to the pool.
        /// The passed <paramref name="instance"/> must be a clone of <paramref name="prefab"/>.
        /// </summary>
        public static void ReturnObject(GameObject prefab, GameObject instance)
        {
            Pool pool;
            if (pools.TryGetValue(prefab, out pool))
            {
                pool.ReturnObject(instance);
            }
            else
            {
                throw new System.ArgumentException("The passed prefab does not have a pool."
                    + " Only return an object to a pool if it has been taken out of it.");
            }
        }

        /// <summary>
        /// Disables <paramref name="instance"/>, an active clone of the <paramref name="prefab"/>,
        /// and returns it to the pool.
        /// The passed <paramref name="instance"/> must be a clone of <paramref name="prefab"/>.
        /// </summary>
        public static void ReturnObject<T>(T prefab, T instance) where T : Component
        {
            ReturnObject(prefab.gameObject, instance.gameObject);
        }

        /// <summary>
        /// Disables <paramref name="instance"/> and returns it to the pool.
        /// Is only supposed to be called by the PoolReturner instance itself.
        /// </summary>
        internal static void ReturnObject(PoolReturner instance)
        {
            ReturnObject(instance.prefab, instance.gameObject);
        }
        #endregion

        #region Prefilling
        /// <summary>
        /// Fills the corresponding pool of the given <paramref name="prefab"/> to have at least <paramref name="target"/> items in it.
        /// This is including the currently active (not pooled) objects.
        /// </summary>
        public static void Fill(GameObject prefab, int target)
        {
            Pool pool;
            if (pools.TryGetValue(prefab, out pool))
            {
                pool.Fill(target);
            }
            else
            {
                pool = new Pool(prefab, target);
                pools.Add(prefab, pool);
            }
        }

        /// <summary>
        /// Creates <paramref name="amount"/> inactive instances of the given <paramref name="prefab"/> and adds them to its corresponding pool.
        /// Use this to create prefabs before they're needed, to avoid hiccups during the game.
        /// </summary>
        public static void CreateObjects(GameObject prefab, int amount)
        {
            Pool pool;
            if (pools.TryGetValue(prefab, out pool))
            {
                pool.InstantiateBatch(amount);
            }
            else
            {
                pool = new Pool(prefab, amount);
                pools.Add(prefab, pool);
            }
        }

        /// <summary>
        /// Creates <paramref name="amount"/> inactive instances of the given <paramref name="prefab"/> and adds them to its corresponding pool.
        /// Use this to create prefabs before they're needed, to avoid hiccups during the game.
        /// </summary>
        public static void CreateObjects(Component prefab, int amount)
        {
            CreateObjects(prefab.gameObject, amount);
        }
        #endregion

        private static void CleanPools()
        {
            foreach (var pool in pools.Values)
            {
                pool.Clean();
                // At this point, it could be checked whether the pool is empty (count == 0),
                // and if it was, it could be removed.
                // However, this just creates more garbage as many pools are likely to be needed later again.
            }
        }

#if UNITY_EDITOR
        internal static void SetEditorObjectVisibility(bool visible)
        {
            if (objectsAreVisibleInHierarchy == visible) return;

            objectsAreVisibleInHierarchy = visible;
            foreach (var pool in pools.Values)
            {
                pool.SetEditorObjectVisibility(visible);
            }
        }

        internal static Dictionary<GameObject, Pool> GetPools()
        {
            return pools;
        }
#endif
    }
}
