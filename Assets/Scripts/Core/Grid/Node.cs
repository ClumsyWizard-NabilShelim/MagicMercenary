using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeadItem<Node>
{
    public bool Walkable { get; private set; }
    public Vector2 WorldPosition { get; private set; }
    public int GridX {get; private set;}
    public int GridY { get; private set; }

    public int gCost; //Distance from starting node
    public int hCost; //Distance from end node
    public int fCost
	{
		get
		{
            return gCost + hCost;
		}
	}

	public int HeapIndex { get; set; }

	public Node parent;

    public Node(bool walkable, Vector2 worldPosition, int gridX, int gridY)
	{
        Walkable = walkable;
        WorldPosition = worldPosition;
        GridX = gridX;
        GridY = gridY;
	}

    public int CompareTo(Node nodeToCompare)
	{
        int compare = fCost.CompareTo(nodeToCompare.fCost);

        if (compare == 0)
            compare = hCost.CompareTo(nodeToCompare.hCost);

        return -compare;
	}
}
