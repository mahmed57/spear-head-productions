using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupLevelActivator : MonoBehaviour
{

    public List<GameObject> powerup_levels;

    public int power_up_number = 0;

    public void Update()
    {
        if(gameObject.activeSelf)
        {
            if(!powerup_levels[power_up_number].activeSelf)
            {
                powerup_levels[power_up_number].SetActive(true);
                disable_powerup_level(power_up_number);
            }
       }
    }

    void disable_powerup_level(int index)
    {
        for(int i = 0; i < powerup_levels.Count; i++)
        {
            if(i != index)
            {
                powerup_levels[i].SetActive(false);
            }
        }
    }

}
