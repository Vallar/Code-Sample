﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAction : MonoBehaviour
{
    [SerializeField] protected UnitStats stats;

    public virtual void ApplyAction() { }
}
