using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthManager:CharacterHealthManager
{
     public float pushForce = 5f;

     private EnemyMovement enemyMovement;
     public Slider health_bar_slider;

    public GameObject health_bar;

    void Start()
    {

         enemyMovement = GetComponent<EnemyMovement>();
        
    }

    public override void deal_damage(float damage, Vector2 attackPosition)
    {
        health_bar.SetActive(true);

        base.deal_damage(damage, attackPosition);

        Vector2 pushDirection = (transform.position - new Vector3(attackPosition.x, attackPosition.y, transform.position.z)).normalized;

        enemyMovement.ApplyPushBack(pushDirection, pushForce);

        health_bar_slider.value = present_health / max_health;
       
    }

}