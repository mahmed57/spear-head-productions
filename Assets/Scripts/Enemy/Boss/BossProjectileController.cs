using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BossProjectileController : MonoBehaviour
{
    public List<GameObject> spawnPoints;

    public float attackCooldown = 5f;
    private float attackCooldownTimer = 0f;

    public float timeBetweenSpawnPointActivation = 0.5f;

    private bool isAttacking = false;

    void Update()
    {
        if((gameObject.GetComponent<BossAttack>().currentPhase == 1) || 
        gameObject.GetComponent<BossMovement>().is_in_range())
        {
            return;
        }
        attackCooldownTimer -= Time.deltaTime;

        if (attackCooldownTimer <= 0f && !isAttacking)
        {
            StartCoroutine(ActivateSpawnPoints());
        }
    }

    IEnumerator ActivateSpawnPoints()
    {
        isAttacking = true;

        foreach (GameObject spawnPoint in spawnPoints)
        {
            spawnPoint.SetActive(true);

            yield return new WaitForSeconds(timeBetweenSpawnPointActivation);
        }

        attackCooldownTimer = attackCooldown;


        yield return new WaitForSeconds(CalculateTotalFiringTime());


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
