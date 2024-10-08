using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f;
    public float detectionRange = 5f;
    public float separationDistance = 1.5f; // Minimum distance between enemies to prevent overlap
    public float attackRange = 1.5f;

    [Header("Layer Masks")]
    public LayerMask enemyLayer; // Layer for enemies
    public LayerMask wallLayer;  // Layer for walls

    [Header("Wall Detection Settings")]
    public float wallDetectionDistance = 0.5f;

    [Header("Detection Radii")]
    public float detectionRadius = 0.5f; // Radius to detect other enemies

    private Transform playerTransform;
    private Rigidbody2D rb;
    private Vector2 movement;

    // Wall detection booleans
    private bool isTouchingWallLeft;
    private bool isTouchingWallRight;
    private bool isTouchingWallBottom;
    private bool isTouchingWallTop;

    // Pushback variables
    private bool isPushedBack = false;  // To track if enemy is being pushed back
    public float pushBackDuration = 0.2f;  
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
        if (isPushedBack)
            return;  

        DetectWalls();  

        if (playerTransform == null)
            return;  

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
        {
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

            
            if ((isTouchingWallRight && directionToPlayer.x > 0) ||
                (isTouchingWallLeft && directionToPlayer.x < 0))
            {
                directionToPlayer.x = 0;
            }

            if ((isTouchingWallTop && directionToPlayer.y > 0) ||
                (isTouchingWallBottom && directionToPlayer.y < 0))
            {
                directionToPlayer.y = 0;
            }

            
            Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);

            if (nearbyEnemies.Length > 0)
            {
               
                foreach (Collider2D enemyCollider in nearbyEnemies)
                {
                    if (enemyCollider.gameObject != gameObject)  
                    {
                        Vector2 directionToEnemy = (enemyCollider.transform.position - transform.position).normalized;
                        float distanceToEnemy = Vector2.Distance(transform.position, enemyCollider.transform.position);

                        if (distanceToEnemy < separationDistance)
                        {
                           
                            directionToPlayer -= directionToEnemy * (separationDistance - distanceToEnemy);
                        }
                    }
                }
            }

            movement = directionToPlayer.normalized;
        }
        else
        {
            movement = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        if (isPushedBack)
        {
            pushBackTimer -= Time.fixedDeltaTime;
            if (pushBackTimer <= 0)
            {
                isPushedBack = false; 
            }
        }
        else
        {
            MoveCharacter(movement);
        }
    }


    public void ApplyPushBack(Vector2 pushDirection, float pushForce)
    {
        isPushedBack = true;
        pushBackTimer = pushBackDuration;
        rb.velocity = pushDirection * pushForce;  
    }

    void MoveCharacter(Vector2 direction)
    {
        rb.velocity = direction * speed;
    }

    void DetectWalls()
    {
        isTouchingWallLeft = Physics2D.Raycast(transform.position, Vector2.left, wallDetectionDistance, wallLayer);
        isTouchingWallRight = Physics2D.Raycast(transform.position, Vector2.right, wallDetectionDistance, wallLayer);
        isTouchingWallTop = Physics2D.Raycast(transform.position, Vector2.up, wallDetectionDistance, wallLayer);
        isTouchingWallBottom = Physics2D.Raycast(transform.position, Vector2.down, wallDetectionDistance, wallLayer);
    }

  
    void OnDrawGizmosSelected()
    {
 
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

    
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * wallDetectionDistance);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * wallDetectionDistance);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * wallDetectionDistance);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * wallDetectionDistance);
    }
}
