using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThirteenPixels.Soda;
using ThirteenPixels.Pooling;

[RequireComponent(typeof(AvatarAgentMove))]
public class Unit : Interactable
{
    private PoolReturner returner;


    protected override void Awake()
    {
        returner = GetComponent<PoolReturner>();
        base.Awake();
    }

    protected override void Interact()
    {
        base.Interact();
    }

    public void DisableUnit()
    {
        returner.Return();
    }
}
