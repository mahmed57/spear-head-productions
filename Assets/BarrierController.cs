using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BarrierController : MonoBehaviour
{
    public Transform player;          
    public float barrierRange = 2.0f; 
    private bool barrierDeactivated = false;

    private GameObject game_manager_obj;

    void Start()
    {
     
        if (player == null)
        {
            GameObject player_obj = GameObject.FindGameObjectWithTag("Player");
            player = player_obj.transform;
        }

        if(player == null)
        {
            Debug.Log("Problem with assigning player transform");
        }

        game_manager_obj = GameObject.FindGameObjectWithTag("GameManager");
        
        if(game_manager_obj == null)
        {
            Debug.Log("Problem with reading game_manager_obj.GetComponentInChildren<PlayerStatistics>()");
        }


    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (!barrierDeactivated)
        {
            
            if (distance <= barrierRange)
            {
                
                bool input_condition = Input.GetKeyDown(KeyCode.F);

                Gamepad gamepad = Gamepad.current;
        
                if (gamepad != null)
                {
                    input_condition = Input.GetKeyDown(KeyCode.F) || gamepad.buttonEast.wasPressedThisFrame;
                }

                if (input_condition)
                {
                    
                    if (game_manager_obj.GetComponentInChildren<PlayerStatistics>().crystal_count > 0)
                    {
                        
                        Transform barrierChild = transform.GetChild(0);
                        barrierChild.gameObject.SetActive(false);
                        barrierDeactivated = true;

                        Debug.Log ("barrier deactivated");
                    }
                }
            }
        }
        else
        {
            
            float xDistance = player.position.x - transform.position.x;
            if (xDistance > 3.2f)
            {
                game_manager_obj.GetComponentInChildren<PlayerStatistics>().crystal_count -= 1;
                
                Transform barrierChild = transform.GetChild(0);
                
                barrierChild.gameObject.SetActive(true);

                barrierDeactivated = false;
            }
        }
    }
}
