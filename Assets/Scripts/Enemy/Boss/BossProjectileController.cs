using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BossProjectileController : MonoBehaviour
{
    // List of spawn point GameObjects
    public List<GameObject> spawnPoints;

    // Cooldown period between attacks
    public float attackCooldown = 5f;
    private float attackCooldownTimer = 0f;

    // Time between activating each spawn point
    public float timeBetweenSpawnPointActivation = 0.5f;

    // Flag to check if the boss is currently attacking
    private bool isAttacking = false;

    void Update()
    {
        if((gameObject.GetComponent<BossAttack>().currentPhase == 1) || gameObject.GetComponent<BossMovement>().is_in_range())
        {
            return;
        }
        attackCooldownTimer -= Time.deltaTime;

        // If cooldown has elapsed and the boss is not already attacking
        if (attackCooldownTimer <= 0f && !isAttacking)
        {
            // Start the attack sequence
            StartCoroutine(ActivateSpawnPoints());
        }
    }

    IEnumerator ActivateSpawnPoints()
    {
        isAttacking = true;

        // Activate each spawn point with a delay
        foreach (GameObject spawnPoint in spawnPoints)
        {
            spawnPoint.SetActive(true);

            // Wait before activating the next spawn point
            yield return new WaitForSeconds(timeBetweenSpawnPointActivation);
        }

        // Reset the cooldown timer
        attackCooldownTimer = attackCooldown;

        // Wait for all spawn points to finish firing
        yield return new WaitForSeconds(CalculateTotalFiringTime());

        // Deactivate all spawn points
        foreach (GameObject spawnPoint in spawnPoints)
        {
            spawnPoint.SetActive(false);
        }

        isAttacking = false;
    }

    float CalculateTotalFiringTime()
    {

        float maxFiringTime = 0f;

        foreach (GameObject spawnPoint in spawnPoints)
        {
            BossProjectileSpawner spawner = spawnPoint.GetComponent<BossProjectileSpawner>();
            if (spawner != null)
            {
                float firingTime = spawner.timeBetweenProjectiles;
                if (firingTime > maxFiringTime)
                {
                    maxFiringTime = firingTime;
                }
            }
        }

        return maxFiringTime;
    }
}
