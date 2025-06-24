using System.Collections;
using System.Drawing;
using UnityEngine;

public class SoundtrackManager : MonoBehaviour
{

    [Header("AudioClips")]
    [SerializeField] private AudioClip[] audioClips;

    private AudioSource audioSource;
    private int lastAudioClipIndex = 0;
    public float baseAudioVolume { get; private set; }

    [Header("Fade Values")]
    [SerializeField] private float audioFadeDuration;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        baseAudioVolume = audioSource.volume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator PlayMusic()
    {
        float elapsed = 0f;
        float audioVolume = audioSource.volume;

        while (elapsed < audioFadeDuration)
        {
            float t = elapsed / audioFadeDuration;
            audioVolume = Mathf.Lerp(baseAudioVolume, 0f, t);
            audioSource.volume = audioVolume;
            elapsed += UnityEngine.Time.deltaTime;
            yield return null;
        }

        audioSource.Stop();

        int randomAudioIndex = Random.Range(0, audioClips.Length);

        while (randomAudioIndex == lastAudioClipIndex)
        {
            randomAudioIndex = Random.Range(0, audioClips.Length);
        }

        lastAudioClipIndex = randomAudioIndex;
        AudioClip newAudioClip = audioClips[randomAudioIndex];

        audioSource.PlayOneShot(newAudioClip);

        elapsed = 0f;

        while (elapsed < audioFadeDuration)
        {
            float t = elapsed / audioFadeDuration;
            audioVolume = Mathf.Lerp(0f, baseAudioVolume, t);
            audioSource.volume = audioVolume;
            elapsed += UnityEngine.Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator FadeMusic(float fromVolume, float toVolume)
    {

        if(toVolume > 0f)
        {
            int randomAudioIndex = Random.Range(0, audioClips.Length);

            while (randomAudioIndex == lastAudioClipIndex)
            {
                randomAudioIndex = Random.Range(0, audioClips.Length);
            }

            lastAudioClipIndex = randomAudioIndex;

            AudioClip newAudioClip = audioClips[randomAudioIndex];

            audioSource.PlayOneShot(newAudioClip);
        }

        float elapsed = 0f;
        float audioVolume = audioSource.volume;

        while (elapsed < audioFadeDuration)
        {
            float t = elapsed / audioFadeDuration;
            audioVolume = Mathf.Lerp(fromVolume, toVolume, t);
            audioSource.volume = audioVolume;
            elapsed += UnityEngine.Time.deltaTime;
            yield return null;
        }

        audioVolume = toVolume;
        audioSource.volume = audioVolume;

        if(toVolume <= 0f)
            audioSource.Stop();
    }

    public void StopAll()
    {
        audioSource.Stop();
    }

    public void SetVolume(float volume)
    {
        baseAudioVolume = volume;
        audioSource.volume = baseAudioVolume;
    }

    public void PlaySoundtrack()
    {
        audioSource.Stop();

        int randomAudioIndex = Random.Range(0, audioClips.Length);

        while (randomAudioIndex == lastAudioClipIndex)
        {
            randomAudioIndex = Random.Range(0, audioClips.Length);
        }

        lastAudioClipIndex = randomAudioIndex;

        AudioClip newAudioClip = audioClips[randomAudioIndex];

        audioSource.PlayOneShot(newAudioClip);
    }
}
