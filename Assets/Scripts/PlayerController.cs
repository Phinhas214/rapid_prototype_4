using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 90f; // degrees per second
    
    [Header("Player Sprite")]
    [SerializeField] private Sprite playerSprite; // Assign a player tile sprite here
    
    [Header("Shooting Settings")]
    [SerializeField] private GameObject seedPrefab;
    [SerializeField] private float shootCooldown = 0.2f;
    [SerializeField] private float seedSpeed = 10f;
    
    private float currentRotation = 0f;
    private float lastShootTime = 0f;
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            mainCamera = FindFirstObjectByType<Camera>();
        }
        
        // Get or add SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        
        // Set the player sprite if assigned
        if (playerSprite != null)
        {
            spriteRenderer.sprite = playerSprite;
        }
    }
    
    void Update()
    {
        HandleRotation();
        HandleShooting();
    }
    
    void HandleRotation()
    {
        float rotationInput = 0f;
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rotationInput = -1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rotationInput = 1f;
        }
        
        if (rotationInput != 0f)
        {
            currentRotation += rotationInput * rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, currentRotation);
        }
    }
    
    void HandleShooting()
    {
        if (Time.time - lastShootTime < shootCooldown)
            return;
        
        bool shootLeft = false;
        bool shootRight = false;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Shoot in both directions
            shootLeft = true;
            shootRight = true;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            // Shoot only left
            shootLeft = true;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            // Shoot only right
            shootRight = true;
        }
        
        if (shootLeft)
        {
            ShootSeed(-180f); // Left direction (180 degrees from current rotation)
        }
        
        if (shootRight)
        {
            ShootSeed(0f); // Right direction (same as current rotation)
        }
        
        if (shootLeft || shootRight)
        {
            lastShootTime = Time.time;
        }
    }
    
    void ShootSeed(float angleOffset)
    {
        if (seedPrefab == null)
        {
            Debug.LogWarning("Seed prefab not assigned to PlayerController!");
            return;
        }
        
        // Calculate the shooting direction
        float shootAngle = currentRotation + angleOffset;
        Quaternion shootRotation = Quaternion.Euler(0, 0, shootAngle);
        
        // Instantiate seed at player position
        GameObject seed = Instantiate(seedPrefab, transform.position, shootRotation);
        
        // Set the seed's velocity/direction
        SeedProjectile seedScript = seed.GetComponent<SeedProjectile>();
        if (seedScript != null)
        {
            seedScript.SetDirection(shootAngle, seedSpeed);
        }
    }
}

