using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ButtonPressHandler : MonoBehaviour
{

    public List<GameObject> game_objects;

    public GameObject active_object;

    public GameObject game_manager_shop;

    public bool special_powerup = false;

    void Update()
    {

        foreach(GameObject game_object in game_objects)
        {   
            if(game_object.activeSelf)
            {
                active_object = game_object;
                break;
            }

            game_object.transform.GetChild(1).gameObject.SetActive(false);
            game_object.transform.GetChild(0).gameObject.SetActive(true);
        }


    }

    public void buy()
    {
        if((active_object != null) && !game_manager_shop.GetComponent<PlayerBag>().collectedPowerups.Contains(active_object.name))
        {
            game_manager_shop.GetComponent<PlayerBag>().collectedPowerups.Add(active_object.name);
            //game_manager_shop.GetComponent<ItemPowerupCoinMapper>().PowerupDictionary[active_object.name][0];
            
            active_object.SetActive(false);

            return;

        }


    }

    public void reroll()
    {
        if((active_object != null) && game_manager_shop.GetComponent<PlayerBag>().collectedPowerups.Contains(active_object.name))
        {
            game_manager_shop.GetComponent<PlayerBag>().collectedPowerups.Remove(active_object.name);

            active_object.SetActive(false);

            //game_manager_shop.GetComponent<ItemPowerupCoinMapper>().PowerupDictionary[active_object.name][1];
        }
        
    }

    public void i()
    {
        if(active_object.transform.GetChild(1).gameObject.activeSelf)
        {
            active_object.transform.GetChild(0).gameObject.SetActive(true);
            active_object.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            active_object.transform.GetChild(0).gameObject.SetActive(false);
            active_object.transform.GetChild(1).gameObject.SetActive(true);

        }
    }


}
