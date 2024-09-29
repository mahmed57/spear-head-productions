using UnityEngine;

/// <summary>
/// Controls the AI behavior of the RageTanker enemy.
/// </summary>
public class RageTankerAI : MonoBehaviour
{
    public float moveSpeed = 2f;         // Movement speed
    public float detectionRange = 5f;    // Range to detect the player
    public int health = 100;             // Health points

    private Transform playerTransform;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        // Find the player by tag (make sure your player has the "Player" tag)
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTransform = player.transform;

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (playerTransform == null)
            return;

        // Calculate distance to the player
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance <= detectionRange)
        {
            // Move towards the player
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            movement = direction;
        }
        else
        {
            // Idle or patrol behavior can be added here
            movement = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        MoveCharacter(movement);
    }

    void MoveCharacter(Vector2 direction)
    {
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    // Example of taking damage
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            // Handle enemy death
            Destroy(gameObject);
        }
    }
}
