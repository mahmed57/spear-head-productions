using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{

    public void SpawnEnemies(List<ProceduralLevelGenerator.EnemyType> enemyTypes, Room room, Tilemap floorTilemap)
    {
        var rageTankerType = enemyTypes.Find(e => e.name == "RageTanker");
        if (rageTankerType == null)
        {
            Debug.LogError("RageTanker enemy type not found in enemyTypes list.");
            return;
        }

        int enemyCount = Random.Range(rageTankerType.minEnemiesPerRoom, rageTankerType.maxEnemiesPerRoom + 1);

        List<Vector3Int> roomFloorPositions = new List<Vector3Int>();
        foreach (var tilePosition in room.floorTiles)
        {
            roomFloorPositions.Add(tilePosition);
        }

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

            if (attempts >= 10)
                continue;

            usedPositions.Add(spawnTilePosition);

            Vector3 spawnPosition = floorTilemap.CellToWorld(spawnTilePosition) + new Vector3(0.5f, 0.5f, 0);

            Instantiate(rageTankerType.enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
}

