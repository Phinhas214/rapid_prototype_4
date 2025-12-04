using UnityEngine;
using TMPro;

public class WateringTimer : MonoBehaviour
{
    public float timeRemaining = 30f;
    public TextMeshProUGUI timerText;
    public PlayerController playerController;

    private bool timerRunning = false;
    private bool timeExpired = false;
    private Stage2GameManager gameManager;

    void Start()
    {
        timerText.text = Mathf.Ceil(timeRemaining).ToString();
        
        // Find Stage2GameManager
        gameManager = FindFirstObjectByType<Stage2GameManager>();
    }

    bool startTimeCondition()
    {
        return Input.GetKeyDown(KeyCode.Space) ||
                Input.GetKeyDown(KeyCode.W) ||
                Input.GetKeyDown(KeyCode.A) || 
                Input.GetKeyDown(KeyCode.S) || 
                Input.GetKeyDown(KeyCode.D) &&
                !timerRunning && playerController.canWater;
    }

    void Update()
    {
        // First input starts timer
        if (startTimeCondition() == true)
        {
            timerRunning = true;
        }

        if (timerRunning)
        {
            if (timeRemaining > 0f)
            {
                timeRemaining -= Time.deltaTime;
                timerText.text = Mathf.Ceil(timeRemaining).ToString();
            }
            else
            {
                timeRemaining = 0f;
                timerRunning = false;
                playerController.canWater = false;
                
                // Notify game manager that time ran out (only once)
                if (!timeExpired && gameManager != null)
                {
                    timeExpired = true;
                    gameManager.OnTimerExpired();
                }
            }
        }
    }
}