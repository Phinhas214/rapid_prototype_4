using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Stage2GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private int totalSaplingsNeeded = 0; // Will be set from Stage 1 score
    
    [Header("UI References")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI treesGrownText;
    [SerializeField] private UnityEngine.UI.Button nextStageButton;
    
    [Header("Scene Settings")]
    [SerializeField] private string nextSceneName = "Stage 3";
    
    [Header("Audio")]
    [SerializeField] private BackgroundMusicPlayer backgroundMusicPlayer;
    [SerializeField] private AudioClip stage2BackgroundMusic;
    [SerializeField] private float bgmVolume = 0.5f;
    
    private PlayerController playerController;
    private bool gameWon = false;
    private bool gameLost = false;
    private bool gameActive = true;
    private AudioSource musicSource;
    
    void Start()
    {
        // Get total saplings needed from Stage 1 score
        totalSaplingsNeeded = MainMenuController.stage1Score;
        
        // If no score from Stage 1, default to 5
        if (totalSaplingsNeeded <= 0)
        {
            totalSaplingsNeeded = 5;
            Debug.LogWarning("Stage 1 score is 0. Defaulting to 5 saplings needed.");
        }
        
        // Setup background music
        SetupBackgroundMusic();
        
        // Find PlayerController
        playerController = FindFirstObjectByType<PlayerController>();
        
        // Find UI elements if not assigned
        if (gameOverPanel == null)
        {
            GameObject foundPanel = GameObject.Find("GameOver");
            if (foundPanel != null)
            {
                gameOverPanel = foundPanel;
            }
        }
        
        // Hide game over panel at start
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        
        // Setup button if assigned
        if (nextStageButton != null)
        {
            nextStageButton.onClick.AddListener(GoToNextStage);
        }
        
        Debug.Log($"Stage 2: Need to grow {totalSaplingsNeeded} trees.");
    }
    
    void SetupBackgroundMusic()
    {
        // Try to find BackgroundMusicPlayer if not assigned
        if (backgroundMusicPlayer == null)
        {
            backgroundMusicPlayer = FindFirstObjectByType<BackgroundMusicPlayer>();
        }
        
        // If BackgroundMusicPlayer exists, use it
        if (backgroundMusicPlayer != null)
        {
            if (stage2BackgroundMusic != null)
            {
                backgroundMusicPlayer.SetVolume(bgmVolume);
                // Note: BackgroundMusicPlayer might need a method to change clip
                // For now, we'll create our own AudioSource if needed
            }
        }
        
        // Create our own AudioSource for background music if BackgroundMusicPlayer doesn't exist or doesn't support changing clips
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.playOnAwake = false;
            musicSource.loop = true;
            musicSource.volume = bgmVolume;
        }
        
        // Play background music if assigned
        if (stage2BackgroundMusic != null && musicSource != null)
        {
            musicSource.clip = stage2BackgroundMusic;
            musicSource.Play();
        }
        else if (stage2BackgroundMusic == null)
        {
            Debug.LogWarning("Stage2GameManager: Stage 2 background music not assigned! Please assign 'Stage2BackgroundMusic' in the Unity Inspector.");
        }
    }
    
    void Update()
    {
        if (!gameActive || gameWon || gameLost)
            return;
        
        // Check if all trees are grown
        if (playerController != null && playerController.treeCount >= totalSaplingsNeeded)
        {
            WinGame();
        }
    }
    
    void WinGame()
    {
        if (gameWon)
            return;
        
        gameWon = true;
        gameActive = false;
        
        // Stop background music
        StopBackgroundMusic();
        
        // Save trees watered count for Stage 3
        if (playerController != null)
        {
            MainMenuController.stage2TreesWatered = playerController.treeCount;
        }
        
        // Disable player movement
        if (playerController != null)
        {
            playerController.enabled = false;
        }
        
        // Show game over panel with win message
        ShowGameOverPanel("ALL TREES GROWN!");
        
        Debug.Log($"Stage 2 Complete! All {totalSaplingsNeeded} trees grown! Trees watered: {playerController.treeCount}");
    }
    
    public void OnTimerExpired()
    {
        if (gameLost || gameWon)
            return;
        
        gameLost = true;
        gameActive = false;
        
        // Stop background music
        StopBackgroundMusic();
        
        // Save trees watered count for Stage 3 (even if not all were grown)
        if (playerController != null)
        {
            MainMenuController.stage2TreesWatered = playerController.treeCount;
        }
        
        // Disable player movement
        if (playerController != null)
        {
            playerController.enabled = false;
        }
        
        // Show game over panel with lose message
        ShowGameOverPanel("YOU ALMOST GOT IT!");
        
        Debug.Log($"Stage 2: Time ran out! Trees grown: {playerController.treeCount} / {totalSaplingsNeeded}");
    }
    
    void StopBackgroundMusic()
    {
        // Stop BackgroundMusicPlayer if it exists
        if (backgroundMusicPlayer != null && backgroundMusicPlayer.IsPlaying())
        {
            backgroundMusicPlayer.StopMusic();
        }
        
        // Stop our own AudioSource
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
    
    void ShowGameOverPanel(string message = "ALL TREES GROWN!")
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            
            // Update game over text
            if (gameOverText != null)
            {
                gameOverText.text = message;
            }
            
            // Update trees grown text
            if (treesGrownText != null)
            {
                treesGrownText.text = $"Trees Grown: {playerController.treeCount} / {totalSaplingsNeeded}";
            }
        }
        else
        {
            Debug.LogWarning("Stage2GameManager: GameOver panel not found!");
        }
    }
    
    public void GoToNextStage()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Stage2GameManager: Next scene name not set!");
        }
    }
    
    public bool IsGameActive()
    {
        return gameActive;
    }
    
    public int GetTotalSaplingsNeeded()
    {
        return totalSaplingsNeeded;
    }
}

