using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private float time_between_attack;
    public float start_time_btw_attack;

    private GameObject sword;

    public Transform attackpos;

    public bool amaretsu_enabled = false;
    public float attackRange;

    public LayerMask whatIsEnemeies;
    public float light_attack_damage;

    public float heavy_attack_damage;

    public float default_hv_attack_dam;
    public float default_li_attack_dam;

    public Animator player_anim;

    public bool is_attacking = false;

    public AudioClip Light_attack;
    public AudioClip Heavy_attack;

    private AudioSource audioSource;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        default_hv_attack_dam = heavy_attack_damage;
        default_li_attack_dam = light_attack_damage;
        sword = transform.Find("Sword")?.gameObject;
    }
    void Update()
    {
        if(Time.timeScale == 0f)
        {
            return;
        }

        if(is_attacking)
        {
            return;
        }

        if(light_attack_damage < default_li_attack_dam)
        {
            light_attack_damage = default_li_attack_dam;
        }

        if(heavy_attack_damage < default_hv_attack_dam)
        {
            heavy_attack_damage = default_hv_attack_dam;
        }

        if (is_light_attack())
        {
            player_anim.SetTrigger("lightAttack");
            PlayerAttackSound(Light_attack);
        }
        if (is_heavy_attack())
        {
            player_anim.SetTrigger("heavyAttack");
            PlayerAttackSound(Heavy_attack);
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
        Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(attackpos.position,new Vector2(attackRange, attackRange-1f), whatIsEnemeies);

        time_between_attack = start_time_btw_attack;
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {

            if((enemiesToDamage[i].tag == "Despair") || (enemiesToDamage[i].tag == "Misery") ||
            (enemiesToDamage[i].tag == "RageTanker"))
            {
                enemiesToDamage[i].GetComponent<EnemyHealthManager>().deal_damage(damage, transform.position);
                if(amaretsu_enabled)
                {
                    enemiesToDamage[i].GetComponent<AmaretsuEnemyBurn>().burn();
                }
            }

            if(enemiesToDamage[i].tag == "Boss")
            {
                enemiesToDamage[i].GetComponent<BossHealthManager>().deal_damage(damage, transform.position);
                
                if(amaretsu_enabled)
                {
                    enemiesToDamage[i].GetComponent<AmaretsuEnemyBurn>().burn();
                }
            }
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 boxSize = new Vector2(attackRange, attackRange - 1f);
        Matrix4x4 defaultMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(attackpos.position, Quaternion.Euler(0, 0, 0), Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
        Gizmos.matrix = defaultMatrix;    
    
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

    public void HeavyAttack()
    {
        attack_enemy(heavy_attack_damage);
    }

    public void LightAttack()
    {
        attack_enemy(light_attack_damage);
    }
    
    public void set_attack_true()
    {
        is_attacking = true;
    }

    public void set_attack_false()
    {
        is_attacking = false;
    }

    
}
