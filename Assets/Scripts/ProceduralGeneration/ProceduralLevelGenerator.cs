using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class ProceduralLevelGenerator : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public string name = "element";
        public string prefab_name;
        public int minEnemiesPerRoom;
        public int maxEnemiesPerRoom;

        public EnemyType(string prefab_name, int minEnemiesPerRoom, int maxEnemiesPerRoom)
        {
            this.prefab_name = prefab_name;
            this.minEnemiesPerRoom = minEnemiesPerRoom;
            this.maxEnemiesPerRoom = maxEnemiesPerRoom;
        }
    }

    [System.Serializable]
    public class Room_Enemy_Settings
    {
        public List<EnemyType> enemyTypes;
        public int minRoomSize;
        public int maxRoomSize;

        public Room_Enemy_Settings()
        {
            this.minRoomSize = 8;
            this.maxRoomSize = 15;

            this.enemyTypes = new List<EnemyType>
            {
                new EnemyType("Despair", 3, 3),
                new EnemyType("RageTanker", 3, 3)
            };
        }
    }

    public Vector3 left_most_room = new Vector3(0, 0, 0);

    public GameObject player;

    [Header("Spawner Object")]
    public GameObject spawner;

    [Header("Room and Enemy Settings")]
    public List<Room_Enemy_Settings> room_enemy_settings;

    [Header("Tilemap and Tiles")]
    public Tilemap floorTilemap;
    public Tilemap wallTilemap;
    public TileBase floorTile;
    public TileBase wallLeftTile;
    public TileBase wallRightTile;
    public TileBase wallTopTile;
    public TileBase wallBottomTile;

    // Corner tiles
    public TileBase wallCornerTopLeftTile;
    public TileBase wallCornerTopRightTile;
    public TileBase wallCornerBottomLeftTile;
    public TileBase wallCornerBottomRightTile;

    [Header("Barrier Prefab")]
    public GameObject barrierPrefab; // Added barrierPrefab

    [Header("Dungeon Generation Parameters")]
    private int roomCount = 10;
    public int mapWidth = 50;
    public int mapHeight = 50;

    public int minDistanceBetweenRooms = 2;

    [Header("Room Prefab")]
    public GameObject roomPrefab;

    public List<Room> rooms;
    private Vector3Int dungeonOffset;

    private HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();

    private bool is_active_player = false;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        roomCount = room_enemy_settings.Count;
        rooms = new List<Room>();
        GenerateRooms();
        CalculateDungeonOffset();
        ApplyDungeonOffset();
        GenerateCorridors();
        DrawFloors();
        DrawWalls();
        PlaceRoomCorners();
        IdentifyRoomDoors(); 
        PlaceBarriers();
        InstantiateRoomGameObjects();
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerStatistics>().start_pos = left_most_room;
        
    }

    void Update()
    {
        if(!is_active_player)
        {
            is_active_player = true;
            player.transform.position =left_most_room;
            player.SetActive(true);
            
        }
    }

    void GenerateRooms()
    {
        int maxAttempts = roomCount * 20;
        int attempts = 0;
        int index = 0;

        while (rooms.Count < roomCount && attempts < maxAttempts)
        {
            attempts++;

            int width = Random.Range(room_enemy_settings[index].minRoomSize, room_enemy_settings[index].maxRoomSize + 1);
            int height = Random.Range(room_enemy_settings[index].minRoomSize, room_enemy_settings[index].maxRoomSize + 1);

            int xMin = minDistanceBetweenRooms;
            int xMax = mapWidth - width - minDistanceBetweenRooms;
            int yMin = minDistanceBetweenRooms;
            int yMax = mapHeight - height - minDistanceBetweenRooms;

            if (xMax < xMin || yMax < yMin)
            {
                continue;
            }

            int x = Random.Range(xMin, xMax + 1);
            int y = Random.Range(yMin, yMax + 1);

            RectInt newRect = new RectInt(x, y, width, height);

            bool overlaps = false;
            foreach (Room room in rooms)
            {
                RectInt expandedRoomRect = new RectInt(
                    room.bounds.x - minDistanceBetweenRooms,
                    room.bounds.y - minDistanceBetweenRooms,
                    room.bounds.width + 2 * minDistanceBetweenRooms,
                    room.bounds.height + 2 * minDistanceBetweenRooms
                );

                if (expandedRoomRect.Overlaps(newRect))
                {
                    overlaps = true;
                    break;
                }
            }

            if (!overlaps)
            {
                Room newRoom = new Room(newRect);
                rooms.Add(newRoom);
                index++;
            }
        }
    }

    void CalculateDungeonOffset()
    {
        Vector2Int firstRoomCenter = GetRoomCenter(rooms[0].bounds);
        dungeonOffset = new Vector3Int(-firstRoomCenter.x, -firstRoomCenter.y, 0);
    }

    void ApplyDungeonOffset()
    {
        foreach (Room room in rooms)
        {
            room.bounds.position += new Vector2Int(dungeonOffset.x, dungeonOffset.y);
        }
    }

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

                    room.floorTiles.Add(tilePosition);
                }
            }
        }
    }

    void DrawWalls()
    {
        foreach (Vector3Int floorPos in floorPositions)
        {
            TryPlaceWall(floorPos + Vector3Int.up);
            TryPlaceWall(floorPos + Vector3Int.down);
            TryPlaceWall(floorPos + Vector3Int.left);
            TryPlaceWall(floorPos + Vector3Int.right);
        }
    }

    void TryPlaceWall(Vector3Int pos)
    {
        if (!floorPositions.Contains(pos) && !wallTilemap.HasTile(pos))
        {
            bool hasFloorLeft = floorPositions.Contains(pos + Vector3Int.left);
            bool hasFloorRight = floorPositions.Contains(pos + Vector3Int.right);
            bool hasFloorUp = floorPositions.Contains(pos + Vector3Int.up);
            bool hasFloorDown = floorPositions.Contains(pos + Vector3Int.down);

            if (hasFloorUp)
            {
                wallTilemap.SetTile(pos, wallBottomTile);
            }
            else if (hasFloorDown)
            {
                wallTilemap.SetTile(pos, wallTopTile);
            }
            else if (hasFloorLeft)
            {
                wallTilemap.SetTile(pos, wallRightTile);
            }
            else if (hasFloorRight)
            {
                wallTilemap.SetTile(pos, wallLeftTile);
            }
        }
    }

    void PlaceRoomCorners()
    {
        foreach (Room room in rooms)
        {
            Vector3Int topLeft = new Vector3Int(room.bounds.xMin - 1, room.bounds.yMax, 0);
            Vector3Int topRight = new Vector3Int(room.bounds.xMax, room.bounds.yMax, 0);
            Vector3Int bottomLeft = new Vector3Int(room.bounds.xMin - 1, room.bounds.yMin - 1, 0);
            Vector3Int bottomRight = new Vector3Int(room.bounds.xMax, room.bounds.yMin - 1, 0);

            wallTilemap.SetTile(topLeft, wallCornerTopLeftTile);
            wallTilemap.SetTile(topRight, wallCornerTopRightTile);
            wallTilemap.SetTile(bottomLeft, wallCornerBottomLeftTile);
            wallTilemap.SetTile(bottomRight, wallCornerBottomRightTile);
        }
    }

    Vector2Int GetRoomCenter(RectInt room)
    {
        int x = Mathf.RoundToInt(room.xMin + (room.width - 1) / 2f);
        int y = Mathf.RoundToInt(room.yMin + (room.height - 1) / 2f);
        return new Vector2Int(x, y);
    }

    void CreateCorridor(Vector2Int from, Vector2Int to)
    {
        List<Vector2Int> path = GetShortestPath(from, to);

        for (int i = 0; i < path.Count; i++)
        {
            Vector2Int position = path[i];
            Vector3Int tilePosition = new Vector3Int(position.x, position.y, 0);
            floorTilemap.SetTile(tilePosition, floorTile);
            floorPositions.Add(tilePosition);

            Vector2Int direction;
            if (i == 0)
            {
                if (path.Count > 1)
                {
                    direction = path[i + 1] - position;
                }
                else
                {
                    direction = Vector2Int.right;
                }
            }
            else
            {
                direction = position - path[i - 1];
            }

            Vector2Int perpendicular = new Vector2Int(-direction.y, direction.x);
            if (perpendicular == Vector2Int.zero)
            {
                perpendicular = Vector2Int.right;
            }

            Vector2Int adjacentPosition = position + perpendicular;
            Vector3Int tilePositionAdj = new Vector3Int(adjacentPosition.x, adjacentPosition.y, 0);

            if (!floorPositions.Contains(tilePositionAdj))
            {
                floorTilemap.SetTile(tilePositionAdj, floorTile);
                floorPositions.Add(tilePositionAdj);
            }
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

    void InstantiateRoomGameObjects()
    {
        int index = 0;
        float min_room_x = float.MaxValue;
        
        foreach(Room room in rooms)
        {
            Grid grid = floorTilemap.layoutGrid;

            Vector3Int roomCellPosition = new Vector3Int(room.bounds.xMin, room.bounds.yMin, 0);

            Vector3 roomWorldPosition = grid.CellToWorld(roomCellPosition);

            Vector3 cellSize = floorTilemap.cellSize;

            Vector3 roomWorldSize = new Vector3(room.bounds.width * cellSize.x, room.bounds.height * cellSize.y, 0);

            Vector3 room_center_position = roomWorldPosition + new Vector3(roomWorldSize.x / 2f, roomWorldSize.y / 2f, 0);

            room.room_center_position = room_center_position;

            room.room_world_size = roomWorldSize;
        
        }

        rooms = rooms.OrderBy(r => r.room_center_position.x).ToList();

        foreach (Room room in rooms)
        {

            GameObject roomGO = Instantiate(roomPrefab, room.room_center_position, Quaternion.identity);
            roomGO.name = "Room_" + room.room_center_position;
            roomGO.tag = "Room";
            

            if(room.room_center_position.x < min_room_x)
            {
                Debug.Log("Leftx");
                left_most_room =room.room_center_position;
                min_room_x = room.room_center_position.x;
                Debug.Log(min_room_x);
                Debug.Log("Left");
                Debug.Log(left_most_room);
            }

            BoxCollider2D collider = roomGO.GetComponent<BoxCollider2D>();
            collider.size = new Vector2(room.room_world_size.x, room.room_world_size.y);
            collider.offset = Vector2.zero;

            RoomController roomController = roomGO.GetComponent<RoomController>();
            roomController.room = room;
            roomController.floorTilemap = floorTilemap;
            roomController.spawner = spawner;
            roomController.enemyTypes = room_enemy_settings[index].enemyTypes;

            roomController.barriers = room.barriers;

            index++;
        }

        
    }



    // Function to identify door positions
    void IdentifyRoomDoors()
    {
        foreach (Room room in rooms)
        {
            // Horizontal edges
            for (int x = room.bounds.xMin; x < room.bounds.xMax; x++)
            {
                Vector3Int topPosition = new Vector3Int(x, room.bounds.yMax, 0);
                Vector3Int bottomPosition = new Vector3Int(x, room.bounds.yMin - 1, 0);

                CheckDoorway(room, topPosition);
                CheckDoorway(room, bottomPosition);
            }

            // Vertical edges
            for (int y = room.bounds.yMin; y < room.bounds.yMax; y++)
            {
                Vector3Int leftPosition = new Vector3Int(room.bounds.xMin - 1, y, 0);
                Vector3Int rightPosition = new Vector3Int(room.bounds.xMax, y, 0);

                CheckDoorway(room, leftPosition);
                CheckDoorway(room, rightPosition);
            }
        }
    }

    void CheckDoorway(Room room, Vector3Int position)
    {
        if (floorPositions.Contains(position) && !room.floorTiles.Contains(position))
        {
            room.doorPositions.Add(position);
        }
    }

    // Function to place barriers at door positions
    void PlaceBarriers()
    {
        foreach (Room room in rooms)
        {
            foreach (Vector3Int doorPosition in room.doorPositions)
            {
                Vector3 worldPosition = floorTilemap.CellToWorld(doorPosition) + floorTilemap.cellSize / 2f;
                GameObject barrier = Instantiate(barrierPrefab, worldPosition, Quaternion.identity);
                room.barriers.Add(barrier);

                // Initially enable the barrier
                barrier.SetActive(true);
            }
        }
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
