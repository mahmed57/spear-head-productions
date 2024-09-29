using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2f;
    public float detectionRange = 5f;
    private Transform playerTransform;
    private Rigidbody2D rb;
    private Vector2 movement;

    // Reference to the wall detection script
    private WallDetection wallDetection;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        rb = GetComponent<Rigidbody2D>();

        // Initialize the wall detection reference
        wallDetection = GetComponent<WallDetection>();
    }

    void Update()
    {
        if (playerTransform == null)
            return;

        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance <= detectionRange)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;

            // Prevent movement if touching walls
            if ((wallDetection.isTouchingWallRight && direction.x > 0) ||
                (wallDetection.isTouchingWallLeft && direction.x < 0))
            {
                direction.x = 0;
            }

            if ((wallDetection.isTouchingWallTop && direction.y > 0) ||
                (wallDetection.isTouchingWallBottom && direction.y < 0))
            {
                direction.y = 0;
            }

            movement = direction;
        }
        else
        {
            movement = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        MoveCharacter(movement);
    }

    void MoveCharacter(Vector2 direction)
    {
        rb.velocity = direction * speed;
    }
}
