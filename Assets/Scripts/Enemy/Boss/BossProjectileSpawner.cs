using UnityEngine;

public class BossProjectileSpawner : MonoBehaviour
{

    // Delay between successive projectiles
    public float timeBetweenProjectiles = 0.5f;

    // Projectile speed
    public float projectileSpeed = 3f;

    // Timer to track time between projectiles
    private float projectileTimer = 0f;

    void Update()
    {
            // Update the timer
            projectileTimer -= Time.deltaTime;

            // Check if it's time to fire the next projectile
            if (projectileTimer <= 0f)
            {
                // Fire a projectile
                FireProjectile();

                // Reset the timer
                projectileTimer = timeBetweenProjectiles;
            }

    }

    void FireProjectile()
    {
        // Calculate direction from spawn point to target (e.g., player)
        Vector2 direction = transform.position - GameObject.FindGameObjectWithTag("BossBody").transform.position;

        // Calculate rotation to face the direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Instantiate projectile at spawn point, with rotation to face the direction
    
        GameObject projectile = Instantiate(Resources.Load<GameObject>("Prefabs/" + "BossProjectile"), transform.position, Quaternion.Euler(0f, 0f, angle));

        // Set the projectile's velocity
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }
    }

}
