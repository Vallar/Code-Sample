using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeleeAI : EnemyAI
{
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override IEnumerator AILoop()
    {
        while (currentStats.hp > 0)
        {
            if (currentTargetStats == null)
            {
                //int random = Random.Range(1, playerEntities.value.entities.Count);

                currentTargetStats = PlayerEntitiesList.entities[0].GetComponent<AvatarStatsHandler>();

                currentTarget = currentTargetStats.transform;
            }

            float distance = Vector2.Distance(transform.position, currentTarget.position);

            if (movement.hasDestination == false && distance > currentStats.actionRange)
                movement.MoveToDestination(currentTargetStats.transform.position);
            
            while (distance <= currentStats.actionRange)
            {
                attack.Attack(currentTargetStats);

                yield return new WaitForSeconds(currentStats.actionInterval);
            }

            yield return 0;
        }
    }
}
