using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PlayerHealthManager : CharacterHealthManager
{

    private void heal(float heal_amount)
    {
        if(is_over_heal(heal_amount))
        {
            present_health = max_health;
        }
        else
        {
            present_health += heal_amount;
        }

    }

    private bool is_over_heal(float heal_amount){
            return (present_health + heal_amount) > max_health;
    }    
}
