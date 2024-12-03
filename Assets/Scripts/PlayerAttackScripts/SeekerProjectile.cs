using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SeekerProjectile : MonoBehaviour
{
    public float damage = 3f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Misery") || collision.gameObject.CompareTag("Despair") ||
            collision.gameObject.CompareTag("RageTanker"))
        {
                Debug.Log("Executed Rage Tnker...");
                collision.gameObject.GetComponent<EnemyHealthManager>().deal_damage(damage);
                Destroy(gameObject);
        }

        else if(collision.gameObject.CompareTag("Boss"))
        {
            collision.gameObject.GetComponent<BossHealthManager>().deal_damage(damage);
            Destroy(gameObject);
        }

        else if(collision.gameObject.CompareTag("Player"))
        {

        }

        else
        {
            Destroy(gameObject);
        }
    }
}
