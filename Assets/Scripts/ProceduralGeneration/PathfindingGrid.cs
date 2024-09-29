using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class PathfindingGrid : MonoBehaviour
{
    public Tilemap floorTilemap;
    public Tilemap wallTilemap;

    public Dictionary<Vector3Int, GridNode> nodes = new Dictionary<Vector3Int, GridNode>();

    void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        // Get bounds of the floor tilemap
        BoundsInt bounds = floorTilemap.cellBounds;

        // Iterate over each position in the bounds
        for (int x = bounds.xMin; x <= bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y <= bounds.yMax; y++)
            {
                Vector3Int gridPosition = new Vector3Int(x, y, 0);

                // Check if the position is walkable (floor tile exists and no wall tile)
                bool isWalkable = floorTilemap.HasTile(gridPosition) && !wallTilemap.HasTile(gridPosition);

                // Get the world position of the cell center
                Vector3 worldPosition = floorTilemap.CellToWorld(gridPosition) + floorTilemap.cellSize / 2;

                // Create a new GridNode and add it to the dictionary
                GridNode node = new GridNode(gridPosition, worldPosition, isWalkable);
                nodes[gridPosition] = node;
            }
        }
    }

    public GridNode GetNodeAtPosition(Vector3 position)
    {
        Vector3Int gridPosition = floorTilemap.WorldToCell(position);
        nodes.TryGetValue(gridPosition, out GridNode node);
        return node;
    }

    public List<GridNode> GetNeighbors(GridNode node)
    {
        List<GridNode> neighbors = new List<GridNode>();

        Vector3Int[] directions = new Vector3Int[]
        {
            new Vector3Int(-1,  0, 0), // Left
            new Vector3Int( 1,  0, 0), // Right
            new Vector3Int( 0, -1, 0), // Down
            new Vector3Int( 0,  1, 0), // Up
        };

        foreach (var direction in directions)
        {
            Vector3Int neighborPos = node.gridPosition + direction;
            if (nodes.TryGetValue(neighborPos, out GridNode neighbor) && neighbor.isWalkable)
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }
}
