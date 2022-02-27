using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
	[SerializeField] private LayerMask obstacleLayers;
	[SerializeField] private LayerMask walkableLayers;
	[SerializeField] private Vector2 gridBounds;
	[SerializeField] private float nodeRadius;
	private Node[,] grid;

	float nodeDiameter;
	int gridSizeX;
	int gridSizeY;

	public int GridMaxSize
	{
		get
		{
			return gridSizeX * gridSizeY;
		}
	}

	private void Awake()
	{
		InitGrid();
	}

	private void InitGrid()
	{
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridBounds.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridBounds.y / nodeDiameter);

		CreateGrid();
	}

	public void ClearNodeStats()
	{
		for (int x = 0; x < grid.GetLength(0); x++)
		{
			for (int y = 0; y < grid.GetLength(1); y++)
			{
				Node node = grid[x, y];
				node.gCost = int.MaxValue;
				node.hCost = 0;
				node.parent = null;
			}
		}
	}

	private void CreateGrid()
	{
		grid = new Node[gridSizeX, gridSizeY];
		Vector2 worldBottomLeft = (Vector2)transform.position - (Vector2.right * (gridBounds.x / 2.0f)) - (Vector2.up * (gridBounds.y / 2.0f));

		for (int x = 0; x < gridSizeX; x++)
		{
			for (int y = 0; y < gridSizeY; y++)
			{
				Vector2 worldPosition = worldBottomLeft + (Vector2.right * (x * nodeDiameter + nodeRadius)) + (Vector2.up * (y * nodeDiameter + nodeRadius));
				bool walkable = false;

				Collider2D col = Physics2D.OverlapCircle(worldPosition, 0.1f, walkableLayers);

				if(col != null)
					walkable = !Physics2D.OverlapCircle(worldPosition, nodeRadius, obstacleLayers);

				grid[x, y] = new Node(walkable, worldPosition, x, y);
			}
		}
	}

	public Node NodeFromWorldPosition(Vector2 worldPosition)
	{
		int x = Mathf.FloorToInt(worldPosition.x + Mathf.RoundToInt(gridBounds.x / 2));
		int y = Mathf.FloorToInt(worldPosition.y + Mathf.RoundToInt(gridBounds.y / 2));

		return grid[x, y];
	}

	public List<Node> GetNeighbours(Node centreNode)
	{
		List<Node> neighbourNodes = new List<Node>();

		for (int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{
				if (x == 0 && y == 0)
					continue;

				int CheckX = centreNode.GridX + x;
				int CheckY = centreNode.GridY + y;

				if (CheckX >= 0 && CheckX < gridSizeX && CheckY >= 0 && CheckY < gridSizeY)
					neighbourNodes.Add(grid[CheckX, CheckY]);
			}
		}

		return neighbourNodes;
	}

	//Debug
	[Range(0.1f, 1.0f)]
	public float DEBUG_GridOpacity = 0.1f;
	[HideInInspector] public bool isGridVisible;

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireCube(transform.position, gridBounds);

		if (grid == null)
			return;

		foreach (Node node in grid)
		{
			Gizmos.color = (node.Walkable) ? new Color(0.0f, 0.0f, 1.0f, DEBUG_GridOpacity) : new Color(1.0f, 0.0f, 0.0f, DEBUG_GridOpacity);
			Gizmos.DrawCube(node.WorldPosition, Vector3.one * (nodeDiameter));
			Gizmos.color = Color.black;
			Gizmos.DrawWireCube(node.WorldPosition, Vector3.one * (nodeDiameter));
		}
	}

	public void DEBUG_InitGrid()
	{
		DEBUG_ClearGrid();
		InitGrid();
	}

	public void DEBUG_ClearGrid()
	{
		grid = null;

		nodeDiameter = 0.0f;
		gridSizeX = 0;
		gridSizeY = 0;
	}
}