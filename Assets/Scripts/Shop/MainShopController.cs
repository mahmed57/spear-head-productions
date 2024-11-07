using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

public class MainShopController : MonoBehaviour
{
    
    public GameObject essence_ui_game_object;
    public GameObject items_ui_game_object;

    public bool turned_on = false;

    public List<GameObject> rows;

    public GameObject shop;

    int count = 0;

    public void set_essence_ui_active()
    {
        items_ui_game_object.SetActive(false);
        essence_ui_game_object.SetActive(true);
        
    }

    public void set_items_ui_active()
    {
        essence_ui_game_object.SetActive(false);
        items_ui_game_object.SetActive(true);
    }

    void Update()
    {
         if(shop.activeSelf && !turned_on)
         {
             essence_ui_game_object.SetActive(true);
         }

         if(!shop.activeSelf)
         {
            essence_ui_game_object.SetActive(false);
            items_ui_game_object.SetActive(false);
         }

        if(essence_ui_game_object.activeSelf || items_ui_game_object.activeSelf)
        {
            if(!turned_on)
            {
                if(essence_ui_game_object.activeSelf)
                {
                    foreach(GameObject row in rows)
                    {
                        
                        row.GetComponent<Row>().enable_random_powerup();
                    }
                }
                turned_on = true;
            }
        }

        if(!essence_ui_game_object.activeSelf && !items_ui_game_object.activeSelf)
        {
            count += 1;
            turned_on = false;

        }
    }



}
