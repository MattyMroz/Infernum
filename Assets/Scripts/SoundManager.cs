using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider generalSlider;
    [SerializeField] private SoundtrackManager soundtrackManager;

    private float MsliderValue = 1f;
    private float GsliderValue = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        soundtrackManager.SetVolume(1f);
        AudioListener.volume = 1.0f;

        musicSlider.value = 1.0f;
        generalSlider.value = 1.0f;

        MsliderValue = 1f;
        GsliderValue = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMusicVolume()
    {
        MsliderValue = musicSlider.value;
        soundtrackManager.SetVolume(MsliderValue);
    }

    public void SetGeneralVolume()
    {
        GsliderValue = generalSlider.value;
        AudioListener.volume = GsliderValue;
    }
}
