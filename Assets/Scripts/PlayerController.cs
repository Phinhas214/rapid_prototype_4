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
    
    [Header("Shooting Resource Bar")]
    [SerializeField] private float maxEnergy = 10f;
    [SerializeField] private float energyRegenRate = 1f; // Energy per second
    [SerializeField] private float singleShotCost = 1f;
    [SerializeField] private float dualShotCost = 2f;
    
    [Header("Audio")]
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private float shootSoundVolume = 0.8f;
    
    private float currentRotation = 0f;
    private float lastShootTime = 0f;
    private float currentEnergy;
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;
    private UIManager uiManager;
    private AudioSource audioSource;
    
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
        
        // Initialize energy
        currentEnergy = maxEnergy;
        
        // Find UIManager for energy bar updates
        uiManager = FindFirstObjectByType<UIManager>();
        if (uiManager != null)
        {
            uiManager.UpdateEnergyBar(currentEnergy / maxEnergy);
        }

        // Get or add AudioSource component for shooting sound
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }
    
    void Update()
    {
        HandleRotation();
        HandleShooting();
        RegenerateEnergy();
    }
    
    void RegenerateEnergy()
    {
        if (currentEnergy < maxEnergy)
        {
            currentEnergy += energyRegenRate * Time.deltaTime;
            if (currentEnergy > maxEnergy)
            {
                currentEnergy = maxEnergy;
            }
            
            // Update UI
            if (uiManager != null)
            {
                uiManager.UpdateEnergyBar(currentEnergy / maxEnergy);
            }
        }
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
        float energyCost = 0f;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Shoot in both directions - costs 2 energy
            if (currentEnergy >= dualShotCost)
            {
                shootLeft = true;
                shootRight = true;
                energyCost = dualShotCost;
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            // Shoot only left - costs 1 energy
            if (currentEnergy >= singleShotCost)
            {
                shootLeft = true;
                energyCost = singleShotCost;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            // Shoot only right - costs 1 energy
            if (currentEnergy >= singleShotCost)
            {
                shootRight = true;
                energyCost = singleShotCost;
            }
        }
        
        // Only shoot if we have enough energy
        if (energyCost > 0f)
        {
            // Consume energy
            currentEnergy -= energyCost;
            if (currentEnergy < 0f)
            {
                currentEnergy = 0f;
            }
            
            // Update UI
            if (uiManager != null)
            {
                uiManager.UpdateEnergyBar(currentEnergy / maxEnergy);
            }
            
            // Perform shooting
            if (shootLeft)
            {
                ShootSeed(-180f); // Left direction (180 degrees from current rotation)
            }
            
            if (shootRight)
            {
                ShootSeed(0f); // Right direction (same as current rotation)
            }
            
            // Play shooting sound
            PlayShootSound();
            
            lastShootTime = Time.time;
        }
    }
    
    void PlayShootSound()
    {
        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound, shootSoundVolume);
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

