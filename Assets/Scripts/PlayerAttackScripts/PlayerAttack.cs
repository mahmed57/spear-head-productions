using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public Collider2D swordCollider; // Assign the sword's Collider2D in the Inspector
    public float heavyAttackDamage = 20f;
    public float lightAttackDamage = 10f;
    public float heavyAttackDuration = 0.5f; // Duration for heavy attack collider
    public float lightAttackDuration = 0.3f; // Duration for light attack collider
    public float heavyAttackCooldown = 1.0f; // Cooldown time for heavy attacks
    public float lightAttackCooldown = 0.5f; // Cooldown time for light attacks

    public bool isAttacking = false;
    private bool isAttackOnCooldown = false;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (swordCollider != null)
            swordCollider.enabled = false;
    }

    void Update()
    {
        // Check for heavy attack input
        if (!isAttackOnCooldown && IsHeavyAttackInput())
        {
            StartCoroutine(PerformAttack(heavyAttackDamage, heavyAttackDuration, heavyAttackCooldown, isHeavyAttack: true));
        }
        // Check for light attack input
        else if (!isAttackOnCooldown && IsLightAttackInput())
        {
            StartCoroutine(PerformAttack(lightAttackDamage, lightAttackDuration, lightAttackCooldown, isHeavyAttack: false));
        }
    }

    private bool IsHeavyAttackInput()
    {
        bool input_condition = Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.K);
        Gamepad gamepad = Gamepad.current;

        if (gamepad != null)
        {
            input_condition = input_condition || gamepad.buttonNorth.wasPressedThisFrame;
        }

        return input_condition;
    }

    private bool IsLightAttackInput()
    {
        bool input_condition = Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.J);
        Gamepad gamepad = Gamepad.current;

        if (gamepad != null)
        {
            input_condition = input_condition || gamepad.buttonWest.wasPressedThisFrame;
        }

        return input_condition;
    }

    private IEnumerator PerformAttack(float damageAmount, float attackDuration, float attackCooldown, bool isHeavyAttack)
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

        // Wait for the attack duration (to complete the attack animation)
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

        // Wait for the cooldown of the attack type
        yield return new WaitForSeconds(attackCooldown);
        isAttackOnCooldown = false;
    }
}
