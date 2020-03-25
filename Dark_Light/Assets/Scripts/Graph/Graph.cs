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
}
