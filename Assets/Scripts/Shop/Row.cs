using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{

    public List<GameObject> powerups;

    void onEnable()
    {

        int randomIndex = Random.Range(0, powerups.Count);

        powerups[randomIndex].SetActive(true);

    }
    

    void onDisable()
    {   
        foreach(GameObject powerup in powerups)
        {
             powerup.SetActive(false);
        } 
    }
}
