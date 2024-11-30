using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public GameObject shop;

    bool pause_game = false;
    bool canUnpause = false;

    public float start_time = 5;

    public GameObject pause_menu;

    void Start()
    {
        shop.SetActive(false);
        pause_menu.SetActive(false);
        Time.timeScale = 1.2f;
    }


    void Update()
    {
        bool input_condition_shop = Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.JoystickButton4);
        bool input_condition_pause_menu = Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton5);
        if(!pause_menu.activeSelf && !shop.activeSelf)
        {
            pause_game = false;
            canUnpause=false;
        }
        if (!pause_game && (input_condition_shop|| input_condition_pause_menu))
        {
            Time.timeScale = 0;

            if (input_condition_pause_menu){
                pause_menu.SetActive(true);
                shop.SetActive(false);
            }

            else
            {        
                shop.SetActive(true);
                pause_menu.SetActive(false);
            }
            
            pause_game = true;
            canUnpause = false;
        }

        if (pause_game)
        {
            if (!canUnpause)
            {
                canUnpause = true;
                return;
            }

            bool input_condition_shop_1 = Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.JoystickButton4);
            bool input_condition_pause_menu_1 = Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton5);
            
            if ((input_condition_shop_1 && shop.activeSelf) || (input_condition_pause_menu_1 && pause_menu.activeSelf))
            {
                
                shop.SetActive(false);
                pause_menu.SetActive(false);
                float future_time = Time.time + start_time;
                
                pause_game = false;

                Time.timeScale = 1.2f;
            }

        }
    }
}
