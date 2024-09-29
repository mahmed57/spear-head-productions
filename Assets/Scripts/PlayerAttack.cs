using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public Collider2D swordCollider; // Assign the sword's Collider2D in the Inspector
    public float heavyAttackDamage = 20f;
    public float lightAttackDamage = 10f;
    public float attackDuration = 0.3f; // Duration the attack collider is active
    public float attackCooldown = 0.5f; // Time between attacks

    private bool isAttacking = false;
    private bool isAttackOnCooldown = false;
    private Animator animator;
    Gamepad gamepad = Gamepad.current;

    void Start()
    {
        // Get the Animator component attached to the player
        animator = GetComponent<Animator>();

        // Ensure the sword's collider is disabled at the start
        if (swordCollider != null)
            swordCollider.enabled = false;
    }

    void Update()
    {
        // Check for heavy attack input
        if (!isAttackOnCooldown && (IsHeavyAttackInput()))
        {
            StartCoroutine(PerformAttack(heavyAttackDamage, isHeavyAttack: true));
        }
        // Check for light attack input
        else if (!isAttackOnCooldown && (IsLightAttackInput()))
        {
            StartCoroutine(PerformAttack(lightAttackDamage, isHeavyAttack: false));
        }
    }

    private bool IsHeavyAttackInput()
    {
        // Adjust the joystick button based on your controller mapping
        if (gamepad!= null){
        return gamepad.buttonEast.wasPressedThisFrame || // Square button
               Input.GetMouseButtonDown(1) ||               // Mouse Right Click
               Input.GetKeyDown(KeyCode.P);
        }
        return  Input.GetMouseButtonDown(1) ||               // Mouse Right Click
               Input.GetKeyDown(KeyCode.P);
        
                         // 'P' key
    }

    private bool IsLightAttackInput()
    {
        if (gamepad!= null){
        return Input.GetMouseButtonDown(0) ||               // Mouse Left Click
               Input.GetKeyDown(KeyCode.O) || 
               gamepad.buttonWest.wasPressedThisFrame;                 // 'O' key
        }
        else
        {
            return Input.GetMouseButtonDown(0) ||               // Mouse Left Click
               Input.GetKeyDown(KeyCode.O);
        }
    }

    private IEnumerator PerformAttack(float damageAmount, bool isHeavyAttack)
    {
        isAttacking = true;
        isAttackOnCooldown = true;

        // Set the appropriate Animator boolean
        if (isHeavyAttack)
        {
            animator.SetBool("isHeavyAttack", true);
        }
        else
        {
            animator.SetBool("isLightAttack", true);
        }

        // Enable the sword's collider
        swordCollider.enabled = true;

        // Set the damage amount in the SwordAttack script
        SwordAttack swordAttack = swordCollider.GetComponent<SwordAttack>();
        if (swordAttack != null)
        {
            swordAttack.damageAmount = damageAmount;
        }

        // Wait for the attack duration
        yield return new WaitForSeconds(attackDuration);

        // Disable the sword's collider
        swordCollider.enabled = false;
        isAttacking = false;

        // Reset Animator booleans
        if (isHeavyAttack)
        {
            animator.SetBool("isHeavyAttack", false);
        }
        else
        {
            animator.SetBool("isLightAttack", false);
        }

        // Wait for attack cooldown
        yield return new WaitForSeconds(attackCooldown);
        isAttackOnCooldown = false;
    }
}
