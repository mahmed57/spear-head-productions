using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Audio Clips")]
    public AudioClip attackSound;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float attackVolume = 0.4f;

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
}