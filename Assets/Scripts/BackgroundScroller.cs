using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Header("Scrolling Settings")]
    [SerializeField] private float scrollSpeed = 2f;
    [SerializeField] private bool loopBackground = true;
    
    private SpriteRenderer spriteRenderer;
    private float backgroundHeight;
    private Camera mainCamera;
    private GameObject duplicateBackground;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
        
        if (spriteRenderer != null)
        {
            backgroundHeight = spriteRenderer.bounds.size.y;
            
            // Create a duplicate background for seamless looping
            if (loopBackground)
            {
                CreateDuplicateBackground();
            }
        }
    }
    
    void CreateDuplicateBackground()
    {
        // Create a duplicate GameObject
        duplicateBackground = new GameObject("Background_Duplicate");
        duplicateBackground.transform.SetParent(transform.parent);
        
        // Copy the SpriteRenderer component
        SpriteRenderer duplicateSprite = duplicateBackground.AddComponent<SpriteRenderer>();
        duplicateSprite.sprite = spriteRenderer.sprite;
        duplicateSprite.color = spriteRenderer.color;
        duplicateSprite.sortingOrder = spriteRenderer.sortingOrder;
        duplicateSprite.sortingLayerID = spriteRenderer.sortingLayerID;
        
        // Position the duplicate directly above the original
        duplicateBackground.transform.position = new Vector3(
            transform.position.x,
            transform.position.y + backgroundHeight,
            transform.position.z
        );
        
        // Copy the scale
        duplicateBackground.transform.localScale = transform.localScale;
    }
    
    void Update()
    {
        if (spriteRenderer == null) return;
        
        // Scroll both backgrounds upward (move down in world space)
        transform.position += Vector3.down * scrollSpeed * Time.deltaTime;
        
        if (duplicateBackground != null)
        {
            duplicateBackground.transform.position += Vector3.down * scrollSpeed * Time.deltaTime;
        }
        
        // Seamless looping: when one background goes off-screen, reset it above the other
        if (loopBackground)
        {
            // Get camera bounds
            float cameraBottom = mainCamera != null ? mainCamera.transform.position.y - mainCamera.orthographicSize : -10f;
            
            // Check if original background has scrolled off the bottom
            if (transform.position.y + backgroundHeight * 0.5f < cameraBottom - 2f)
            {
                // Reset it above the duplicate
                if (duplicateBackground != null)
                {
                    transform.position = new Vector3(
                        transform.position.x,
                        duplicateBackground.transform.position.y + backgroundHeight,
                        transform.position.z
                    );
                }
            }
            
            // Check if duplicate background has scrolled off the bottom
            if (duplicateBackground != null)
            {
                if (duplicateBackground.transform.position.y + backgroundHeight * 0.5f < cameraBottom - 2f)
                {
                    // Reset it above the original
                    duplicateBackground.transform.position = new Vector3(
                        duplicateBackground.transform.position.x,
                        transform.position.y + backgroundHeight,
                        duplicateBackground.transform.position.z
                    );
                }
            }
        }
    }
    
    void OnDestroy()
    {
        // Clean up duplicate when this object is destroyed
        if (duplicateBackground != null)
        {
            Destroy(duplicateBackground);
        }
    }
}

