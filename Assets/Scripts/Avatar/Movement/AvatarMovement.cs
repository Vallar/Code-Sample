using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarMovement : MonoBehaviour
{
    [HideInInspector] public bool hasDestination = false;

    [SerializeField] protected UnitStats stats;

    public virtual void MoveToDestination(Vector2 _destination)
    {

    }
}
