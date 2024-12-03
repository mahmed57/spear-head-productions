using System.Collections;
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

    public float dashSpeed = 10f;
    public float dashDuration = 0.2f;
    private bool isDashing = false;

    public AudioClip Roll;

    private AudioSource audioSource;

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

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource is missing on Player!");
        }
    }

    void Update()
    {
        if(Time.timeScale == 0f)
        {
            return;
        }

        if(gameObject.GetComponent<PlayerAttack>().is_attacking)
        {
            return;
        }

        input();
    }

    void FixedUpdate()
    {
        move();
    }

    private void input()
    {
        if (isDashing) return; // Prevent input during dash

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
        if (isDashing)
        {
            animator.SetBool("isWalking", false);
            return;
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

        if (playerAttack.is_attacking)
        {
            animator.SetBool("isWalking", false);
            return;
        }
        else
        {
            Vector2 movement = direction.normalized * speed * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
        }
    }

    private void Dash()
    {
        if (!isDashing && !playerAttack.is_attacking)
        {
            animator.SetTrigger("roll");

            PlayerRollSound();

            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true;
        float elapsedTime = 0f;

        Vector2 dashDirection = Vector2.zero;

        if(direction.y > 0)
        {
            dashDirection += Vector2.up;
        }
        
        if(direction.y < 0)
        {
            dashDirection += Vector2.down;
        }

        if(direction.x > 0)
        {
            dashDirection += Vector2.right;
        }    
        
        if(direction.x < 0)
        { 
            dashDirection += Vector2.left;
        }    

        while (elapsedTime < dashDuration)
        {
            rb.MovePosition(rb.position + dashDirection.normalized * dashSpeed * Time.fixedDeltaTime);
            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        isDashing = false;
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 rotation = characterVisuals.eulerAngles;
        rotation.y += 180;
        characterVisuals.eulerAngles = rotation;
    }


    private void PlayerRollSound()
    {
        if(audioSource != null && Roll != null)
        {
            audioSource.PlayOneShot(Roll);
            Debug.Log("Playing roll sound");
        }
        else
        {
            Debug.LogWarning("Roll sound or AudioSource is missing!");
        }
    }

}
