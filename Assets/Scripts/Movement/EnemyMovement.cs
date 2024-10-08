using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2f;
    public float detectionRange = 5f;
    public float separationDistance = 1.5f; // Minimum distance between enemies to prevent overlap
    public LayerMask enemyLayer; // Layer for enemies
    public float detectionRadius = 0.5f; // Radius to detect other enemies

    private Transform playerTransform;
    private Rigidbody2D rb;
    private Vector2 movement;

    private bool isPushedBack = false;  // To track if enemy is being pushed back
    public float pushBackDuration = 0.2f;  // Duration of the pushback effect
    private float pushBackTimer = 0f;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (playerTransform == null || isPushedBack)
            return;  // Do nothing if the player is missing or enemy is being pushed back

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= detectionRange)
        {
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

            // Detect other enemies in range
            Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);

            if (nearbyEnemies.Length > 0)
            {
                // Adjust movement if other enemies are too close
                foreach (Collider2D enemyCollider in nearbyEnemies)
                {
                    if (enemyCollider.gameObject != gameObject)  // Ignore self
                    {
                        Vector2 directionToEnemy = (enemyCollider.transform.position - transform.position).normalized;
                        float distanceToEnemy = Vector2.Distance(transform.position, enemyCollider.transform.position);

                        if (distanceToEnemy < separationDistance)
                        {
                            // Apply separation force to avoid overlap
                            directionToPlayer -= directionToEnemy * (separationDistance - distanceToEnemy);
                        }
                    }
                }
            }

            movement = directionToPlayer.normalized;
        }
        else
        {
            movement = Vector2.zero;  // Stop moving if out of detection range
        }
    }

    void FixedUpdate()
    {
        if (isPushedBack)
        {
            pushBackTimer -= Time.fixedDeltaTime;
            if (pushBackTimer <= 0)
            {
                isPushedBack = false;  // End pushback
            }
        }
        else
        {
            MoveCharacter(movement);
        }
    }

    // Method to apply pushback when receiving damage
    public void ApplyPushBack(Vector2 pushDirection, float pushForce)
    {
        isPushedBack = true;
        pushBackTimer = pushBackDuration;
        rb.velocity = pushDirection * pushForce;  // Apply the pushback force
    }

    void MoveCharacter(Vector2 direction)
    {
        rb.velocity = direction * speed;
    }

    // Optional visualization for the enemy detection radius
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
