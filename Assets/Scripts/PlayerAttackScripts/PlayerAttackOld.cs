using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerAttackOld : MonoBehaviour
{
    public Collider2D swordCollider; 
    public float heavyAttackDamage = 20f;
    public float lightAttackDamage = 10f;
    public float heavyAttackDuration = 0.5f;
    public float lightAttackDuration = 0.3f;
    public float heavyAttackCooldown = 1.0f;
    public float lightAttackCooldown = 0.5f;

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
       
        if (!isAttackOnCooldown && IsHeavyAttackInput())
        {
            StartCoroutine(PerformAttack(heavyAttackDamage, heavyAttackDuration, heavyAttackCooldown, isHeavyAttack: true));
        }
       
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
        Debug.Log(input_condition);
        return input_condition;
    }

    private IEnumerator PerformAttack(float damageAmount, float attackDuration, float attackCooldown, bool isHeavyAttack)
    {
        isAttacking = true;
        isAttackOnCooldown = true;

        
        if (isHeavyAttack)
        {
            animator.SetBool("isHeavyAttack", true);
        }
        else
        {
            animator.SetBool("isLightAttack", true);
        }

        
        swordCollider.enabled = true;

        
        SwordAttack swordAttack = swordCollider.GetComponent<SwordAttack>();
        if (swordAttack != null)
        {
            swordAttack.damageAmount = damageAmount;
        }

        
        yield return new WaitForSeconds(attackDuration);

        
        swordCollider.enabled = false;
        isAttacking = false;

        
        if (isHeavyAttack)
        {
            animator.SetBool("isHeavyAttack", false);
        }
        else
        {
            animator.SetBool("isLightAttack", false);
        }

        
        yield return new WaitForSeconds(attackCooldown);
        isAttackOnCooldown = false;
    }
}
