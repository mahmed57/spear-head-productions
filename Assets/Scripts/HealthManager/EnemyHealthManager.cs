using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthManager : CharacterHealthManager
{
    public float pushForce = 5f;
    private EnemyMovement enemyMovement;
    public Slider health_bar_slider;
    public GameObject health_bar;
    public GameObject damage_particle_system;

    public AudioSource persistentAudioSource; 
    public AudioClip hurtSoundClip; 

    private float next_particle_time;
    public float particle_effect_cooldown = 1f;

    void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();

        if (persistentAudioSource != null)
        {
            if (!persistentAudioSource.gameObject.activeSelf)
            {
                Debug.LogWarning("Persistent AudioSource GameObject is inactive. Activating it.");
                persistentAudioSource.gameObject.SetActive(true);
            }

            if (!persistentAudioSource.enabled)
            {
                Debug.LogWarning("Persistent AudioSource is disabled. Enabling it.");
                persistentAudioSource.enabled = true;
            }
        }
    }

    void Update()
    {
        if (damage_particle_system != null)
        {
            if (damage_particle_system.activeSelf)
            {
                if (Time.time > next_particle_time)
                {
                    damage_particle_system.SetActive(false);
                }
            }
        }
    }

    public override void deal_damage(float damage, Vector2 attackPosition)
    {
        health_bar.SetActive(true);

        base.deal_damage(damage, attackPosition);

        if (persistentAudioSource != null && hurtSoundClip != null)
        {
            if (persistentAudioSource.isActiveAndEnabled)
            {
                persistentAudioSource.PlayOneShot(hurtSoundClip);
            }
            else
            {
                //Debug.LogWarning("Persistent AudioSource is not active or enabled. Using fallback method.");
                PlaySoundWithFallback(transform.position, hurtSoundClip);
            }
        }
        else
        {
            Debug.LogWarning("Persistent AudioSource or Hurt Sound Clip is not assigned!");
        }

        if (damage_particle_system != null)
        {
            next_particle_time = Time.time + particle_effect_cooldown;
            damage_particle_system.SetActive(true);
        }

        Vector2 pushDirection = (transform.position - new Vector3(attackPosition.x, attackPosition.y, transform.position.z)).normalized;
        enemyMovement.ApplyPushBack(pushDirection, pushForce);

        health_bar_slider.value = present_health / max_health;
    }

    protected override void handle_death()
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerStatistics>().increment_enemy_counter(gameObject);

        CoinCounter.instance.AddCoin(1);

        base.handle_death();
    }

    private void PlaySoundWithFallback(Vector3 position, AudioClip clip)
    {
        GameObject tempAudio = new GameObject("TempAudio");
        AudioSource tempAudioSource = tempAudio.AddComponent<AudioSource>();
        tempAudioSource.clip = clip;

        tempAudioSource.outputAudioMixerGroup = persistentAudioSource.outputAudioMixerGroup;
        tempAudioSource.Play();
        Destroy(tempAudio, clip.length);
    }
}
