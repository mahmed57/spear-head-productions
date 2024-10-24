using System.Collections;
using System.Collections.Generic;

using System.Xml.Serialization;
using UnityEngine;

public class CharacterHealthManager : MonoBehaviour
{
   public float present_health;
   public float max_health;

   public virtual void deal_damage(float damage)
   {
        present_health -= damage;

        if (present_health <=0){
           handle_death();
        }
        
   }

   public virtual void deal_damage(float damage, Vector2 attackPosition)
   {
         deal_damage(damage);
   }
   
   protected virtual void handle_death()
   {
         Destroy(gameObject);
   }

}
