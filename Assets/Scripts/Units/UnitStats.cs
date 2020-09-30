using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Unit Stats")]
public class UnitStats : ScriptableObject
{
    public Sprite icon;
    public int hp;
    public int cost;
    public int actionAmount;
    public float actionRange;
    public float actionInterval;
}
