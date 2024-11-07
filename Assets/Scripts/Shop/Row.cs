using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{

    public List<GameObject> powerups;

    public void enable_random_powerup()
    {

        int randomIndex = Random.Range(0, powerups.Count);

        powerups[randomIndex].SetActive(true);

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
