using System;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Tilemaps;

public class Stage3PlayerController : MonoBehaviour
{
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap collisionTilemap;
    [SerializeField] private GameManger gameManger;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] playerSprites;

    private PlayerMovementStage3 controls;
    private bool hasMadeFirstMove = false;

    private void Awake()
    {
        controls = new PlayerMovementStage3();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        controls.Main.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
        controls.Main.Cut.performed += ctx => Cut();
    }

    private void Move(Vector2 direction)
    {
        Vector2 up = new Vector2(0, 1);
        Vector2 down = new Vector2(0, -1);
        Vector2 left = new Vector2(-1, 0);
        Vector2 right = new Vector2(1, 0);
        //Debug.Log("Move called");
        if (direction == up)
        {
            spriteRenderer.sprite = playerSprites[0];
        }
        else if (direction == down)
        {
            spriteRenderer.sprite = playerSprites[1];
        }
        else if (direction == left)
        {
            spriteRenderer.sprite = playerSprites[2];
        }
        else if (direction == right)
        {
            spriteRenderer.sprite = playerSprites[3];
        }
        // Don't allow movement if game is over
        if (GameManger.isGameOver)
            return;
            
        if (CanMove(direction) && !gameManger.isCutting)
        {
            transform.position += (Vector3)direction;
            
            // Start timer on first successful move
            if (!hasMadeFirstMove)
            {
                hasMadeFirstMove = true;
                
                // Start the timer when player makes first move
                if (gameManger != null)
                {
                    gameManger.StartTimer();
                }
            }
        }
    }

    private void Cut()
    {
        // Don't allow cutting if game is over
        if (GameManger.isGameOver)
            return;
            
        if (GameManger.canCut && !gameManger.isCutting)
        {
            gameManger.isCutting = true;
            gameManger.StartCutting(transform);
        }
    }

    private bool CanMove(Vector2 direction)
    {
        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position + (Vector3)direction);
        
        if (!groundTilemap.HasTile(gridPosition) || collisionTilemap.HasTile(gridPosition))
        {
            Debug.Log("Can't move!");
            return false;
        }
        return true;
    }
}
