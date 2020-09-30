
namespace ThirteenPixels.Pooling.Demo
{
    using UnityEngine;
    using UnityEngine.Assertions;
    using ThirteenPixels.Pooling;

    /// <summary>
    /// Spawns a given prefab in a specified interval.
    /// Uses Pooling.GetObject instead of Instantiate.
    /// </summary>
    public class PoolSpawner : MonoBehaviour
    {
        private const float intervalMinimum = 0.002f;

        [SerializeField]
        private GameObject prefab = default;
        [Range(intervalMinimum, 2f)]
        public float interval = 0.5f;

        private float time;

        private void Update()
        {
            Assert.IsTrue(interval >= intervalMinimum);
            Assert.IsNotNull(prefab);

            time += Time.deltaTime;

            while (time >= interval)
            {
                time -= interval;
                Pooling.GetObject(prefab, transform.position, transform.rotation);
            }
        }
    }
}
