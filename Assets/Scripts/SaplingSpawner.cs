using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class SaplingSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject saplingPrefab;
    [SerializeField] private Transform saplingsParent; // The "Saplings" parent GameObject
    [SerializeField] private Tilemap groundTilemap; // The tilemap to check for valid spawn positions
    
    [Header("Spawn Area")]
    [SerializeField] private int minX = -10;
    [SerializeField] private int maxX = 10;
    [SerializeField] private int minY = -10;
    [SerializeField] private int maxY = 10;
    
    [Header("Minimum Distance")]
    [SerializeField] private float minDistanceBetweenSaplings = 1.5f; // Minimum distance between saplings
    
    private List<Vector3> spawnedPositions = new List<Vector3>();
    
    void Start()
    {
        // Get Stage 1 score (number of targets hit)
        int numberOfSaplings = MainMenuController.stage1Score;
        
        // If no score from Stage 1, default to a minimum (e.g., 5)
        if (numberOfSaplings <= 0)
        {
            numberOfSaplings = 5;
            Debug.LogWarning("Stage 1 score is 0 or invalid. Defaulting to 5 saplings.");
        }
        
        Debug.Log($"Spawning {numberOfSaplings} saplings based on Stage 1 score.");
        
        // Find parent if not assigned
        if (saplingsParent == null)
        {
            GameObject saplingsGO = GameObject.Find("Saplings");
            if (saplingsGO != null)
            {
                saplingsParent = saplingsGO.transform;
            }
            else
            {
                // Create parent if it doesn't exist
                saplingsParent = new GameObject("Saplings").transform;
            }
        }
        
        // Find ground tilemap if not assigned
        if (groundTilemap == null)
        {
            GameObject gridGO = GameObject.Find("Grid");
            if (gridGO != null)
            {
                groundTilemap = gridGO.GetComponentInChildren<Tilemap>();
            }
        }
        
        // Disable or destroy existing manually placed saplings
        DisableExistingSaplings();
        
        // Spawn saplings
        SpawnSaplings(numberOfSaplings);
    }
    
    void DisableExistingSaplings()
    {
        // Disable all existing sapling prefab instances
        if (saplingsParent != null)
        {
            for (int i = saplingsParent.childCount - 1; i >= 0; i--)
            {
                Transform child = saplingsParent.GetChild(i);
                if (child.name.Contains("SaplingPrefab"))
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }
    
    void SpawnSaplings(int count)
    {
        if (saplingPrefab == null)
        {
            Debug.LogError("SaplingSpawner: Sapling prefab not assigned!");
            return;
        }
        
        if (groundTilemap == null)
        {
            Debug.LogError("SaplingSpawner: Ground tilemap not found!");
            return;
        }
        
        spawnedPositions.Clear();
        int attempts = 0;
        int maxAttempts = count * 50; // Prevent infinite loops
        
        for (int i = 0; i < count && attempts < maxAttempts; attempts++)
        {
            // Get random grid position
            int randomX = Random.Range(minX, maxX + 1);
            int randomY = Random.Range(minY, maxY + 1);
            Vector3Int gridPos = new Vector3Int(randomX, randomY, 0);
            
            // Check if this tile exists on the ground tilemap
            if (!groundTilemap.HasTile(gridPos))
            {
                continue; // Skip invalid positions
            }
            
            // Convert grid position to world position
            Vector3 worldPos = groundTilemap.CellToWorld(gridPos);
            worldPos.z = 0; // Ensure Z is 0 for 2D
            
            // Check minimum distance from other saplings
            bool tooClose = false;
            foreach (Vector3 existingPos in spawnedPositions)
            {
                if (Vector3.Distance(worldPos, existingPos) < minDistanceBetweenSaplings)
                {
                    tooClose = true;
                    break;
                }
            }
            
            if (tooClose)
            {
                continue; // Try another position
            }
            
            // Spawn sapling at this position
            GameObject sapling = Instantiate(saplingPrefab, worldPos, Quaternion.identity, saplingsParent);
            spawnedPositions.Add(worldPos);
            i++;
        }
        
        if (spawnedPositions.Count < count)
        {
            Debug.LogWarning($"SaplingSpawner: Only spawned {spawnedPositions.Count} out of {count} saplings. " +
                           $"Try increasing the spawn area or reducing minimum distance.");
        }
        else
        {
            Debug.Log($"SaplingSpawner: Successfully spawned {spawnedPositions.Count} saplings.");
        }
    }
}

