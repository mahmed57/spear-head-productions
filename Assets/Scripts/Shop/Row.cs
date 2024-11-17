using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{

    public List<GameObject> powerups;

    public string active_object_name;

    public void enable_random_powerup()
    {

        int randomIndex = Random.Range(0, powerups.Count);

        powerups[randomIndex].SetActive(true);

        active_object_name = powerups[randomIndex].name;

        disable_all_other_powerups(randomIndex);

    }

    public void disable_all_other_powerups(int index)
    {   
        for(int i = 0; i < powerups.Count; i++)
        {
             if(i != index)
             {
                powerups[i].SetActive(false);
             }
        } 
    }
}
