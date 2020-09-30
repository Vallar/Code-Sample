using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Base Stats")]
public class BaseStats : ScriptableObject
{
    [SerializeField] private int maxHP;
    public int resources;
    public int currentHP;

    private void OnEnable()
    {
        resources = 50;
        currentHP = maxHP;
    }
}
