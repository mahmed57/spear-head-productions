using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class ProceduralLevelGenerator : MonoBehaviour
{
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

    private List<RectInt> rooms;
    private Vector3Int dungeonOffset;

    // New: HashSet to keep track of floor positions
    private HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();

    void Start()
    {
        rooms = new List<RectInt>();
        GenerateRooms();
        CalculateDungeonOffset();
        ApplyDungeonOffset();
        GenerateCorridors();
        DrawFloors();
        DrawWalls();
    }

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

            RectInt newRoom = new RectInt(x, y, width, height);

            bool overlaps = false;
            foreach (RectInt room in rooms)
            {
                if (newRoom.Overlaps(room))
                {
                    overlaps = true;
                    break;
                }
            }

            if (!overlaps)
            {
                rooms.Add(newRoom);
            }
        }
    }

    void CalculateDungeonOffset()
    {
        Vector2Int firstRoomCenter = GetRoomCenter(rooms[0]);
        dungeonOffset = new Vector3Int(-firstRoomCenter.x, -firstRoomCenter.y, 0);
    }

    void ApplyDungeonOffset()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            RectInt room = rooms[i];
            room.position += new Vector2Int(dungeonOffset.x, dungeonOffset.y);
            rooms[i] = room;
        }
    }

    void GenerateCorridors()
    {
        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (RectInt room in rooms)
        {
            roomCenters.Add(GetRoomCenter(room));
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

    void DrawFloors()
    {
        foreach (RectInt room in rooms)
        {
            for (int x = room.xMin; x < room.xMax; x++)
            {
                for (int y = room.yMin; y < room.yMax; y++)
                {
                    Vector3Int tilePosition = new Vector3Int(x, y, 0);
                    floorTilemap.SetTile(tilePosition, floorTile);
                    // Add to floor positions
                    floorPositions.Add(tilePosition);
                }
            }
        }
    }

    void DrawWalls()
    {
        // Calculate bounds based on floorPositions
        if (floorPositions.Count == 0)
            return;

        int xMin = int.MaxValue, xMax = int.MinValue;
        int yMin = int.MaxValue, yMax = int.MinValue;

        foreach (Vector3Int pos in floorPositions)
        {
            if (pos.x < xMin) xMin = pos.x;
            if (pos.x > xMax) xMax = pos.x;
            if (pos.y < yMin) yMin = pos.y;
            if (pos.y > yMax) yMax = pos.y;
        }

        // Expand bounds by 1 to check surrounding tiles
        xMin -= 1; xMax += 1;
        yMin -= 1; yMax += 1;

        for (int x = xMin; x <= xMax; x++)
        {
            for (int y = yMin; y <= yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                if (!floorPositions.Contains(pos) && HasAdjacentFloor(pos))
                {
                    PlaceWall(pos);
                }
            }
        }
    }

    void PlaceWall(Vector3Int pos)
    {
        bool hasFloorLeft = floorPositions.Contains(pos + Vector3Int.left);
        bool hasFloorRight = floorPositions.Contains(pos + Vector3Int.right);
        bool hasFloorUp = floorPositions.Contains(pos + Vector3Int.up);
        bool hasFloorDown = floorPositions.Contains(pos + Vector3Int.down);

        // Corrected logic for placing walls based on adjacent floor tiles
        if (hasFloorUp || hasFloorDown)
        {
            wallTilemap.SetTile(pos, wallTopBottomTile);
        }
        else if (hasFloorLeft || hasFloorRight)
        {
            TileBase wallTile = hasFloorLeft ? wallLeftTile : wallRightTile;
            wallTilemap.SetTile(pos, wallTile);
        }
        else
        {
            // Place a default wall tile if necessary
            wallTilemap.SetTile(pos, wallTopBottomTile);
        }
    }

    bool HasAdjacentFloor(Vector3Int pos)
    {
        // Check in floorPositions instead of tilemap
        return floorPositions.Contains(pos + Vector3Int.up) ||
               floorPositions.Contains(pos + Vector3Int.down) ||
               floorPositions.Contains(pos + Vector3Int.left) ||
               floorPositions.Contains(pos + Vector3Int.right);
    }

    Vector2Int GetRoomCenter(RectInt room)
    {
        int x = Mathf.RoundToInt(room.xMin + room.width / 2f);
        int y = Mathf.RoundToInt(room.yMin + room.height / 2f);
        return new Vector2Int(x, y);
    }

    void CreateCorridor(Vector2Int from, Vector2Int to)
    {
        List<Vector2Int> path = GetShortestPath(from, to);

        foreach (Vector2Int position in path)
        {
            Vector3Int tilePosition = new Vector3Int(position.x, position.y, 0);
            floorTilemap.SetTile(tilePosition, floorTile);
            // Add to floor positions
            floorPositions.Add(tilePosition);
        }
    }

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
