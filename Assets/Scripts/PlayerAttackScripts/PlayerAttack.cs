using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private float time_between_attack;
    public float start_time_btw_attack;

    public Transform attackpos;
    public float attackRange;

    public LayerMask whatIsEnemeies;
    public float light_attack_damage;

    public float heavy_attack_damage;

    public Animator player_anim;

    public bool is_attacking = false;

    public AudioClip Light_attack;
    public AudioClip Heavy_attack;

    private AudioSource audioSource;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }
    void Update()
    {
        if (time_between_attack <= 0)
        {
            if (is_light_attack())
            {
                is_attacking = true;
                player_anim.SetTrigger("lightAttack");
                PlayerAttackSound(Light_attack);
                attack_enemy(light_attack_damage);

            }
            else if (is_heavy_attack())
            {
                is_attacking = true;
                player_anim.SetTrigger("heavyAttack");
                PlayerAttackSound(Heavy_attack);
                attack_enemy(heavy_attack_damage);
            }
            else
            {
                is_attacking = false;
            }

        }
        else
        {
            time_between_attack -= Time.deltaTime;
        }
    }

    bool is_light_attack()
    {
        bool input_condition = Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.J);
        Gamepad gamepad = Gamepad.current;

        if (gamepad != null)
        {
            input_condition = input_condition || gamepad.buttonWest.wasPressedThisFrame;
        }

        return input_condition;
    }

    bool is_heavy_attack()
    {

        bool input_condition = Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.K);
        Gamepad gamepad = Gamepad.current;

        if (gamepad != null)
        {
            input_condition = input_condition || gamepad.buttonNorth.wasPressedThisFrame;
        }

        return input_condition;
    }

    void attack_enemy(float damage)
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackpos.position, attackRange, whatIsEnemeies);

        time_between_attack = start_time_btw_attack;
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {

            enemiesToDamage[i].GetComponent<EnemyHealthManager>().deal_damage(damage, transform.position);

        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackpos.position, attackRange);
    }


    private void PlayerAttackSound(AudioClip clip)
    {
        if(audioSource == null)
        {
            Debug.LogWarning("AudioSource is missing!");
            return;
        }

        if(clip == null)
        {
            Debug.LogWarning("Attempted to play a null AudioClip");
            return;
        }

        Debug.Log($"Playing sound: {clip.name}");
        audioSource.PlayOneShot(clip);
    }
}
