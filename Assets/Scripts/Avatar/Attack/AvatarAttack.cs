using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarAttack : MonoBehaviour
{
    [SerializeField] protected UnitStats stats;
    [SerializeField] protected bool canChangeTarget;
    protected AvatarStatsHandler target;
    protected new Transform transform;

    private void Awake()
    {
        transform = base.transform;
    }

    public virtual void Attack(AvatarStatsHandler _target)
    {
        if (_target == null)
            target = _target;

        if (target != _target && canChangeTarget == false)
            return;

        if (target != _target && canChangeTarget)
            target = _target;

        StartCoroutine(AttackLoop());
    }

    protected virtual IEnumerator AttackLoop()
    {
        yield return 0;
    }
}
