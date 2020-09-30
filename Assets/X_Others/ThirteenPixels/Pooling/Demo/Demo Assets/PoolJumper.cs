
namespace ThirteenPixels.Pooling.Demo
{
    using UnityEngine;
    using ThirteenPixels.Pooling;

    /// <summary>
    /// This component showcases both the use of the PoolReturner component and the IPoolingBehaviour interface.
    /// The PoolReturner component that is required to be on the same GameObject is used to return the object to its pool
    /// as soon as it moved downwards far enough.
    /// The OnPoolingSetActive method implemented from IPoolingBehaviour is used to reset the object in multiple ways
    /// in addition to calling gameObject.SetActive.
    /// </summary>
    [RequireComponent(typeof(PoolReturner))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PoolJumper : MonoBehaviour, IPoolingBehaviour
    {
        private PoolReturner poolReturner;
        new private Rigidbody2D rigidbody2D;

        private void Awake()
        {
            poolReturner = GetComponent<PoolReturner>();
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (transform.position.y < -4)
            {
                poolReturner.Return();
            }
        }

        /// <summary>
        /// This method, implemented for the IPoolingBehaviour interface,
        /// is called whenever the object is taken from or returned to its pool.
        /// It replaces the default behaviour, which is having gameObject.SetActive(active) called.
        /// </summary>
        public void OnPoolingSetActive(bool active)
        {
            gameObject.SetActive(active);
            if (active)
            {
                rigidbody2D.velocity = Random.insideUnitCircle * 4;

                transform.rotation = Quaternion.identity;
                rigidbody2D.angularVelocity = Random.Range(-90f, 90f);

                // Alternatively, use this to spawn the object with zero velocity every time.
                // rigidbody2D.velocity = Vector2.zero;
            }
        }
    }
}
