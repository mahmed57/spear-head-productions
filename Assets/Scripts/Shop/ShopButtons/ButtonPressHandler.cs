using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressHandler : MonoBehaviour
{
    public List<GameObject> game_objects;
    public GameObject active_object;
    public GameObject game_manager_shop;
    private GameObject coin_counter;
    public AudioSource spendSound;
    public bool special_powerup = false;

    public GameObject power_up_object;

    void Start()
    {
        coin_counter = GameObject.FindGameObjectWithTag("CoinCounter");
    }

    void Update()
    {
        foreach (GameObject game_object in game_objects)
        {
            if (game_object.activeSelf)
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
        if ((active_object != null) && !game_manager_shop.GetComponent<PlayerBag>().collectedPowerups.Contains(active_object.name))
        {
            game_manager_shop.GetComponent<PlayerBag>().collectedPowerups.Add(active_object.name);
            int price = game_manager_shop.GetComponent<ItemPowerupCoinMapper>().PowerupDictionary[active_object.name][0];

            if (coin_counter.GetComponent<CoinCounter>().RemoveCoin(price))
            {
                power_up_object.GetComponent<PowerupManager>().apply_powerup(active_object.name);
                

                if (spendSound != null)
                {
                    spendSound.Play();
                }

                active_object.transform.GetChild(5).gameObject.SetActive(false);
                active_object.transform.GetChild(6).gameObject.SetActive(true);
                

            }
        }
    }

    public void reroll()
    {
        if ((active_object != null) && game_manager_shop.GetComponent<PlayerBag>().collectedPowerups.Contains(active_object.name))
        {
            game_manager_shop.GetComponent<PlayerBag>().collectedPowerups.Remove(active_object.name);
            int price = game_manager_shop.GetComponent<ItemPowerupCoinMapper>().PowerupDictionary[active_object.name][1];

            if (coin_counter.GetComponent<CoinCounter>().RemoveCoin(price))
            {
                power_up_object.GetComponent<PowerupManager>().remove_powerup(active_object.name);

                if (spendSound != null)
                {
                    spendSound.Play();
                }

                active_object.transform.GetChild(5).gameObject.SetActive(true);
                active_object.transform.GetChild(6).gameObject.SetActive(false);
            }
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
