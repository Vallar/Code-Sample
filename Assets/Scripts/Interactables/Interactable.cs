using System.Collections;
using System.Collections.Generic;
using ThirteenPixels.Soda;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected GlobalInteractable currentActiveInteractable;
    [SerializeField] protected Color selectionColor = Color.blue;
    protected SpriteRenderer sr;

    protected virtual void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        PlayerEntitiesList.entities.Add(transform);
    }

    protected virtual void Interact()
    {
        if(currentActiveInteractable.value != null)
            currentActiveInteractable.value.ToggleSelectionEffect();

        ToggleSelectionEffect();

        currentActiveInteractable.value = this;
    }

    protected virtual void OnMouseDown()
    {
        Interact();
    }

    public virtual void ToggleSelectionEffect()
    {
        if (sr.color == selectionColor)
            sr.color = Color.white;
        else
            sr.color = selectionColor;
    }

    private void OnDisable()
    {
        PlayerEntitiesList.entities.Remove(transform);
    }
}
