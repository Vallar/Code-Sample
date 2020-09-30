using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSingleTargetMelee : AvatarAttack
{

    public override void Attack(AvatarStatsHandler _target)
    {
        base.Attack(_target);
    }

    protected override IEnumerator AttackLoop()
    {
        while (target != null)
        {
            if(Vector2.Distance(target.transform.position, transform.position) <= stats.actionRange)
            {
                target.TakeDamage(stats.actionAmount);

                yield return new WaitForSeconds(stats.actionInterval);
            }
        }
    }
}
