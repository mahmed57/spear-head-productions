using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

/// <summary>
/// Generates a procedural dungeon with rooms and corridors.
/// </summary>
public class ProceduralLevelGenerator : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public string name;
        public GameObject enemyPrefab;
        public int minEnemiesPerRoom;
        public int maxEnemiesPerRoom;
    }

    [Header("Enemy Settings")]
    public List<EnemyType> enemyTypes;

    [Header("Tilemap and Tiles")]
    public Tilemap floorTilemap;  // Floor Tilemap
    public Tilemap wallTilemap;   // Wall Tilemap
    public TileBase floorTile;
    public TileBase wallLeftTile;
    public TileBase wallRightTile;
    public TileBase wallTopBottomTile;

    [Header("Dungeon Generation Parameters")]
    public int roomCount = 10;
    public int minRoomSize = 4;
    public int maxRoomSize = 8;
    public int mapWidth = 50;
    public int mapHeight = 50;

    [Header("Room Prefab")]
    public GameObject roomPrefab; // Assign this in the Inspector

    private List<Room> rooms;
    private Vector3Int dungeonOffset;

    // HashSet to keep track of floor positions
    private HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();

    void Start()
    {
        rooms = new List<Room>();
        GenerateRooms();
        CalculateDungeonOffset();
        ApplyDungeonOffset();
        GenerateCorridors();
        DrawFloors();
        DrawWalls();
        InstantiateRoomGameObjects();
    }

    /// <summary>
    /// Generates random rooms within the dungeon bounds.
    /// </summary>
    void GenerateRooms()
    {
        int maxAttempts = roomCount * 5;
        int attempts = 0;

        while (rooms.Count < roomCount && attempts < maxAttempts)
        {
            attempts++;

            int width = Random.Range(minRoomSize, maxRoomSize + 1);
            int height = Random.Range(minRoomSize, maxRoomSize + 1);
            int x = Random.Range(0, mapWidth - width);
            int y = Random.Range(0, mapHeight - height);

            RectInt newRect = new RectInt(x, y, width, height);

            bool overlaps = false;
            foreach (Room room in rooms)
            {
                if (newRect.Overlaps(room.bounds))
                {
                    overlaps = true;
                    break;
                }
            }

            if (!overlaps)
            {
                Room newRoom = new Room(newRect);
                rooms.Add(newRoom);
            }
        }
    }

    /// <summary>
    /// Calculates the offset to center the dungeon around (0,0).
    /// </summary>
    void CalculateDungeonOffset()
    {
        Vector2Int firstRoomCenter = GetRoomCenter(rooms[0].bounds);
        dungeonOffset = new Vector3Int(-firstRoomCenter.x, -firstRoomCenter.y, 0);
    }

    /// <summary>
    /// Applies the calculated offset to all rooms.
    /// </summary>
    void ApplyDungeonOffset()
    {
        foreach (Room room in rooms)
        {
            room.bounds.position += new Vector2Int(dungeonOffset.x, dungeonOffset.y);
        }
    }

    /// <summary>
    /// Generates corridors connecting the rooms using a minimum spanning tree.
    /// </summary>
    void GenerateCorridors()
    {
        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (Room room in rooms)
        {
            roomCenters.Add(GetRoomCenter(room.bounds));
        }

        List<Edge> allEdges = new List<Edge>();
        for (int i = 0; i < roomCenters.Count; i++)
        {
            for (int j = i + 1; j < roomCenters.Count; j++)
            {
                float distance = Vector2Int.Distance(roomCenters[i], roomCenters[j]);
                allEdges.Add(new Edge(roomCenters[i], roomCenters[j], distance));
            }
        }

        allEdges.Sort((a, b) => a.distance.CompareTo(b.distance));

        DisjointSet ds = new DisjointSet(roomCenters);
        List<Edge> mst = new List<Edge>();

        foreach (Edge edge in allEdges)
        {
            if (ds.Find(edge.from) != ds.Find(edge.to))
            {
                ds.Union(edge.from, edge.to);
                mst.Add(edge);
            }
        }

        foreach (Edge edge in mst)
        {
            CreateCorridor(edge.from, edge.to);
        }
    }

    /// <summary>
    /// Draws the floor tiles for all rooms.
    /// </summary>
    void DrawFloors()
    {
        foreach (Room room in rooms)
        {
            for (int x = room.bounds.xMin; x < room.bounds.xMax; x++)
            {
                for (int y = room.bounds.yMin; y < room.bounds.yMax; y++)
                {
                    Vector3Int tilePosition = new Vector3Int(x, y, 0);
                    floorTilemap.SetTile(tilePosition, floorTile);
                    floorPositions.Add(tilePosition);

                    // Add to the room's floor tiles
                    room.floorTiles.Add(tilePosition);
                }
            }
        }
    }

    /// <summary>
    /// Draws walls around the floor tiles.
    /// </summary>
    void DrawWalls()
    {
        // For each floor position, check adjacent positions for walls
        foreach (Vector3Int floorPos in floorPositions)
        {
            // Check all four directions (up, down, left, right) for wall placement
            TryPlaceWall(floorPos + Vector3Int.up);
            TryPlaceWall(floorPos + Vector3Int.down);
            TryPlaceWall(floorPos + Vector3Int.left);
            TryPlaceWall(floorPos + Vector3Int.right);
        }
    }

    /// <summary>
    /// Attempts to place a wall tile at the specified position.
    /// </summary>
    void TryPlaceWall(Vector3Int pos)
    {
        // Only place wall if there's no floor at this position and no existing wall
        if (!floorPositions.Contains(pos) && !wallTilemap.HasTile(pos))
        {
            // Determine which wall tile to place based on adjacent floor tiles
            bool hasFloorLeft = floorPositions.Contains(pos + Vector3Int.left);
            bool hasFloorRight = floorPositions.Contains(pos + Vector3Int.right);
            bool hasFloorUp = floorPositions.Contains(pos + Vector3Int.up);
            bool hasFloorDown = floorPositions.Contains(pos + Vector3Int.down);

            // Place appropriate wall tiles based on surrounding floors
            if (hasFloorUp || hasFloorDown)
            {
                wallTilemap.SetTile(pos, wallTopBottomTile);
            }
            else if (hasFloorLeft || hasFloorRight)
            {
                TileBase wallTile = hasFloorLeft ? wallLeftTile : wallRightTile;
                wallTilemap.SetTile(pos, wallTile);
            }
        }
    }

    /// <summary>
    /// Gets the center position of a room.
    /// </summary>
    Vector2Int GetRoomCenter(RectInt room)
    {
        int x = Mathf.RoundToInt(room.xMin + (room.width - 1) / 2f);
        int y = Mathf.RoundToInt(room.yMin + (room.height - 1) / 2f);
        return new Vector2Int(x, y);
    }

    /// <summary>
    /// Creates a corridor between two points.
    /// </summary>
    void CreateCorridor(Vector2Int from, Vector2Int to)
    {
        List<Vector2Int> path = GetShortestPath(from, to);

        for (int i = 0; i < path.Count; i++)
        {
            Vector2Int position = path[i];
            Vector3Int tilePosition = new Vector3Int(position.x, position.y, 0);
            floorTilemap.SetTile(tilePosition, floorTile);
            floorPositions.Add(tilePosition);

            // Determine direction of movement
            Vector2Int direction;
            if (i == 0)
            {
                if (path.Count > 1)
                {
                    direction = path[i + 1] - position;
                }
                else
                {
                    direction = Vector2Int.right; // Default direction
                }
            }
            else
            {
                direction = position - path[i - 1];
            }

            // Get perpendicular direction
            Vector2Int perpendicular = new Vector2Int(-direction.y, direction.x);
            if (perpendicular == Vector2Int.zero)
            {
                perpendicular = Vector2Int.right; // Default perpendicular direction
            }

            // Place additional floor tile to widen the corridor
            Vector2Int adjacentPosition = position + perpendicular;
            Vector3Int tilePositionAdj = new Vector3Int(adjacentPosition.x, adjacentPosition.y, 0);

            // Check if the tile hasn't been set already
            if (!floorPositions.Contains(tilePositionAdj))
            {
                floorTilemap.SetTile(tilePositionAdj, floorTile);
                floorPositions.Add(tilePositionAdj);
            }
        }
    }

    /// <summary>
    /// Gets the shortest path between two points.
    /// </summary>
    List<Vector2Int> GetShortestPath(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int current = start;

        while (current != end)
        {
            if (current.x != end.x)
            {
                int direction = end.x > current.x ? 1 : -1;
                current = new Vector2Int(current.x + direction, current.y);
            }
            else if (current.y != end.y)
            {
                int direction = end.y > current.y ? 1 : -1;
                current = new Vector2Int(current.x, current.y + direction);
            }

            path.Add(current);
        }

        return path;
    }

    /// <summary>
    /// Instantiates room GameObjects with trigger colliders.
    /// </summary>
    /// 
    void InstantiateRoomGameObjects()
    {
        foreach (Room room in rooms)
        {
            // Access the Grid component associated with the tilemap
            Grid grid = floorTilemap.layoutGrid;

            // Get the bottom-left corner of the room in cell coordinates
            Vector3Int roomCellPosition = new Vector3Int(room.bounds.xMin, room.bounds.yMin, 0);

            // Convert the cell position to world position using the grid
            Vector3 roomWorldPosition = grid.CellToWorld(roomCellPosition);

            // Get the cell size from the tilemap
            Vector3 cellSize = floorTilemap.cellSize;

            // Calculate the room's world size based on cell size and room dimensions
            Vector3 roomWorldSize = new Vector3(room.bounds.width * cellSize.x, room.bounds.height * cellSize.y, 0);

            // Calculate the center position of the room in world coordinates
            Vector3 roomCenterPosition = roomWorldPosition + new Vector3(roomWorldSize.x / 2f, roomWorldSize.y / 2f, 0);

            // Instantiate the room GameObject at the calculated center position
            GameObject roomGO = Instantiate(roomPrefab, roomCenterPosition, Quaternion.identity);
            roomGO.name = "Room_" + roomCenterPosition;

            // Get the BoxCollider2D component and set its size to match the room's world size
            BoxCollider2D collider = roomGO.GetComponent<BoxCollider2D>();
            collider.size = new Vector2(roomWorldSize.x, roomWorldSize.y);
            collider.offset = Vector2.zero; // Ensure the collider is centered

            // Assign references to the RoomController script
            RoomController roomController = roomGO.GetComponent<RoomController>();
            roomController.room = room;
            roomController.floorTilemap = floorTilemap;
            roomController.enemyTypes = enemyTypes;
        }
    }


    /// <summary>
    /// Represents an edge between two points, used for corridor generation.
    /// </summary>
    class Edge
    {
        public Vector2Int from;
        public Vector2Int to;
        public float distance;

        public Edge(Vector2Int from, Vector2Int to, float distance)
        {
            this.from = from;
            this.to = to;
            this.distance = distance;
        }
    }

    /// <summary>
    /// Disjoint Set (Union-Find) data structure for Kruskal's algorithm.
    /// </summary>
    class DisjointSet
    {
        private Dictionary<Vector2Int, Vector2Int> parent = new Dictionary<Vector2Int, Vector2Int>();

        public DisjointSet(List<Vector2Int> elements)
        {
            foreach (Vector2Int elem in elements)
            {
                parent[elem] = elem;
            }
        }

        public Vector2Int Find(Vector2Int item)
        {
            if (parent[item] != item)
            {
                parent[item] = Find(parent[item]);
            }
            return parent[item];
        }

        public void Union(Vector2Int itemA, Vector2Int itemB)
        {
            Vector2Int rootA = Find(itemA);
            Vector2Int rootB = Find(itemB);

            if (rootA != rootB)
            {
                parent[rootB] = rootA;
            }
        }
    }
}
