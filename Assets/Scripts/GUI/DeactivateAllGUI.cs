using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThirteenPixels.Soda;

public class DeactivateAllGUI : MonoBehaviour
{
    [SerializeField] private GameEvent deactivateGUI;

    private void OnEnable()
    {
        deactivateGUI.onRaise.AddResponse(() => gameObject.SetActive(false));
    }
}
