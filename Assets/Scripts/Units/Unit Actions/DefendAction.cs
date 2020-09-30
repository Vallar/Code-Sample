using System.Collections;
using System.Collections.Generic;
using ThirteenPixels.Pooling;
using UnityEngine;

public class DefendAction : UnitAction
{
    public AvatarStatsHandler target;

    [SerializeField] private Transform shootingPoint;
    [SerializeField] private Projectile projectile;
    private Timer timer;

    private void OnEnable()
    {
        timer = new Timer(stats.actionInterval);
    }

    public override void ApplyAction()
    {
        base.ApplyAction();
    }

    private void Update()
    {
        if (target == null)
            return;

        if (target.gameObject.activeSelf == false)
            target = null;

        if (timer.IsTimerUp())
        {
            Projectile currentProjectile = Pooling.GetObject(projectile);

            currentProjectile.transform.position = shootingPoint.position;
            currentProjectile.gameObject.layer = gameObject.layer;
            currentProjectile.Fire(target.transform, stats.actionAmount);
        }
    }
}
