using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public List<NodeEditor> nodes;

    List<Node> graph;
    int graphSize;

    private void Awake()
    {
        createGraph();
    }

    void createGraph()
    {
        graph = new List<Node>();
    }
}
