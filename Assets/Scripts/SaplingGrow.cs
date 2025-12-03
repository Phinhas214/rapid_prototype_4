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

    void Start()
    {
        transform.localScale = smallScale;

        player = GameObject.FindWithTag("Player").transform;
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        // Already grown? Then do nothing.
        if (hasGrown)
            return;
        playerNearby = Vector2.Distance(player.position, transform.position) < 1.2f;
        if (playerNearby && playerController.canWater && Input.GetKey(KeyCode.Space))
        {
            holdTimer += Time.deltaTime;

            if (holdTimer >= requiredHoldTime)
            {
                GrowIntoBigTree();
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            holdTimer = 0;
        }
    }

    void GrowIntoBigTree()
    {
        if (hasGrown)
            return;

        hasGrown = true; // ⭐ Set so it only happens once

        transform.localScale = bigScale;

        // ⭐ Add to score once
        if (playerController != null && playerController.treeScoreText != null)
        {
            playerController.treeCount++;
            playerController.treeScoreText.text =
                "Trees Watered: " + playerController.treeCount;
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
        }
    }
}