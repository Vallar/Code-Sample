using System.Collections;
using System.Collections.Generic;
using ThirteenPixels.Soda;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] protected UnitStats stats;
    [SerializeField] protected AvatarAttack attack;
    [SerializeField] protected AvatarMovement movement;
    protected UnitStats currentStats;
    protected AvatarStatsHandler currentTargetStats;
    protected Transform currentTarget;
    protected new Transform transform;

    protected virtual void Awake()
    {
        attack = GetComponent<AvatarAttack>();
        movement = GetComponent<AvatarMovement>();
        transform = base.transform;
    }

    protected virtual void OnEnable()
    {
        currentStats = Instantiate(stats);

        StartCoroutine(AILoop());
    }

    protected virtual IEnumerator AILoop()
    {
        yield return 0;
    }
}
