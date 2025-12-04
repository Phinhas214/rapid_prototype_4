using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    
    [Header("Energy Bar")]
    [SerializeField] private UnityEngine.UI.Image energyBarFill;
    [SerializeField] private UnityEngine.UI.Image energyBarBackground;
    
    private float energyBarMaxWidth;
    
    void Start()
    {
        // Try to find UI elements if not assigned
        if (timerText == null)
        {
            timerText = GameObject.Find("TimerText")?.GetComponent<TextMeshProUGUI>();
        }
        
        if (scoreText == null)
        {
            scoreText = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();
        }
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        
        if (finalScoreText == null && gameOverPanel != null)
        {
            finalScoreText = gameOverPanel.transform.Find("FinalScoreText")?.GetComponent<TextMeshProUGUI>();
        }
        
        // Store the max width of the energy bar for scaling
        if (energyBarFill != null)
        {
            RectTransform fillRect = energyBarFill.GetComponent<RectTransform>();
            if (fillRect != null)
            {
                energyBarMaxWidth = fillRect.rect.width;
            }
        }
    }
    
    public void UpdateTimer(float timeRemaining)
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);
            timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
        }
    }
    
    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
    
    public void ShowGameOver(int finalScore)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        
        if (finalScoreText != null)
        {
            finalScoreText.text = "Seeds Planted: " + finalScore;
        }
    }
    
    public void UpdateEnergyBar(float energyPercentage)
    {
        if (energyBarFill != null)
        {
            RectTransform fillRect = energyBarFill.GetComponent<RectTransform>();
            if (fillRect != null)
            {
                // Clamp the percentage between 0 and 1
                float clampedPercentage = Mathf.Clamp01(energyPercentage);
                
                // Get the parent (background) width if available, otherwise use stored max width
                float maxWidth = energyBarMaxWidth;
                if (energyBarBackground != null)
                {
                    RectTransform bgRect = energyBarBackground.GetComponent<RectTransform>();
                    if (bgRect != null)
                    {
                        maxWidth = bgRect.rect.width;
                        // Account for any padding/margins (if you set margins in Step 2)
                        maxWidth -= 4f; // Subtract 4 for 2px margin on each side
                    }
                }
                
                // Set the width based on energy percentage
                fillRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWidth * clampedPercentage);
            }
        }
    }
}

