using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovements : MonoBehaviour
{
    public float speed;
    private Vector2 direction;
    public float dash_range;
    private Vector2 controller_input;
    public enum Facing { UP, DOWN, LEFT, RIGHT };
    public Facing FacingDir = Facing.DOWN;
    private bool facingRight = true;

    private Animator animator;
    public Transform characterVisuals;
   
    private PlayerAttack playerAttack;

    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        animator.SetBool("isWalking", false);

        if (characterVisuals == null)
        {
            characterVisuals = animator.transform;
        }
         
        playerAttack = GetComponent<PlayerAttack>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        input();
        
    }

    void FixedUpdate()
    {
        move();
    }

    private void input()
    {
        
        direction = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector2.up;
            FacingDir = Facing.UP;
        }

        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector2.left;
            FacingDir = Facing.LEFT;
        }

        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector2.down;
            FacingDir = Facing.DOWN;
        }

        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector2.right;
            FacingDir = Facing.RIGHT;
        }

        Gamepad gamepad = Gamepad.current;

        if (gamepad != null)
        {
            controller_input = gamepad.leftStick.ReadValue();
            direction += controller_input;

            if (controller_input != Vector2.zero)
            {
                if (Mathf.Abs(controller_input.x) > Mathf.Abs(controller_input.y))
                {
                    FacingDir = (controller_input.x > 0) ? Facing.RIGHT : Facing.LEFT;
                }
                else
                {
                    FacingDir = (controller_input.y > 0) ? Facing.UP : Facing.DOWN;
                }
            }

            
 
            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                Dash();
            }
            
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Dash();
        }
    }

    private void move()
    {
        
        if (direction != Vector2.zero)
        {
            animator.SetBool("isWalking", true);

            if (direction.x > 0 && !facingRight)
            {
                Flip();
            }
            else if (direction.x < 0 && facingRight)
            {
                Flip();
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        Vector2 movement = new Vector2(direction.x, direction.y);
        
        if (playerAttack.is_attacking)
        {
        
            animator.SetBool("isWalking", false);
            return;
        
        }
        else
        {
            Vector2 position = (Vector2)rb.position + movement.normalized * speed * Time.deltaTime;
            rb.MovePosition(position);
        }
    }

    private void Dash()
    {
        animator.SetTrigger("roll");
        if (playerAttack.is_attacking) return;

        Vector2 target_pos = Vector2.zero;

        if (FacingDir == Facing.UP)
        {
            target_pos += Vector2.up;
        }
        else if (FacingDir == Facing.DOWN)
        {
            target_pos += Vector2.down;
        }
        else if (FacingDir == Facing.RIGHT)
        {
            target_pos += Vector2.right;
        }
        else if (FacingDir == Facing.LEFT)
        {
            target_pos += Vector2.left;
        }

        Vector3 dashMovement = new Vector3(target_pos.x, target_pos.y, 0);
        transform.Translate(dashMovement * dash_range, Space.World);
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 rotation = characterVisuals.eulerAngles;
        rotation.y += 180;
        characterVisuals.eulerAngles = rotation;
    }
}
