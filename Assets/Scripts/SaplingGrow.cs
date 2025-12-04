using UnityEngine;

public class SaplingGrow : MonoBehaviour
{
    public float requiredHoldTime = 0.1f; 
    private float holdTimer = 0f;
    private bool playerNearby = false;

    private Transform player;
    private PlayerController playerController;

    private bool hasGrown = false; // ⭐ Prevent double scoring

    private Vector3 smallScale = new Vector3(0.3f, 0.3f, 1f);
    private Vector3 bigScale = new Vector3(1f, 1f, 1f);
    
    [Header("Audio")]
    [SerializeField] private AudioClip wateringSound;
    [SerializeField] private float wateringSoundVolume = 0.8f;
    private AudioSource audioSource;
    private bool isPlayingWateringSound = false;

    void Start()
    {
        transform.localScale = smallScale;

        player = GameObject.FindWithTag("Player").transform;
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        
        // Setup audio source for watering sound
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    void Update()
    {
        // Already grown? Then do nothing.
        if (hasGrown)
            return;
        playerNearby = Vector2.Distance(player.position, transform.position) < 1.2f;
        if (playerNearby && playerController.canWater && Input.GetKey(KeyCode.Space))
        {
            // Start playing watering sound if not already playing
            if (!isPlayingWateringSound)
            {
                StartWateringSound();
            }
            
            holdTimer += Time.deltaTime;

            if (holdTimer >= requiredHoldTime)
            {
                GrowIntoBigTree();
            }
        }
        else
        {
            // Stop watering sound if space is not being held or player moved away
            if (isPlayingWateringSound)
            {
                StopWateringSound();
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            holdTimer = 0;
            // Stop watering sound when space is released
            if (isPlayingWateringSound)
            {
                StopWateringSound();
            }
        }
    }

    void GrowIntoBigTree()
    {
        if (hasGrown)
            return;

        hasGrown = true; // ⭐ Set so it only happens once

        transform.localScale = bigScale;
        
        // Stop watering sound when plant grows
        if (isPlayingWateringSound)
        {
            StopWateringSound();
        }

        // ⭐ Add to score once
        if (playerController != null && playerController.treeScoreText != null)
        {
            playerController.treeCount++;
            playerController.treeScoreText.text =
                "Trees Watered: " + playerController.treeCount;
                MainMenuController.totalScore += 10;
        }
    }
    
    void StartWateringSound()
    {
        if (wateringSound != null && audioSource != null && !isPlayingWateringSound)
        {
            audioSource.clip = wateringSound;
            audioSource.volume = wateringSoundVolume;
            audioSource.loop = true; // Loop the sound while watering
            audioSource.Play();
            isPlayingWateringSound = true;
        }
    }
    
    void StopWateringSound()
    {
        if (audioSource != null && isPlayingWateringSound)
        {
            audioSource.Stop();
            isPlayingWateringSound = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = false;
            holdTimer = 0;
            // Stop watering sound when player moves away
            if (isPlayingWateringSound)
            {
                StopWateringSound();
            }
        }
    }
}