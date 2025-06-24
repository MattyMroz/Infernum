using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider generalSlider;
    [SerializeField] private AudioSource musicAudioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        musicAudioSource.volume = 1.0f;
        AudioListener.volume = 1.0f;

        musicSlider.value = 1.0f;
        generalSlider.value = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMusicVolume()
    {
        musicAudioSource.volume = musicSlider.value;
    }

    public void SetGeneralVolume()
    {
        AudioListener.volume = generalSlider.value;
    }
}
