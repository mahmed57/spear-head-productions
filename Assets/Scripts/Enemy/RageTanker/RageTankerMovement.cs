using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class RageTankerMovement : MonoBehaviour
{
   
    public float detectionRange = 5f;
    public float attackRange = 1f;   
    public float moveSpeed = 2f;  
    public bool isInAttackRange = false;

    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isFacingRight = true;
    private bool isWalking = false;
    public Transform characterVisuals;
    public float separationDistance = 0.5f;
    public LayerMask enemyLayer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (characterVisuals == null)
        {
            characterVisuals = animator.transform;
        }


    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        
        if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
        {
            FollowPlayer();
        }
        else if (distanceToPlayer <= attackRange)
        {
            StopAndAttack();
        }
        else
        {
            isWalking = false;
            animator.SetBool("isWalking", isWalking);
        }

        if (!isInAttackRange && ((isFacingRight && player.position.x < transform.position.x) || (!isFacingRight && player.position.x > transform.position.x)))
        {
            Flip();
        }
    }

    void FollowPlayer()
    {
        isInAttackRange = false;

        isWalking = true;

        animator.SetBool("isWalking", isWalking);

        Vector2 targetPosition = (Vector2) (player.position - transform.position);

        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, separationDistance, enemyLayer);

        if (nearbyEnemies.Length > 0)
        {
            foreach (Collider2D enemyCollider in nearbyEnemies)
            {
                if (enemyCollider.gameObject != gameObject)
                {
    
                            Vector2 directionToEnemy = (transform.position - enemyCollider.transform.position).normalized;
                            
                            float distanceToEnemy = Vector2.Distance(transform.position, enemyCollider.transform.position);

                            targetPosition += directionToEnemy * (separationDistance - distanceToEnemy);
                            
                }
            }
       }
        
        rb.MovePosition((Vector2)rb.position + targetPosition.normalized* moveSpeed * Time.deltaTime);
    }

    void StopAndAttack()
    {
        isWalking = false;
        animator.SetBool("isWalking", isWalking);
        isInAttackRange = true;
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 rotation = characterVisuals.eulerAngles;
        rotation.y += 180;
        characterVisuals.eulerAngles = rotation;
    }

    public void PushBack(float force, Vector2 direction)
    {
        isWalking = false;
        animator.SetBool("isWalking", isWalking);
        rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
    }


}
