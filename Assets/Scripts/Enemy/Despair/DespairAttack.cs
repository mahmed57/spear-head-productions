using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespairAttack : MonoBehaviour
{
    public float attackRange = 1.5f;
    public float damage = 5f;
    public float attackCooldown = 2f;
    private float nextAttackTime = 0f;

    private Transform player;
    private MainPlayerHealthManager playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<MainPlayerHealthManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
        {

            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }


    void Attack()
    {
        Debug.Log("Enemy attacks!");
        playerHealth.damage(damage);
    }
}