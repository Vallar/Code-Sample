using System.Collections;
using System.Collections.Generic;
using ThirteenPixels.Soda;
using UnityEngine;
using UnityEngine.UI;
using ThirteenPixels.Pooling;

public class TransformUnitType : MonoBehaviour
{
    [SerializeField] private GlobalInteractable interactable;
    [SerializeField] private UnitAction unitAction;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (interactable.value != null && interactable.value is Unit)
        {
            UnitAction unit = Pooling.GetObject(unitAction);

            Unit oldUnit = interactable.value as Unit;
            unit.transform.position = oldUnit.transform.position;
            oldUnit.DisableUnit();

            interactable.value = unit.GetComponent<Unit>();
            interactable.value.ToggleSelectionEffect();
        }
    }

    private void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }
}
