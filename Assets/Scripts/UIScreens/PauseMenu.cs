using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;

    [Header("UI References")]
    public GameObject pauseMenuUI; 
    public GameObject optionsMenuUI; 

    [Header("Audio Settings")]
    public Slider musicSlider; 
    public Slider sfxSlider; 
    public AudioMixer musicMixer; 
    public AudioMixer sfxMixer; 

    private void Start()
    {
        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        musicSlider.value = savedMusicVolume;
        sfxSlider.value = savedSFXVolume;

        musicMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Max(savedMusicVolume, 0.01f)) * 20);
        sfxMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Max(savedSFXVolume, 0.01f)) * 20);

        musicSlider.onValueChanged.AddListener(delegate { UpdateMusicVolume(); });
        sfxSlider.onValueChanged.AddListener(delegate { UpdateSFXVolume(); });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (optionsMenuUI.activeSelf)
            {
                ExitOptionsMenu();
            }
            else if (pauseMenuUI.activeSelf)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadOptionsMenu()
    {
        Debug.Log("Options menu activated!");
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);
    }

    public void ExitOptionsMenu()
    {
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        // Quit the game
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void UpdateMusicVolume()
    {
        float volume;
        if (musicSlider.value <= 0.01f)
        {
            volume = -80f; 
        }
        else
        {
            volume = Mathf.Log10(musicSlider.value) * 20; 
        }
        musicMixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.Save();
    }

    public void UpdateSFXVolume()
    {
        float volume;
        if (sfxSlider.value <= 0.01f)
        {
            volume = -80f; 
        }
        else
        {
            volume = Mathf.Log10(sfxSlider.value) * 20; 
        }
        sfxMixer.SetFloat("SFXVolume", volume);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        PlayerPrefs.Save();
    }
}
