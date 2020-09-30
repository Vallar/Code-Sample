using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherAction : UnitAction
{
    [SerializeField] private BaseStats baseStats;
    [SerializeField] private bool isGathering = false;
    private Timer timer;
    private AvatarAgentMove moveUnit;

    private void Awake()
    {
        moveUnit = GetComponent<AvatarAgentMove>();
    }

    private void OnEnable()
    {
        timer = new Timer(stats.actionInterval);
    }

    public override void ApplyAction()
    {
        moveUnit.hasDestination = false;
        isGathering = true;
    }

    public void StopAction()
    {
        moveUnit.hasDestination = true;
        isGathering = false;
    }

    private void Update()
    {
        if (isGathering == false)
            return;

        if (timer.IsTimerUp())
        {
            baseStats.resources += stats.actionAmount;
        }
    }
}
