using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    [HideInInspector]
    public float damageAmount = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealthManager enemyHealth = collision.GetComponent<EnemyHealthManager>();

        if (enemyHealth != null)
        {
            //enemyHealth.deal_damage(damageAmount);
        }
    }
}
