using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public List<Transform> nodes;

    private int numNodes;
    private static Graph instance;

    public static Graph Instance
    {
        get
        {
            return instance;
        }
    }


    private void Awake()
    {
        instance = this;
        numNodes = nodes.Count;
    }

    public Transform returnRandomNode()
    {
        int rand = Random.Range(0, numNodes - 1);
        return nodes[rand];
    }

    public Vector3 GetFarestPoint(Vector3 enemy)
    {
        float dist = 0f;
        int index = 0;

        for (int i = 0; i < nodes.Count; i++)
        {
            float newDist = Vector3.Distance(nodes[i].position, enemy);
            if ( newDist > dist)
            {
                dist = newDist;
                index = i;
            }
        }

        return nodes[index].position;
    }
}
