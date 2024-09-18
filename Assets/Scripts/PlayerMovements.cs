using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class PlayerMovements : MonoBehaviour
{
    public float player_speed;

    private Vector2 direction;
    
    private Vector2 controller_input;

    void Update()
    {
         input();
         move();
         
    }

    private void input(){
        direction = Vector2.zero;
        
        if(Input.GetKey(KeyCode.W)){
            direction += Vector2.up;
        }
        
        if(Input.GetKey(KeyCode.A)){
            direction += Vector2.left;
        }
        
        if(Input.GetKey(KeyCode.S)){
            direction += Vector2.down;
        }

        if(Input.GetKey(KeyCode.D)){
            direction += Vector2.right;
        }

        Gamepad gamepad = Gamepad.current;
        if (gamepad != null)
        {
            controller_input = gamepad.leftStick.ReadValue(); 
            direction += controller_input;
        }

    }

    private void move(){
        transform.Translate(direction * player_speed * Time.deltaTime);
    }

}
