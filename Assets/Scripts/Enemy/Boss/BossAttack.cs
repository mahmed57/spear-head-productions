using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{

    public float swordAttackRange = 2f;
    public float projectileAttackRange = 10f;
    public float wipeAttackRange = 3f;

    public GameObject wipe_ring;
    public float swordDamage = 10f;
    public float projectileDamage = 8f;


    public float attackCooldown = 2f;
    private float nextAttackTime = 0f;

    private Animator animator;

    private Transform player;
    private PlayerHealthManager playerHealth;

    public int currentPhase = 1;
    public float phaseHealth = 100f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealthManager>();

        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (Time.time >= nextAttackTime)
        {
            if (currentPhase == 1)
            {
                gameObject.GetComponent<BossMovement>().attackRange = swordAttackRange;
                if (distanceToPlayer <= swordAttackRange)
                {
                    animator.SetTrigger("attack1");
                    nextAttackTime = Time.time + attackCooldown;
                }
            }
            else if (currentPhase == 2)
            {
                gameObject.GetComponent<BossMovement>().speed = 3;
                
                if (distanceToPlayer <= swordAttackRange)
                {
                    animator.SetTrigger("attack1");
                    nextAttackTime = Time.time + attackCooldown;
                }
                else if (distanceToPlayer <= projectileAttackRange)
                {
                    ProjectileAttack();
                    nextAttackTime = Time.time + attackCooldown;
                }
            }
            else if (currentPhase == 3)
            {
                gameObject.GetComponent<BossMovement>().speed = 8;
                gameObject.GetComponent<BossMovement>().attackRange = swordAttackRange;
                if (distanceToPlayer <= wipeAttackRange)
                {
                    
                     
                    int randomAttack = Random.Range(0, 5);
                    if (randomAttack == 0)
                    {
                         animator.SetTrigger("attack3");
                    }

                    else
                    {
                    
                        if(distanceToPlayer <= swordAttackRange)
                        {
                            animator.SetTrigger("attack1");
                        }

                    }

                     nextAttackTime = Time.time + attackCooldown;
               

            }
        }
        }


    }

    void SwordAttack()
    {
        playerHealth.deal_damage(swordDamage);
   
    }

    void ProjectileAttack()
    {
    
    }

    void WipeAttack()
    {
        
        float damage = playerHealth.present_health / 2f;
        playerHealth.deal_damage(damage);
        Debug.Log("Boss performs Wipe Attack!");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, swordAttackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, wipeAttackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, projectileAttackRange);
    }

    public void enable_ring()
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/" + "Ring"), wipe_ring.transform.position, Quaternion.identity);
    }


}
