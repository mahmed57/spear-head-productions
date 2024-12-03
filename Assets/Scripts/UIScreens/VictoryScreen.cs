using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    public AudioSource audioSource; // Victory screen's AudioSource
    public AudioClip victoryMusic; // Victory music clip
    [Range(0f, 1f)] public float victoryVolume = 1f; // Volume for the victory music

    private bool musicPlayed = false; // Ensure victory music plays only once

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // Add AudioSource if not already attached
            audioSource.loop = true;
            audioSource.playOnAwake = false;
            audioSource.volume = victoryVolume;
        }
    }

    void OnEnable()
    {
        StopAllOtherAudio(); // Stop other music
        PlayVictoryMusic(); // Play the victory music
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("Level-0", LoadSceneMode.Single);
        DestroyDontDestroyOnLoadObjects();
        Time.timeScale = 1f;
    }

    public void ExitButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void DestroyDontDestroyOnLoadObjects()
    {
        GameObject[] rootObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in rootObjects)
        {
            if (obj.scene.name == null || obj.scene.name == "DontDestroyOnLoad")
            {
                Destroy(obj);
            }
        }
    }

    private void PlayVictoryMusic()
    {
        if (!musicPlayed && victoryMusic != null && audioSource != null)
        {
            audioSource.clip = victoryMusic;
            audioSource.Play();
            musicPlayed = true;
        }
    }

    private void StopAllOtherAudio()
    {
        // Find all active AudioSources in the scene
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource source in allAudioSources)
        {
            // Stop all audio sources except the one attached to the VictoryScreen
            if (source != audioSource)
            {
                source.Stop();
            }
        }
    }
}
