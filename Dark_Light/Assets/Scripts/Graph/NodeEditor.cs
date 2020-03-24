using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeEditor : MonoBehaviour
{
    public List<Transform> neightbours;

    private void Update()
    {
        for (int i = 0; i < neightbours.Count; i++)
        {
            Debug.DrawLine(transform.position, neightbours[i].position, Color.red);
        }
    }
}
