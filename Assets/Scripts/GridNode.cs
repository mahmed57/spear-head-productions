using UnityEngine;
using System.Collections;
public class GridNode
{
    public Vector3Int gridPosition;     // Position in grid coordinates
    public Vector3 worldPosition;       // Position in world coordinates
    public bool isWalkable;             // Whether the node is walkable
    public int gCost;                   // Cost from start node
    public int hCost;                   // Heuristic cost to end node
    public GridNode parent;             // Parent node in the path

    public int FCost => gCost + hCost;

    public GridNode(Vector3Int gridPosition, Vector3 worldPosition, bool isWalkable)
    {
        this.gridPosition = gridPosition;
        this.worldPosition = worldPosition;
        this.isWalkable = isWalkable;
    }
}
