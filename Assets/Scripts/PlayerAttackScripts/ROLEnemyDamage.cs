using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class ROLEnemyDamage : MonoBehaviour
{
    public bool rol_enabled = false;

    public float cooldown = 1f;

    public float radius = 1f;

    public float damage = 2f;

    public LayerMask whatIsEnemeies;

    private bool routine_started = false;
    
    void Update()
    {
         if(rol_enabled && !routine_started)
         {
            routine_started = true;

            StartCoroutine(RolAttack());
         }
    }

    IEnumerator RolAttack()
    {
        Vector2 Position = (Vector2)transform.Find("mainBody")?.gameObject.transform.position;
        
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(Position, radius, whatIsEnemeies);

        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            if((enemiesToDamage[i].tag == "Despair") || (enemiesToDamage[i].tag == "Misery") ||
            (enemiesToDamage[i].tag == "RageTanker"))
            {
                Debug.LogWarning("Executing....");
                enemiesToDamage[i].GetComponent<EnemyHealthManager>().deal_damage(damage);
            }

            if(enemiesToDamage[i].tag == "Boss")
            {
                Debug.LogWarning("Executing....");
                enemiesToDamage[i].GetComponent<BossHealthManager>().deal_damage(damage);
                
            }
        }

        yield return new WaitForSeconds(cooldown);

        routine_started = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere((Vector2)transform.Find("mainBody")?.gameObject.transform.position, radius);
    }


}
