using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{

    public Vector3 worldPosition;
    public List<Vector3> neighbours = new List<Vector3>();

    public int gCost;
    public int hCost;
    public Node parent;
    int heapIndex;

    public Node(Vector3 _worldpos, List<Vector3> _neighbours) {
        worldPosition = _worldpos;
        neighbours = _neighbours;
    }

    public int fCost
    {
        get
        {
            return gCost + fCost;
        }
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
