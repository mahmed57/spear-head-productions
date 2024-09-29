using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

/// <summary>
/// Controls room behavior, such as spawning enemies when the player enters.
/// </summary>
public class RoomController : MonoBehaviour
{
    public Room room;
    public Tilemap floorTilemap;
    public List<ProceduralLevelGenerator.EnemyType> enemyTypes;

    private bool hasSpawnedEnemies = false;

    void Start()
    {
        // Optionally, disable the renderer so the room GameObject is invisible
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
            renderer.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasSpawnedEnemies && other.CompareTag("Player"))
        {
            SpawnEnemies();
            hasSpawnedEnemies = true;
        }
    }

    /// <summary>
    /// Spawns enemies in the room when the player enters.
    /// </summary>
    void SpawnEnemies()
    {
        // For now, we will only spawn RageTankers
        var rageTankerType = enemyTypes.Find(e => e.name == "RageTanker");
        if (rageTankerType == null)
        {
            Debug.LogError("RageTanker enemy type not found in enemyTypes list.");
            return;
        }

        int enemyCount = Random.Range(rageTankerType.minEnemiesPerRoom, rageTankerType.maxEnemiesPerRoom + 1);

        // Collect all floor positions within this room (excluding walls)
        List<Vector3Int> roomFloorPositions = new List<Vector3Int>();
        foreach (var tilePosition in room.floorTiles)
        {
            roomFloorPositions.Add(tilePosition);
        }

        // If no valid positions are found, skip this room
        if (roomFloorPositions.Count == 0)
        {
            Debug.LogWarning("No valid floor positions found in room for spawning enemies.");
            return;
        }

        HashSet<Vector3Int> usedPositions = new HashSet<Vector3Int>();

        for (int i = 0; i < enemyCount; i++)
        {
            Vector3Int spawnTilePosition;
            int attempts = 0;

            do
            {
                spawnTilePosition = roomFloorPositions[Random.Range(0, roomFloorPositions.Count)];
                attempts++;
            }
            while (usedPositions.Contains(spawnTilePosition) && attempts < 10);

            // If failed to find a unique position, skip spawning this enemy
            if (attempts >= 10)
                continue;

            usedPositions.Add(spawnTilePosition);

            // Convert tile position to world position
            Vector3 spawnPosition = floorTilemap.CellToWorld(spawnTilePosition) + new Vector3(0.5f, 0.5f, 0);

            // Instantiate the enemy
            Instantiate(rageTankerType.enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
