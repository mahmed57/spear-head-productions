using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupLevelActivator : MonoBehaviour
{

    public List<GameObject> powerup_levels;

    public int power_up_number = 0;

    void onEnable()
    {
        Debug.Log("Executed power up row....");
        powerup_levels[power_up_number].SetActive(true);
    }

    void onDisable()
    {
        powerup_levels[power_up_number].SetActive(false);
    }

}
