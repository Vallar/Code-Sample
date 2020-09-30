// Copyright © Sascha Graeff/13Pixels.

namespace ThirteenPixels.Pooling
{
    using UnityEngine;
    using System.Collections;

    /// <summary>
    /// Allow a prefab instance to return *itself* to the pool.
    /// Instead of having to pass the prefab to Pooling.ReturnObject, pass a reference to this component.
    /// Instantiating a prefab with this component on takes a tiny bit of extra time, but keeps your code more clean when needed.
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Pooling/Pool Returner")]
    public sealed class PoolReturner : MonoBehaviour
    {
        internal GameObject prefab;

        /// <summary>
        /// Return this instance to the pool.
        /// If it was spawned with regular Instantiate rather than the Pooling system, the GameObject will be destroyed.
        /// </summary>
        public void Return()
        {
            StopAllCoroutines();
            if (prefab)
            {
                Pooling.ReturnObject(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Return this instance to the pool, but after a specified delay.
        /// </summary>
        public void ReturnAfter(float delay)
        {
            StopAllCoroutines();
            StartCoroutine(ReturnAfterCoroutine(delay));
        }

        private IEnumerator ReturnAfterCoroutine(float delay)
        {
            while (delay > 0)
            {
                yield return null;
                delay -= Time.deltaTime;
            }
            Return();
        }
    }
}
