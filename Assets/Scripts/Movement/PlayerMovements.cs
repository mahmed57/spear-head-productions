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

    // Reference to the wall detection script
    private WallDetection wallDetection;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        animator.SetBool("isHeavyAttack", false);
        animator.SetBool("isLightAttack", false);
        animator.SetBool("isWalking", false);

        if (characterVisuals == null)
        {
            characterVisuals = animator.transform;
        }

        // Initialize the wall detection reference
        wallDetection = GetComponent<WallDetection>();
    }

    void Update()
    {
        input();
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
                    if (controller_input.x > 0)
                    {
                        FacingDir = Facing.RIGHT;
                    }
                    else
                    {
                        FacingDir = Facing.LEFT;
                    }
                }
                else
                {
                    if (controller_input.y > 0)
                    {
                        FacingDir = Facing.UP;
                    }
                    else
                    {
                        FacingDir = Facing.DOWN;
                    }
                }
            }

            // Disable dash if any wall detection is true
            if (!IsTouchingWall())
            {
                if (gamepad.buttonSouth.wasPressedThisFrame)
                {
                    Dash();
                }
            }
        }

        if (!IsTouchingWall())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Dash();
            }
        }
    }

    // Method to check if the player is touching any walls
    private bool IsTouchingWall()
    {
        return wallDetection.isTouchingWallRight || wallDetection.isTouchingWallLeft || 
               wallDetection.isTouchingWallTop || wallDetection.isTouchingWallBottom;
    }

    private void Dash()
    {
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

    private void move()
    {
        // Prevent movement if touching walls
        if ((wallDetection.isTouchingWallRight && direction.x > 0) || 
            (wallDetection.isTouchingWallLeft && direction.x < 0))
        {
            direction.x = 0;
        }

        if ((wallDetection.isTouchingWallTop && direction.y > 0) || 
            (wallDetection.isTouchingWallBottom && direction.y < 0))
        {
            direction.y = 0;
        }

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

        Vector3 movement = new Vector3(direction.x, direction.y, 0);
        transform.Translate(movement.normalized * speed * Time.deltaTime, Space.World);
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 rotation = characterVisuals.eulerAngles;
        rotation.y += 180;
        characterVisuals.eulerAngles = rotation;
    }
}
