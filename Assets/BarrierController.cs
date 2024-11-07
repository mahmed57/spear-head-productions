using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BarrierController : MonoBehaviour
{
    public Transform player;          
    public float barrierRange = 3.0f; 
    private bool barrierDeactivated = false;

    private GameObject game_manager_obj;

    void Start()
    {
     
        if (player == null)
        {
            GameObject player_obj = GameObject.FindGameObjectWithTag("Player");
            player = player_obj.transform;
        }

        game_manager_obj = GameObject.FindGameObjectWithTag("GameManager");

    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        Debug.Log(game_manager_obj.GetComponentInChildren<PlayerStatistics>().crystal_count);

        if (!barrierDeactivated && (player.position.x < transform.position.x))
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
                        
                        Transform barrierChild1 = transform.GetChild(0);
                        Transform barrierChild2 = transform.GetChild(1);

                        barrierChild1.gameObject.SetActive(false);
                        barrierChild2.gameObject.SetActive(false);

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
                if(barrierDeactivated)
                {
                
                    Transform barrierChild1 = transform.GetChild(0);
                    Transform barrierChild2 = transform.GetChild(1);

                    barrierChild1.gameObject.SetActive(true);
                    barrierChild2.gameObject.SetActive(true);

                    //game_manager_obj.GetComponentInChildren<PlayerStatistics>().crystal_count -= 1;

                    barrierDeactivated = false;
                }
            }
        }
    }
}
