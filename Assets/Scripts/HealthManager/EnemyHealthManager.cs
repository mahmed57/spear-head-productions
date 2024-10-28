using UnityEngine;

public class EnemyHealthManager:CharacterHealthManager
{
     public float pushForce = 5f;

     private EnemyMovement enemyMovement;
     private RageTankerMovement rage_tanker_movement;

    void Start()
    {

         enemyMovement = GetComponent<EnemyMovement>();
        
    }

    public override void deal_damage(float damage, Vector2 attackPosition)
    {
        base.deal_damage(damage, attackPosition);

        Vector2 pushDirection = (transform.position - new Vector3(attackPosition.x, attackPosition.y, transform.position.z)).normalized;


        enemyMovement.ApplyPushBack(pushDirection, pushForce);
    


        
        
    }

}