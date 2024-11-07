using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{   
    public GameObject essence_ui_game_object;
    public GameObject items_ui_game_object;

    void Start()
    {
      
    }


    void Update()
    {
        
    }

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

}
