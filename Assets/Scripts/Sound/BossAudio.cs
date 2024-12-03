using UnityEngine;

public class BossAudio : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Wave Attack Sounds")]
    public AudioClip waveAttackSound;
    public AudioClip waveChargeSound;

    [Header("Laugh Sounds")]
    public AudioClip laughSoundPhase1;
    public AudioClip laughSoundPhase2;
    public AudioClip laughSoundPhase3;

    [Header("Sword Attack Sound")]
    public AudioClip swordAttackSound;

    [Header("Hurt Sound")]
    public AudioClip hurtSound;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float waveAttackVolume = 1f;
    [Range(0f, 1f)] public float waveChargeVolume = 1f;
    [Range(0f, 1f)] public float laughVolume = 1f;
    [Range(0f, 1f)] public float swordAttackVolume = 1f;
    [Range(0f, 1f)] public float hurtVolume = 0.1f;

    private AudioSource waveChargeAudioSource; 

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        waveChargeAudioSource = gameObject.AddComponent<AudioSource>();
        waveChargeAudioSource.loop = true; 
    }

    public void PlayWaveAttackSound()
    {
        if (audioSource != null && waveAttackSound != null)
        {
            audioSource.PlayOneShot(waveAttackSound, waveAttackVolume);
        }
    }

    public void PlayWaveChargeSound()
    {
        if (waveChargeAudioSource != null && waveChargeSound != null)
        {
            waveChargeAudioSource.clip = waveChargeSound;
            waveChargeAudioSource.volume = waveChargeVolume;
            waveChargeAudioSource.Play();
        }
    }

    public void StopWaveChargeSound()
    {
        if (waveChargeAudioSource != null && waveChargeAudioSource.isPlaying)
        {
            waveChargeAudioSource.Stop();
        }
    }

    public void PlayLaughSound()
    {
        if (audioSource != null && laughSoundPhase1 != null)
        {
            audioSource.PlayOneShot(laughSoundPhase1, laughVolume); 
        }
    }

    public void PlaySwordAttackSound()
    {
        if (audioSource != null && swordAttackSound != null)
        {
            audioSource.PlayOneShot(swordAttackSound, swordAttackVolume);
        }
    }

    public void PlayHurtSound()
    {
        if (audioSource != null && hurtSound != null)
        {
            audioSource.PlayOneShot(hurtSound, hurtVolume);
        }
    }
}
