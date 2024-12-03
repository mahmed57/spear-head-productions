using UnityEngine;

public class BossProjectileSpawner : MonoBehaviour
{

    public float timeBetweenProjectiles = 0.5f;

    public float projectileSpeed = 3f;

    private float projectileTimer = 0f;

    void Update()
    {
            projectileTimer -= Time.deltaTime;

            if (projectileTimer <= 0f)
            {
                FireProjectile();

                projectileTimer = timeBetweenProjectiles;
            }

    }

    void FireProjectile()
    {
        Vector2 direction = transform.position - GameObject.FindGameObjectWithTag("BossBody").transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

    
        GameObject projectile = Instantiate(Resources.Load<GameObject>("Prefabs/" + "BossProjectile"), transform.position, Quaternion.Euler(0f, 0f, angle));

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }
    }

}
