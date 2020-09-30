// Copyright © Sascha Graeff/13Pixels.

namespace ThirteenPixels.Soda
{
    using UnityEngine;

    /// <summary>
    /// A GlobalVariable representing a Interactable.
    /// </summary>
    [CreateAssetMenu(menuName = "Soda/GlobalVariable/Interactable", order = 200)]
    public class GlobalInteractable : GlobalVariableBase<Interactable>
    {

    }

    /// <summary>
    /// A ScopedVariable representing either a GlobalInteractable or a local Interactable value.
    /// </summary>
    [System.Serializable]
    public class ScopedInteractable : ScopedVariableBase<Interactable, GlobalInteractable>
    {

    }
}
