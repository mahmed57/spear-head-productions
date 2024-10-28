using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;


public class RoomController : MonoBehaviour
{
    public Room room;
    public Tilemap floorTilemap;
    public List<ProceduralLevelGenerator.EnemyType> enemyTypes;

    private bool hasSpawnedEnemies = false;

    public GameObject spawner;

    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        
        if (renderer != null)
            renderer.enabled = false;

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasSpawnedEnemies && other.CompareTag("Player"))
        {   

            spawner.GetComponent<AssetSpawner>().spawn_assets();
            spawner.GetComponent<EnemySpawner>().SpawnEnemies(enemyTypes, room, floorTilemap);
            hasSpawnedEnemies = true;
        }
    }
}
