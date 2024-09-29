using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class ProceduralLevelGenerator : MonoBehaviour
{
    [Header("Tilemap and Tiles")]
    public Tilemap tilemap;
    public TileBase floorTile;
    public TileBase wallTile;

    [Header("Dungeon Generation Parameters")]
    public int roomCount = 5;
    public int minRoomSize = 4;
    public int maxRoomSize = 8;
    public int mapWidth = 50;
    public int mapHeight = 50;

    private List<RectInt> rooms;

    void Start()
    {
        rooms = new List<RectInt>();
        GenerateRooms();
        GenerateCorridors();
        DrawFloors();
        DrawWalls();
    }

    void GenerateRooms()
    {
        for (int i = 0; i < roomCount; i++)
        {
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
            else
            {
                i--; // Try again
            }
        }
    }

    void GenerateCorridors()
    {
        for (int i = 0; i < rooms.Count - 1; i++)
        {
            Vector2Int currentCenter = GetRoomCenter(rooms[i]);
            Vector2Int nextCenter = GetRoomCenter(rooms[i + 1]);

            if (Random.value < 0.5f)
            {
                CreateHorizontalCorridor(currentCenter.x, nextCenter.x, currentCenter.y);
                CreateVerticalCorridor(currentCenter.y, nextCenter.y, nextCenter.x);
            }
            else
            {
                CreateVerticalCorridor(currentCenter.y, nextCenter.y, currentCenter.x);
                CreateHorizontalCorridor(currentCenter.x, nextCenter.x, nextCenter.y);
            }
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
                    tilemap.SetTile(new Vector3Int(x, y, 0), floorTile);
                }
            }
        }
    }

    void DrawWalls()
    {
        BoundsInt bounds = tilemap.cellBounds;

        for (int x = bounds.xMin - 1; x <= bounds.xMax + 1; x++)
        {
            for (int y = bounds.yMin - 1; y <= bounds.yMax + 1; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                if (tilemap.GetTile(pos) != floorTile && HasAdjacentFloor(pos))
                {
                    tilemap.SetTile(pos, wallTile);
                    RotateWall(pos);
                }
            }
        }
    }

    void RotateWall(Vector3Int pos)
    {
        Matrix4x4 matrix = Matrix4x4.identity;
        TileBase tile = tilemap.GetTile(pos);

        bool hasFloorLeft = tilemap.GetTile(pos + Vector3Int.left) == floorTile;
        bool hasFloorRight = tilemap.GetTile(pos + Vector3Int.right) == floorTile;
        bool hasFloorUp = tilemap.GetTile(pos + Vector3Int.up) == floorTile;
        bool hasFloorDown = tilemap.GetTile(pos + Vector3Int.down) == floorTile;

        if (hasFloorLeft && !hasFloorRight)
        {
            matrix.SetTRS(Vector3.zero, Quaternion.Euler(0, 0, 90), Vector3.one);
        }
        else if (!hasFloorLeft && hasFloorRight)
        {
            matrix.SetTRS(Vector3.zero, Quaternion.Euler(0, 0, -90), Vector3.one);
        }
        else if (hasFloorUp && !hasFloorDown)
        {
            matrix.SetTRS(Vector3.zero, Quaternion.Euler(0, 0, 0), Vector3.one);
        }
        else if (!hasFloorUp && hasFloorDown)
        {
            matrix.SetTRS(Vector3.zero, Quaternion.Euler(0, 0, 180), Vector3.one);
        }

        tilemap.SetTransformMatrix(pos, matrix);
    }

    bool HasAdjacentFloor(Vector3Int pos)
    {
        return tilemap.GetTile(pos + Vector3Int.up) == floorTile ||
               tilemap.GetTile(pos + Vector3Int.down) == floorTile ||
               tilemap.GetTile(pos + Vector3Int.left) == floorTile ||
               tilemap.GetTile(pos + Vector3Int.right) == floorTile;
    }

    Vector2Int GetRoomCenter(RectInt room)
    {
        int x = Mathf.RoundToInt(room.xMin + room.width / 2f);
        int y = Mathf.RoundToInt(room.yMin + room.height / 2f);
        return new Vector2Int(x, y);
    }

    void CreateHorizontalCorridor(int xStart, int xEnd, int y)
    {
        int xMin = Mathf.Min(xStart, xEnd);
        int xMax = Mathf.Max(xStart, xEnd);

        for (int x = xMin; x <= xMax; x++)
        {
            tilemap.SetTile(new Vector3Int(x, y, 0), floorTile);
        }
    }

    void CreateVerticalCorridor(int yStart, int yEnd, int x)
    {
        int yMin = Mathf.Min(yStart, yEnd);
        int yMax = Mathf.Max(yStart, yEnd);

        for (int y = yMin; y <= yMax; y++)
        {
            tilemap.SetTile(new Vector3Int(x, y, 0), floorTile);
        }
    }
}
