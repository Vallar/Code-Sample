using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ThirteenPixels.Soda;

public class BuildUnit : MonoBehaviour
{
    [SerializeField] private UnitStats unitStats;
    [SerializeField] private BaseStats baseStats;
    [SerializeField] private GlobalTransform currentBaseWaypoint;
    [SerializeField] private Transform unit;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(Build);
    }

    private void Build()
    {
        if (baseStats.resources >= unitStats.cost)
        {
            baseStats.resources -= unitStats.cost;

            Transform obj = ThirteenPixels.Pooling.Pooling.GetObject(unit);
            obj.position = currentBaseWaypoint.value.position;
        }
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(Build);
    }
}
