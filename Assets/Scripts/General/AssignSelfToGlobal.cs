using System.Collections;
using System.Collections.Generic;
using ThirteenPixels.Soda;
using UnityEngine;

public class AssignSelfToGlobal : MonoBehaviour
{
    [SerializeField] private GlobalTransform globalTransform;

    private void OnEnable()
    {
        globalTransform.value = transform;
    }
}
