using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Audio Clips")]
    public AudioClip attackSound;
    public AudioClip walkSound;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float attackVolume = 0.4f;
    [Range(0f, 1f)] public float walkVolume = 0.85f;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void PlayAttackSound()
    {
        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound, attackVolume);
        }
    }

    public void PlayWalkSound()
    {
        if (audioSource != null && walkSound != null)
        {
            audioSource.PlayOneShot(walkSound, walkVolume);
        }
    }
}