using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AvatarAgentMove : AvatarMovement
{
    private IAstarAI agent;

    private void Awake()
    {
        agent = GetComponent<IAstarAI>();
    }

    public override void MoveToDestination(Vector2 _position)
    {
        hasDestination = true;

        agent.destination = _position;

        agent.SearchPath();
    }

    private void Update()
    {
        if (agent.reachedDestination && hasDestination)
            hasDestination = false;
        else
            return;
    }
}
