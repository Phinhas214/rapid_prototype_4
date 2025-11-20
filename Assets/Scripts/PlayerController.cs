using System;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap collisionTilemap;

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
        
        controls.Main.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
        Debug.Log("Before Move call");
    }

    private void Move(Vector2 direction)
    {
        Debug.Log("Move called");
        if (CanMove(direction))
        {
            transform.position += (Vector3)direction;
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
