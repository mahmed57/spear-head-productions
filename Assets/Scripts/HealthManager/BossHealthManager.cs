using UnityEngine;
using UnityEngine.UI;

public class BossHealthManager : CharacterHealthManager
{
    public float lightpushForce = 5f;
    public float heavypushforce = 10f;
    private BossMovement bossMovement;

    public Slider health_bar_slider;

    public GameObject health_bar;
    public GameObject damage_particle_system;

    public GameObject damageNumberPrefab;

    private float next_particle_time;
    public float particle_effect_cooldown = 1f;

    public GameObject victoryScreen;

    private BossAudio bossAudio;

    void Start()
    {
        bossMovement = GetComponent<BossMovement>();

        bossAudio = GetComponent<BossAudio>();
        bossAudio?.PlayLaughSound(); 
    }

    void Update()
    {
        if (damage_particle_system != null && damage_particle_system.activeSelf)
        {
            if (Time.time > next_particle_time)
            {
                damage_particle_system.SetActive(false);
            }
        }
    }

    public override void deal_damage(float damage, Vector2 attackPosition)
    {
        health_bar.SetActive(true);

        base.deal_damage(damage, attackPosition);

        bossAudio?.PlayHurtSound();

        if (damage_particle_system != null)
        {
            next_particle_time = Time.time + particle_effect_cooldown;
            damage_particle_system.SetActive(true);
        }

        if (damageNumberPrefab != null)
        {
            Vector3 damagePosition = transform.position + Vector3.up * 1.5f;

            GameObject damageNumber = Instantiate(damageNumberPrefab, damagePosition, Quaternion.identity, transform.parent);
            DamageNumber damageScript = damageNumber.GetComponent<DamageNumber>();

            if (damageScript != null)
            {
                damageScript.SetDamage(damage);
            }
            else
            {
                Debug.LogWarning("DamageNumber script is not attached to the prefab!");
            }
        }

        Vector2 pushDirection = (transform.position - new Vector3(attackPosition.x, attackPosition.y, transform.position.z)).normalized;

        if (damage < 9f)
        {
            bossMovement.ApplyPushBack(pushDirection, lightpushForce);
        }
        else
        {
            bossMovement.ApplyPushBack(pushDirection, heavypushforce);
        }

        health_bar_slider.value = present_health / max_health;
    }

    protected override void handle_death()
    {
        Debug.Log("Boss is dead. Triggering Victory Screen...");

        if (GetComponent<BossAttack>().currentPhase == 3)
        {
            if (victoryScreen != null)
            {
                Debug.Log("Activating Victory Screen...");
                victoryScreen.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                Debug.LogError("Victory Screen is not assigned!");
            }
            Destroy(gameObject);
        }
        else
        {
            int nextPhase = GetComponent<BossAttack>().currentPhase + 1;
            GetComponent<BossAttack>().currentPhase = nextPhase;

            max_health = 100;
            present_health = 100;

            Debug.Log($"Transitioning to phase {nextPhase}...");

            bossAudio?.PlayLaughSound();
        }
    }
}
