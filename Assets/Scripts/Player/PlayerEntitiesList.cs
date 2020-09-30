using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntitiesList : MonoBehaviour
{
    public static List<Transform> entities = new List<Transform>();

    private void OnDisable()
    {
        entities.Clear();
    }
}
