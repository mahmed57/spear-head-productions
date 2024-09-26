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
    
    public float dash_range;
    private Vector2 controller_input;

    public enum Facing {UP, DOWN, LEFT, RIGHT};

    public Facing FacingDir = Facing.DOWN;
    
    void Update()
    {
         input();
         move();
         
    }

    private void input(){
        direction = Vector2.zero;
        
        if(Input.GetKey(KeyCode.W)){
            direction += Vector2.up;
            FacingDir = Facing.UP;
        }
        
        if(Input.GetKey(KeyCode.A)){
            direction += Vector2.left;
            FacingDir = Facing.LEFT;
        }
        
        if(Input.GetKey(KeyCode.S)){
            direction += Vector2.down;
            FacingDir = Facing.DOWN;
        }

        if(Input.GetKey(KeyCode.D)){
            direction += Vector2.right;
            FacingDir = Facing.RIGHT;
        }

        Gamepad gamepad = Gamepad.current;
        
        if (gamepad != null)
        {
            controller_input = gamepad.leftStick.ReadValue(); 
            direction += controller_input;
        }
        
        if (Input.GetKeyDown(KeyCode.Space) )
        {
                
                Vector2 target_pos = Vector2.zero;

                if (FacingDir == Facing.UP){
                    target_pos += Vector2.up;
                }
                else if(FacingDir == Facing.DOWN)
                {
                    target_pos += Vector2.down;
                }
                else if(FacingDir == Facing.RIGHT)
                {
                    target_pos += Vector2.right;
                }
                else if (FacingDir == Facing.LEFT){
                    target_pos += Vector2.left;
                }
                transform.Translate(target_pos * dash_range);
        }
    }

    private void move(){
        transform.Translate(direction * player_speed * Time.deltaTime);
    }

}
