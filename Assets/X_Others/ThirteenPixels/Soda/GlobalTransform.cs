// Copyright © Sascha Graeff/13Pixels.

namespace ThirteenPixels.Soda
{
    using UnityEngine;

    /// <summary>
    /// A GlobalVariable representing a Transform.
    /// </summary>
    [CreateAssetMenu(menuName = "Soda/GlobalVariable/Transform", order = 200)]
    public class GlobalTransform : GlobalVariableBase<Transform>
    {

    }

    /// <summary>
    /// A ScopedVariable representing either a GlobalTransform or a local Transform value.
    /// </summary>
    [System.Serializable]
    public class ScopedTransform : ScopedVariableBase<Transform, GlobalTransform>
    {

    }
}
