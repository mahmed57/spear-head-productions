using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class BossMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f;
    public float detectionRange = 5f;
    public float separationDistance = 1.5f;
    public float attackRange = 1.3f;
    public float flipThresholdDistance = 0.5f; // New threshold distance

    [Header("Flip Settings")]
    public float flipCooldown = 1f; // Cooldown time between flips
    private float lastFlipTime = 0f; // Tracks the last flip time

    [Header("Layer Masks")]
    public LayerMask enemyLayer;
    public LayerMask wallLayer;

    [Header("Wall Detection Settings")]
    public float wallDetectionDistance = 0.5f;

    [Header("Detection Radii")]
    public float detectionRadius = 0.5f;

    private Transform playerTransform;
    private Vector2 movement;

    private bool isTouchingWallLeft;
    private bool isTouchingWallRight;
    private bool isTouchingWallBottom;
    private bool isTouchingWallTop;

    private Animator animator;
    private bool facingRight = false;

    public Transform characterVisuals;

    private bool isPushedBack = false;
    public float pushBackDuration = 0.2f;
    private float pushBackTimer = 0f;
    private Vector2 pushBackDirection;
    private float pushBackSpeed;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTransform = player.transform;

        animator = GetComponentInChildren<Animator>();

        if (characterVisuals == null)
        {
            Transform visuals = transform.Find("Visuals");
            if (visuals != null)
                characterVisuals = visuals;
            else
                characterVisuals = transform;
        }

    }

    void Update()
    {

        if (isPushedBack)
        {
            pushBackTimer -= Time.deltaTime;
            if (pushBackTimer <= 0)
                isPushedBack = false;
        }
        else
        {
            if (playerTransform == null)
                return;

            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
            {
                if (animator != null)
                    animator.SetBool("isWalking", true);

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
                            Vector2 directionToEnemy = (transform.position - enemyCollider.transform.position).normalized;
                            float distanceToEnemy = Vector2.Distance(transform.position, enemyCollider.transform.position);

                            if (distanceToEnemy < separationDistance)
                            {
                                directionToPlayer += directionToEnemy * (separationDistance - distanceToEnemy);
                            }
                        }
                    }
                }

                movement = directionToPlayer.normalized;

                // Only flip if the distance to the player is greater than the threshold distance
                if (distanceToPlayer > flipThresholdDistance && Time.time - lastFlipTime >= flipCooldown)
                {
                    if (movement.x > 0 && !facingRight)
                    {
                        
                        Flip();
                    }
                    else if (movement.x < 0 && facingRight)
                    {
                        Flip();
                    }
                }
            }
            else
            {
                movement = Vector2.zero;

                if (animator != null)
                    animator.SetBool("isWalking", false);

                if (distanceToPlayer < attackRange)
                {
                    
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (isPushedBack)
        {
            if (animator != null)
                animator.SetBool("isWalking", false);

            MoveCharacter(pushBackDirection * pushBackSpeed);
        }
        else
        {
            MoveCharacter(movement * speed);
        }
    }

    void MoveCharacter(Vector2 velocity)
    {
        transform.Translate(velocity * Time.fixedDeltaTime);
    }

    public void ApplyPushBack(Vector2 pushDirection, float pushForce)
    {
        isPushedBack = true;
        pushBackTimer = pushBackDuration;
        pushBackDirection = pushDirection.normalized;
        pushBackSpeed = pushForce;

        if (animator != null)
            animator.SetBool("isWalking", false);
    }

    private void Flip()
    {
        if(Vector2.Distance(transform.position, playerTransform.position) <= attackRange)
        {
            return;
        }
        facingRight = !facingRight;
        lastFlipTime = Time.time; // Update the last flip time


        Vector3 scale = characterVisuals.localScale;
        scale.x *= -1;
        characterVisuals.localScale = scale;
        
    }

    public bool is_in_range()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        return (distanceToPlayer <= attackRange);
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
