using System.Collections;
using UnityEngine;

public class MiseryAttack : MonoBehaviour
{
    public float attackRange = 1.5f;
    public float damage = 5f;
    public float attackCooldown = 2f;
    private float nextAttackTime = 0f;

    public GameObject projectile;
    public float projectile_speed = 15f;

    private Animator animator;

    private Transform player;
    private PlayerHealthManager playerHealth;

    // New variable to control the delay before firing the projectile
    [Tooltip("Time delay between the start of the attack animation and projectile instantiation.")]
    public float projectileDelay = 1f; // Adjust this value to synchronize with your animation

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealthManager>();

        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            animator.SetTrigger("attack");

            // Start the coroutine to delay the projectile instantiation
            StartCoroutine(DelayedProjectile());
        }
    }

    private IEnumerator DelayedProjectile()
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(projectileDelay);

        // Instantiate the projectile
        ShootProjectile();
    }

    private void ShootProjectile()
    {
        // Calculate direction from enemy to player
        Vector2 enemy_pos = transform.position;
        Vector2 player_pos = player.position;
        Vector2 direction = (player_pos - enemy_pos).normalized;

        // Instantiate projectile at enemy position with no rotation
        GameObject new_projectile = Instantiate(projectile, transform.position, Quaternion.identity);
        new_projectile.GetComponent<ArrowProjectile>().damage = damage;

        // Set the projectile's velocity
        new_projectile.GetComponent<Rigidbody2D>().velocity = direction * projectile_speed;

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Adjust rotation of the projectile (modify the added angle if needed)
        new_projectile.transform.rotation = Quaternion.Euler(0, 0, angle + 90);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
