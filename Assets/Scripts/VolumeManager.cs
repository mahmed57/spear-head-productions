using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public Slider SFXSlider;
    public Slider MusicSlider;

    public AudioMixer SFXMixer;
    public AudioMixer MusicMixer;

    private void Start()
    {
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);

        UpdateSFXVolume(SFXSlider.value);
        UpdateMusicVolume(MusicSlider.value);

        SFXSlider.onValueChanged.AddListener(UpdateSFXVolume);
        MusicSlider.onValueChanged.AddListener(UpdateMusicVolume);
    }

    public void UpdateSFXVolume(float value)
    {
        float clampedValue = Mathf.Clamp(value, 0.0001f, 1f);
        SFXMixer.SetFloat("SFXVolume", Mathf.Log10(clampedValue) * 20); 
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    public void UpdateMusicVolume(float value)
    {
        float clampedValue = Mathf.Clamp(value, 0.0001f, 1f);
        MusicMixer.SetFloat("MusicVolume", Mathf.Log10(clampedValue) * 20); 
        PlayerPrefs.SetFloat("MusicVolume", value);
    }
}
