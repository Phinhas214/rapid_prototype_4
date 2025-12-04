using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class PlantSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject plantPrefab;
    [SerializeField] private Transform plantsParent; // Parent GameObject for plants
    [SerializeField] private Tilemap groundTilemap; // The tilemap to check for valid spawn positions
    [SerializeField] private Tilemap collisionTilemap; // Collision tilemap to avoid
    
    [Header("Spawn Area")]
    [SerializeField] private int minX = -10;
    [SerializeField] private int maxX = 10;
    [SerializeField] private int minY = -10;
    [SerializeField] private int maxY = 10;
    
    [Header("Minimum Distance")]
    [SerializeField] private float minDistanceBetweenPlants = 1.5f; // Minimum distance between plants
    
    private List<Vector3> spawnedPositions = new List<Vector3>();
    private GameObject[] spawnedPlants;
    
    void Start()
    {
        // Get Stage 2 trees watered count
        int numberOfPlants = MainMenuController.stage2TreesWatered;
        
        // If no trees watered from Stage 2, default to a minimum (e.g., 5)
        if (numberOfPlants <= 0)
        {
            numberOfPlants = 5;
            Debug.LogWarning("Stage 2 trees watered is 0 or invalid. Defaulting to 5 plants.");
        }
        
        Debug.Log($"Spawning {numberOfPlants} plants based on Stage 2 trees watered ({MainMenuController.stage2TreesWatered}).");
        
        // Find parent if not assigned
        if (plantsParent == null)
        {
            GameObject plantsGO = GameObject.Find("Plants");
            if (plantsGO != null)
            {
                plantsParent = plantsGO.transform;
            }
            else
            {
                // Create parent if it doesn't exist
                plantsParent = new GameObject("Plants").transform;
            }
        }
        
        // Find ground tilemap if not assigned
        if (groundTilemap == null)
        {
            GameObject gridGO = GameObject.Find("Grid");
            if (gridGO != null)
            {
                Tilemap[] tilemaps = gridGO.GetComponentsInChildren<Tilemap>();
                foreach (Tilemap tm in tilemaps)
                {
                    if (tm.name.Contains("Ground") || tm.name.Contains("Tilemap"))
                    {
                        groundTilemap = tm;
                        break;
                    }
                }
                // If still not found, use first tilemap
                if (groundTilemap == null && tilemaps.Length > 0)
                {
                    groundTilemap = tilemaps[0];
                }
            }
        }
        
        // Find collision tilemap if not assigned
        if (collisionTilemap == null)
        {
            GameObject gridGO = GameObject.Find("Grid");
            if (gridGO != null)
            {
                Tilemap[] tilemaps = gridGO.GetComponentsInChildren<Tilemap>();
                foreach (Tilemap tm in tilemaps)
                {
                    if (tm.name.Contains("Collision"))
                    {
                        collisionTilemap = tm;
                        break;
                    }
                }
            }
        }
        
        // Spawn plants
        spawnedPlants = SpawnPlants(numberOfPlants);
        
        // Update GameManger with spawned plants array
        UpdateGameManagerPlants();
    }
    
    GameObject[] SpawnPlants(int count)
    {
        if (plantPrefab == null)
        {
            Debug.LogError("PlantSpawner: Plant prefab not assigned!");
            return new GameObject[0];
        }
        
        if (groundTilemap == null)
        {
            Debug.LogError("PlantSpawner: Ground tilemap not found!");
            return new GameObject[0];
        }
        
        List<GameObject> plants = new List<GameObject>();
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
            
            // Check if position is blocked by collision tilemap
            if (collisionTilemap != null && collisionTilemap.HasTile(gridPos))
            {
                continue; // Skip blocked positions
            }
            
            // Convert grid position to world position
            Vector3 worldPos = groundTilemap.CellToWorld(gridPos);
            worldPos.z = 0; // Ensure Z is 0 for 2D
            
            // Check minimum distance from other plants
            bool tooClose = false;
            foreach (Vector3 existingPos in spawnedPositions)
            {
                if (Vector3.Distance(worldPos, existingPos) < minDistanceBetweenPlants)
                {
                    tooClose = true;
                    break;
                }
            }
            
            if (tooClose)
            {
                continue; // Try another position
            }
            
            // Spawn plant at this position
            GameObject plant = Instantiate(plantPrefab, worldPos, Quaternion.identity, plantsParent);
            spawnedPositions.Add(worldPos);
            plants.Add(plant);
            i++;
        }
        
        if (plants.Count < count)
        {
            Debug.LogWarning($"PlantSpawner: Only spawned {plants.Count} out of {count} plants. " +
                           $"Try increasing the spawn area or reducing minimum distance.");
        }
        else
        {
            Debug.Log($"PlantSpawner: Successfully spawned {plants.Count} plants.");
        }
        
        return plants.ToArray();
    }
    
    void UpdateGameManagerPlants()
    {
        // Find GameManger and update its plants array
        GameManger gameManager = FindFirstObjectByType<GameManger>();
        if (gameManager != null && spawnedPlants != null && spawnedPlants.Length > 0)
        {
            // Use reflection or create a public method to set plants array
            // For now, we'll use a public method we'll add to GameManger
            gameManager.SetPlantsArray(spawnedPlants);
            Debug.Log($"PlantSpawner: Updated GameManger with {spawnedPlants.Length} plants.");
        }
        else if (gameManager == null)
        {
            Debug.LogWarning("PlantSpawner: GameManger not found! Plants spawned but not assigned to GameManger.");
        }
    }
    
    public GameObject[] GetSpawnedPlants()
    {
        return spawnedPlants;
    }
}

