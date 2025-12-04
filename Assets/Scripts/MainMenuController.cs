using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string gameSceneName = "Stage-1";
    private string scene2Name = "Stage_2";
    private string scene3Name = "Stage 3";
    
    [Header("Audio")]
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private float soundDelay = 0.5f; // Delay before loading scene/quitting to let sound play
    [SerializeField] private float musicVolume = 0.5f; // Background music volume (0-1)
    
    private AudioSource audioSource;
    private AudioSource musicSource;
    private bool isProcessing = false; // Prevent multiple clicks

    public static int totalScore = 0; // Static variable to hold total score across scenes
    
    void Start()
    {
        // Get or create AudioSource component for button sounds
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
        
        // Create separate AudioSource for background music
        GameObject musicObject = new GameObject("BackgroundMusic");
        musicObject.transform.SetParent(transform);
        musicSource = musicObject.AddComponent<AudioSource>();
        musicSource.playOnAwake = false;
        musicSource.loop = true; // Loop the background music
        musicSource.volume = musicVolume;
        
        // Play background music if assigned
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
    }
    
    void PlayButtonSound()
    {
        if (buttonClickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }
    
    public void StartGame()
    {
        // Prevent multiple clicks
        if (isProcessing) return;
        isProcessing = true;
        
        // Play sound if available
        PlayButtonSound();
        
        // Wait for sound to play, then load scene
        StartCoroutine(LoadGameScene());
    }
    
    public void ExitGame()
    {
        // Prevent multiple clicks
        if (isProcessing) return;
        isProcessing = true;
        
        // Play sound if available
        PlayButtonSound();
        
        // Wait for sound to play, then exit
        StartCoroutine(QuitGame());
    }
    
    IEnumerator LoadGameScene()
    {
        // Wait for sound to play
        yield return new WaitForSeconds(soundDelay);

        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            // Load the game scene
            if (!string.IsNullOrEmpty(gameSceneName))
            {
                SceneManager.LoadScene(gameSceneName);
                yield break;
            }
            else
            {
                Debug.LogWarning("Game scene name not set in MainMenuController!");
                isProcessing = false; // Reset if scene name is invalid
            }
        }
        else if(SceneManager.GetActiveScene().name == "Stage-1")
        {
            // If currently in Stage 1 menu, load Stage 2 scene
            SceneManager.LoadScene(scene2Name);
            yield break;
        }
        else if(SceneManager.GetActiveScene().name == "Stage_2")
        {
            Debug.Log("loading stage 3");
            // If currently in Stage 2 menu, load Stage 3 scene
            SceneManager.LoadScene(scene3Name);
            yield break;
        }

        
    }
    
    IEnumerator QuitGame()
    {
        // Wait for sound to play
        yield return new WaitForSeconds(soundDelay);
        
        // Exit the application
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}

