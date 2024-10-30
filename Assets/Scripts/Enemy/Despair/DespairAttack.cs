using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespairAttack : MonoBehaviour
{
    public float attackRange = 1.5f;
    public float damage = 5f;
    public float attackCooldown = 2f;
    private float nextAttackTime = 0f;

    private Animator animator;

    private Transform player;
    private PlayerHealthManager playerHealth;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealthManager>();

        animator = GetComponentInChildren<Animator>();;
    }

    void Update()
    {

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
        {
            animator.SetTrigger("attack");
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }


    void Attack()
    {
        Debug.Log("Enemy attacks!");
        playerHealth.deal_damage(damage);
    }
}