using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameOverController : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string replaySceneName = "Stage-1"; // Start from Stage 1 for replay
    
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI gameOverTitleText;
    
    [Header("Audio (Optional)")]
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private float soundDelay = 0.3f;
    
    private AudioSource audioSource;
    private bool isProcessing = false;
    
    void Start()
    {
        // Get or create AudioSource component for button sounds
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
        
        // Display the final score
        UpdateScoreDisplay();
    }
    
    void UpdateScoreDisplay()
    {
        // Get the final score from MainMenuController (trees cut × 100)
        int totalScore = MainMenuController.totalScore;
        
        // Update final score text
        if (finalScoreText != null)
        {
            finalScoreText.text = "Final Score: " + totalScore.ToString();
        }
        else
        {
            // Try to find it automatically
            GameObject foundScore = GameObject.Find("FinalScoreText");
            if (foundScore != null)
            {
                finalScoreText = foundScore.GetComponent<TextMeshProUGUI>();
                if (finalScoreText != null)
                {
                    finalScoreText.text = "Final Score: " + totalScore.ToString();
                }
            }
        }
        
        // Update game over title (optional - can show win/lose message)
        if (gameOverTitleText != null)
        {
            // You can customize this based on whether player won or lost
            gameOverTitleText.text = "GAME OVER";
        }
    }
    
    void PlayButtonSound()
    {
        if (buttonClickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }
    
    public void GoToMainMenu()
    {
        // Prevent multiple clicks
        if (isProcessing) return;
        isProcessing = true;
        
        // Reset scores for a fresh start
        MainMenuController.totalScore = 0;
        MainMenuController.stage1Score = 0;
        MainMenuController.stage2TreesWatered = 0;
        MainMenuController.stage3TreesCut = 0;
        
        // Play sound if available
        PlayButtonSound();
        
        // Wait for sound to play, then load scene
        StartCoroutine(LoadSceneAfterDelay(mainMenuSceneName));
    }
    
    public void ReplayGame()
    {
        // Prevent multiple clicks
        if (isProcessing) return;
        isProcessing = true;
        
        // Reset scores for replay
        MainMenuController.totalScore = 0;
        MainMenuController.stage1Score = 0;
        MainMenuController.stage2TreesWatered = 0;
        
        // Play sound if available
        PlayButtonSound();
        
        // Wait for sound to play, then load scene
        StartCoroutine(LoadSceneAfterDelay(replaySceneName));
    }
    
    IEnumerator LoadSceneAfterDelay(string sceneName)
    {
        // Wait for sound to play
        if (soundDelay > 0f)
        {
            yield return new WaitForSeconds(soundDelay);
        }
        
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("GameOverController: Scene name is empty.");
            isProcessing = false;
        }
    }
}

