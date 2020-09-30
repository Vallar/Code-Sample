using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField] private UnitStats stats;
    private DefendAction action;
    private CircleCollider2D col;

    private void Awake()
    {
        col = GetComponent<CircleCollider2D>();
        action = GetComponent<DefendAction>();
    }

    private void OnEnable()
    {
        col.radius = stats.actionRange;
    }

    private void OnTriggerStay2D(Collider2D _collision)
    {
        AvatarStatsHandler stats = _collision.GetComponent<AvatarStatsHandler>();

        if (stats != null && action.target == null)
            action.target = stats;
    }
}
