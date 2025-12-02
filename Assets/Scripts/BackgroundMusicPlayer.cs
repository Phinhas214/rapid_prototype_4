using UnityEngine;

public class BackgroundMusicPlayer : MonoBehaviour
{
    [Header("Background Music")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private float musicVolume = 0.5f; // Background music volume (0-1)
    [SerializeField] private bool playOnStart = true;
    [SerializeField] private bool loop = true;
    
    private AudioSource musicSource;
    
    void Start()
    {
        // Create AudioSource for background music
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.playOnAwake = false;
        musicSource.loop = loop;
        musicSource.volume = musicVolume;
        
        // Play background music if assigned
        if (backgroundMusic != null && playOnStart)
        {
            PlayMusic();
        }
    }
    
    public void PlayMusic()
    {
        if (backgroundMusic != null && musicSource != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
    }
    
    public void StopMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
    
    public void SetVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }
    }
    
    public bool IsPlaying()
    {
        return musicSource != null && musicSource.isPlaying;
    }
}

