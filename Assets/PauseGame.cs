using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public GameObject shop;

    bool pause_game = false;
    bool canUnpause = false;

    void Start()
    {
        shop.SetActive(false);
    }

    void Update()
    {
        bool input_condition = Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.JoystickButton4);

        if (!pause_game && input_condition)
        {
            Time.timeScale = 0;
            shop.SetActive(true);
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

            bool input_condition_1 = Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.JoystickButton4);

            if (input_condition_1)
            {
                Time.timeScale = 1;
                shop.SetActive(false);
                pause_game = false;
            }
        }
    }
}
