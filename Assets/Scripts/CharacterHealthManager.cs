using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class CharacterHealthManager : MonoBehaviour
{
   public float present_health;
   public float max_health;

   protected void deal_damage(float damage)
   {
        present_health -= damage;

        if (present_health <=0){
            Destroy(gameObject);
        }
        
   }

}
