using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePowerUpEffect : MonoBehaviour
{
    public GameObject powerup_effect;

    public void Start()
    {
        if(powerup_effect != null)
        {
            powerup_effect.SetActive(false);
        }
    
    }

    public void activate_powerup_effect()
    {
        if(powerup_effect != null){
        
        powerup_effect.SetActive(true);
        
        }
    }

    public void deactivate_powerup_effect()
    {
        if(powerup_effect != null){
        
        powerup_effect.SetActive(false);
        
        } 
    }
}
