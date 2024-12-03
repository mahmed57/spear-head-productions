using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerController : MonoBehaviour
{
    public float cooldown = 1f;

    public float radius = 1f;

    public float damage = 2f;

    public LayerMask whatIsEnemeies;

    private bool routine_started = false;

    public float speed = 2f;
    
    void Update()
    {
         if(gameObject.activeSelf && !routine_started)
         {
            routine_started = true;

            StartCoroutine(RolAttack());
         }
    }

    IEnumerator RolAttack()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemeies);

        List<Collider2D> filteredEnemies = new List<Collider2D>();

        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            if (enemiesToDamage[i].tag != "Player")
            {
                filteredEnemies.Add(enemiesToDamage[i]);
            }
        }

        if (filteredEnemies.Count > 0)
        {
            int random_index = Random.Range(0, filteredEnemies.Count);

            Vector2 direction = (filteredEnemies[random_index].GetComponent<Rigidbody2D>().transform.position - transform.position).normalized;

            GameObject projectile = Instantiate(Resources.Load<GameObject>("Prefabs/Seeker_Projectile"), transform.position, Quaternion.identity);

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            rb.velocity = direction * speed;
        }

        yield return new WaitForSeconds(cooldown);

        routine_started = false;
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
