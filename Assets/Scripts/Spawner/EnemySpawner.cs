using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    public float spawnRadius = 50f;

    public List<GameObject> enemies = new List<GameObject>();

    public List<GameObject> SpawnEnemies(List<ProceduralLevelGenerator.EnemyType> enemyTypes, Room room, Tilemap floorTilemap, Vector3 roomCenter)
    {
        if (enemyTypes.Count == 0 || roomCenter == null)
        {
            return enemies;
        }   

        foreach (ProceduralLevelGenerator.EnemyType enemy in enemyTypes)
        {
            int enemyCount = Random.Range(enemy.minEnemiesPerRoom, enemy.maxEnemiesPerRoom + 1);

            List<Vector3Int> roomFloorPositions = new List<Vector3Int>();
            foreach (var tilePosition in room.floorTiles)
            {
                roomFloorPositions.Add(tilePosition);
            }

            if (roomFloorPositions.Count == 0)
            {
                Debug.LogWarning("No valid floor positions found in room for spawning enemies.");
                return enemies;
            }

            HashSet<Vector3Int> usedPositions = new HashSet<Vector3Int>();

            for (int i = 0; i < enemyCount; i++)
            {
                Vector3Int spawnTilePosition;

                while (true)
                {
                    spawnTilePosition = roomFloorPositions[Random.Range(0, roomFloorPositions.Count)];

                    Vector3 spawnWorldPosition = floorTilemap.CellToWorld(spawnTilePosition) + new Vector3(0.5f, 0.5f, 0);

                    if(!usedPositions.Contains(spawnTilePosition) && ((Vector3.Distance(spawnWorldPosition, roomCenter) - spawnRadius) < 5))
                    {
                        break;
                    }

                }

                usedPositions.Add(spawnTilePosition);

                Vector3 spawnPosition = floorTilemap.CellToWorld(spawnTilePosition) + new Vector3(0.5f, 0.5f, 0);
                enemies.Add(Instantiate(Resources.Load<GameObject>("Prefabs/" + enemy.prefab_name), spawnPosition, Quaternion.identity));
            }

        }

        return enemies;
        
    }
}
