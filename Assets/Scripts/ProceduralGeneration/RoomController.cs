using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;


public class RoomController : MonoBehaviour
{
    public Room room;
    public Tilemap floorTilemap;
    public float enemyspawnRadius = 10f; 

    public BoxCollider2D roomCollider;
    public List<ProceduralLevelGenerator.EnemyType> enemyTypes;

    public Vector3 roomCenter;

    private bool hasSpawnedEnemies = false;

    public GameObject spawner;

    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        roomCollider = GetComponent<BoxCollider2D>();

        roomCenter = roomCollider.bounds.center;
        

        if (renderer != null)
            renderer.enabled = false;

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasSpawnedEnemies && other.CompareTag("Player"))
        {   

            spawner.GetComponent<AssetSpawner>().spawn_assets();

            spawner.GetComponent<EnemySpawner>().SpawnEnemies(enemyTypes, room, floorTilemap, roomCenter);
            
            hasSpawnedEnemies = true;
        }
    }
}
