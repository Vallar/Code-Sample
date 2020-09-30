using System.Collections;
using System.Collections.Generic;
using ThirteenPixels.Soda;
using UnityEngine;

public class ToggleObjectListener : MonoBehaviour
{
    [SerializeField] private GameEvent targetEvent;
    [SerializeField] private bool selfDeactivateOnStart;

    private void OnEnable()
    {
        targetEvent.onRaise.AddResponse(Action);
    }

    private void Start()
    {
        if (selfDeactivateOnStart)
            Action();
    }

    private void Action()
    {
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        targetEvent.onRaise.RemoveResponse(Action);
    }
}
