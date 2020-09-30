// Copyright © Sascha Graeff/13Pixels.

namespace ThirteenPixels.Pooling
{
    using UnityEngine;

    /// <summary>
    /// Implement this interface in a component to be put on your to-be-pooled prefabs.
    /// The OnPoolingSetActive method will be called by the Pooling class instead of using GameObject.SetActive.
    /// This way, you are able to implement context-dependent pooling that can easily out-perform the default behaviour.
    /// </summary>
    public interface IPoolingBehaviour
    {
        void OnPoolingSetActive(bool active);
    }
}
