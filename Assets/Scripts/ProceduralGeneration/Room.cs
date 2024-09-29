using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Represents a room in the dungeon.
/// </summary>
public class Room
{
    public RectInt bounds;
    public List<Vector3Int> floorTiles;
    public bool hasSpawnedEnemies = false;

    // Enemy spawning data (if needed)
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
