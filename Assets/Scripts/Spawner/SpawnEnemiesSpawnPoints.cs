using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class SpawnEnemiesSpawnPoints : MonoBehaviour
{
    // Start is called before the first frame update    // List of spawn points set in the Unity Editor
    public List<Transform> spawnPoints;

    // Names of enemy prefabs in the Resources folder
    private string[] enemyNames = { "Despair", "Misery", "RageTanker" };

    public float cooldown = 10f;
    
    void Start()
    {

        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Wait for 10 seconds before spawning the next enemy
            yield return new WaitForSeconds(cooldown);

            // Select a random spawn point
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            Transform spawnPoint = spawnPoints[spawnIndex];

            // Select a random enemy
            int enemyIndex = Random.Range(0, enemyNames.Length);
            string enemyName = enemyNames[enemyIndex];

            // Load the enemy prefab from the Resources folder
            GameObject enemyPrefab = Resources.Load<GameObject>("Prefabs/" + enemyName);

            if (enemyPrefab != null)
            {
                // Instantiate the enemy at the spawn point
                Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Enemy prefab not found: " + enemyName);
            }

            
        }
    }
}
