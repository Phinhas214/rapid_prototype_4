using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private float gameDuration = 60f; // 1 minute
    [SerializeField] private GameObject targetPrefab;
    
    [Header("Target Spawning")]
    [SerializeField] private float spawnInterval = 2f; // Kept for backward compatibility / default
    [SerializeField] private float initialSpawnInterval = 2f; // Interval at start of game
    [SerializeField] private float finalSpawnInterval = 0.5f; // Interval near end of game (faster spawning)
    [SerializeField] private float spawnXMin = -8f;
    [SerializeField] private float spawnXMax = 8f;
    [SerializeField] private float spawnY = 6f;
    
    private float currentTime;
    private int score = 0;
    private bool gameActive = false;
    private Camera mainCamera;
    private UIManager uiManager;
    
    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            mainCamera = FindFirstObjectByType<Camera>();
        }
        
        uiManager = FindFirstObjectByType<UIManager>();
        
        StartGame();
    }
    
    public void StartGame()
    {
        currentTime = gameDuration;
        score = 0;
        gameActive = true;
        
        if (uiManager != null)
        {
            uiManager.UpdateScore(score);
            uiManager.UpdateTimer(currentTime);
        }
        
        // Start spawning targets
        StartCoroutine(SpawnTargets());
    }
    
    void Update()
    {
        if (!gameActive)
            return;
        
        // Update timer
        currentTime -= Time.deltaTime;
        
        if (currentTime <= 0f)
        {
            currentTime = 0f;
            EndGame();
        }
        
        if (uiManager != null)
        {
            uiManager.UpdateTimer(currentTime);
        }
    }
    
    IEnumerator SpawnTargets()
    {
        while (gameActive)
        {
            if (targetPrefab != null)
            {
                // Random X position within bounds
                float randomX = Random.Range(spawnXMin, spawnXMax);
                Vector3 spawnPosition = new Vector3(randomX, spawnY, 0f);
                
                Instantiate(targetPrefab, spawnPosition, Quaternion.identity);
            }

            // Calculate current spawn interval based on how much time has passed
            // 0 at start, 1 at end
            float elapsed = gameDuration - currentTime;
            float t = gameDuration > 0f ? Mathf.Clamp01(elapsed / gameDuration) : 1f;

            // Linearly interpolate between initial and final interval
            float currentInterval = Mathf.Lerp(initialSpawnInterval, finalSpawnInterval, t);

            yield return new WaitForSeconds(currentInterval);
        }
    }
    
    public void AddScore(int points)
    {
        if (!gameActive)
            return;
        
        score += points;
        
        if (uiManager != null)
        {
            uiManager.UpdateScore(score);
        }
    }
    
    void EndGame()
    {
        gameActive = false;
        
        if (uiManager != null)
        {
            uiManager.ShowGameOver(score);
        }
    }
    
    public bool IsGameActive()
    {
        return gameActive;
    }
    
    public float GetRemainingTime()
    {
        return currentTime;
    }
    
    public int GetScore()
    {
        return score;
    }
}

