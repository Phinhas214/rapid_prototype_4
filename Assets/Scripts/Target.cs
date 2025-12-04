using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float scrollSpeed = 3f;
    
    [Header("Scoring")]
    [SerializeField] private int pointsValue = 1;
    
    private GameManager gameManager;
    private bool hasBeenHit = false;
    
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }
    
    void Update()
    {
        // Scroll down
        transform.position += Vector3.down * scrollSpeed * Time.deltaTime;
        
        // Destroy if off-screen (below camera view)
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }
    
    public void OnHit()
    {
        if (hasBeenHit)
            return;
        
        hasBeenHit = true;
        
        // Award points
        if (gameManager != null)
        {
            gameManager.AddScore(pointsValue);
        }
        
        // Visual feedback (optional - can be enhanced later)
        // For now, just destroy the target
        Destroy(gameObject);
    }
}

