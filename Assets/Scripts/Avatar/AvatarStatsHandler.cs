using System.Collections;
using System.Collections.Generic;
using ThirteenPixels.Pooling;
using UnityEngine;

public class AvatarStatsHandler : MonoBehaviour
{
    [SerializeField] private UnitStats stats;
    private UnitStats currentStats;
    private PoolReturner returner;

    private void Awake()
    {
        returner = GetComponent<PoolReturner>();
    }

    private void OnEnable()
    {
        currentStats = Instantiate(stats);
    }

    public void TakeDamage(int _damage)
    {
        if (currentStats.hp - _damage > 0)
            currentStats.hp -= _damage;
        else
        {
            currentStats.hp = 0;

            returner.Return();
        }

        //TODO: Show some effects and damage on UI.
    }

    public void Heal(int _amount)
    {
        if (currentStats.hp + _amount >= stats.hp)
            currentStats.hp = stats.hp;
        else
            currentStats.hp += _amount;

        //TODO: Show it on UI.
    }
}
