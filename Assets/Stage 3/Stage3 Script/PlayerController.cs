using System;
using System.IO.Compression;
using UnityEditor.Rendering.CustomRenderTexture.ShaderGraph;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap collisionTilemap;
    [SerializeField] private GameManger gameManger;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] playerSprites;

    private PlayerMovement controls;

    private void Awake()
    {
        controls = new PlayerMovement();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void Disable()
    {
        controls.Disable();
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        controls.Main.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
        controls.Main.Cut.performed += ctx => Cut();
        Debug.Log("Before Move call");
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
        if (CanMove(direction) && !gameManger.isCutting)
        {
            transform.position += (Vector3)direction;
        }
    }

    private void Cut()
    {
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
