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
    
    private PlayerController playerController;
    private bool gameWon = false;
    private bool gameLost = false;
    private bool gameActive = true;
    
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

