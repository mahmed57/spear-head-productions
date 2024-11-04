using UnityEngine;
using System.Collections.Generic;

public class Room
{
    public RectInt bounds;
    public Vector3 room_center_position;
    public Vector3 room_world_size;
    public List<Vector3Int> floorTiles;
    public bool hasSpawnedEnemies = false;
    public List<Vector3Int> doorPositions = new List<Vector3Int>();
    public List<GameObject> barriers = new List<GameObject>();
    public ProceduralLevelGenerator.EnemyType enemyType;
    public int enemyCount;
    public List<Vector3Int> enemySpawnPositions;

    public Room(RectInt bounds)
    {
        this.bounds = bounds;
        floorTiles = new List<Vector3Int>();
        enemySpawnPositions = new List<Vector3Int>();
    }
}
