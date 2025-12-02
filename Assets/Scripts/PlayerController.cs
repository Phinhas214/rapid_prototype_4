using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro; // Needed for score UI

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap collisionTilemap;

    public bool canWater = true;

    // ⭐ Tree score tracking
    public int treeCount = 0;
    [SerializeField] public TextMeshProUGUI treeScoreText;

    private PlayerMovement controls;

    private void Awake()
    {
        controls = new PlayerMovement();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Start()
    {
        controls.Main.Movement.performed += ctx =>
            Move(ctx.ReadValue<Vector2>());

        // ⭐ Initialize UI text
        if (treeScoreText != null)
        {
            treeScoreText.text = "Trees Planted: 0";
        }
    }

    private void Move(Vector2 direction)
    {
        if (CanMove(direction))
        {
            transform.position += (Vector3)direction;
        }
    }

    private bool CanMove(Vector2 direction)
    {
        Vector3Int gridPosition =
            groundTilemap.WorldToCell(transform.position + (Vector3)direction);

        if (!groundTilemap.HasTile(gridPosition) ||
            collisionTilemap.HasTile(gridPosition))
        {
            return false;
        }

        return true;
    }
}