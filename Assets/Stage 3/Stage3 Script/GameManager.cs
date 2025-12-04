using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;

public class GameManger : MonoBehaviour
{
    [SerializeField] private GameObject[] plants; 
    [SerializeField] private GameObject UIPopUpPrefab;
    [SerializeField] private  GameObject cuttingUIPrefab;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float plantToPlayerDistance = 1f;
    public static bool canCut = false;
    public bool isCutting = false;
    public static GameObject cuttablePlant;
    [SerializeField] private GameObject UIPopUpGO = null;
    private static GameObject cuttingUIPrefabGO;
    [SerializeField] public static bool autoRemove = true; 
    
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text treeCountText;
    [SerializeField] private Text timerText; // Legacy UI Text component
    [SerializeField] private TextMeshProUGUI timerTextTMP; // TextMeshPro component (alternative)
    [SerializeField] private Text finalScoreText;
    private int totalTrees;
    
    AudioSource audioSource;
    AudioSource backgroundMusicSource;

    [SerializeField] private Tilemap collisionTilemap;
    
    [Header("Background Music")]
    [SerializeField] private AudioClip stage3BGM;
    [SerializeField] private float bgmVolume = 0.5f;
    
    [Header("Game Timer")]
    [SerializeField] private float gameTimeLimit = 30f; // 30 seconds
    private float currentTime;
    private bool gameActive = true;
    public static bool isGameOver = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        // Setup background music
        SetupBackgroundMusic();
        
        totalTrees = plants.Length;
        AddPlants();
        SetCuttingUIPrefab(cuttingUIPrefab);
        treeCountText.text = totalTrees.ToString();
        
        // Initialize timer
        currentTime = gameTimeLimit;
        gameActive = true;
        isGameOver = false;
        
        // Find timer text if not assigned
        if (timerText == null && timerTextTMP == null)
        {
            GameObject foundTimer = GameObject.Find("TimerText");
            if (foundTimer != null)
            {
                timerText = foundTimer.GetComponent<Text>();
                if (timerText == null)
                {
                    timerTextTMP = foundTimer.GetComponent<TextMeshProUGUI>();
                }
            }
            
            if (timerText == null && timerTextTMP == null)
            {
                Debug.LogWarning("TimerText not found! Please create a UI Text or TextMeshPro component named 'TimerText' in the scene.");
            }
        }
        
        // Initialize timer display
        UpdateTimerDisplay();
    }
    
    void SetupBackgroundMusic()
    {
        // Create a separate AudioSource for background music
        backgroundMusicSource = gameObject.AddComponent<AudioSource>();
        backgroundMusicSource.playOnAwake = false;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.volume = bgmVolume;
        
        // Play background music if assigned
        if (stage3BGM != null)
        {
            backgroundMusicSource.clip = stage3BGM;
            backgroundMusicSource.Play();
        }
        else
        {
            Debug.LogWarning("Stage-3BGM not assigned! Please assign 'Stage-3BGM' from Assets/Audio folder to the GameManger component in the Unity Inspector.");
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!gameActive)
            return;

        // Update timer
        currentTime -= Time.deltaTime;
        
        // Update timer display
        UpdateTimerDisplay();
        
        // Check if time ran out
        if (currentTime <= 0f)
        {
            currentTime = 0f;
            if (totalTrees > 0)
            {
                // Player lost - didn't cut all trees in time
                GameOver(false);
            }
            return;
        }

        canCut = false;
        cuttablePlant = null;

        // Don't process plant interactions if game is over
        if (!isGameOver)
        {
            for(int i = 0; i < plants.Length; i++)
            {
                if(IsPlantCloseToPlayer(plants[i].transform))
                {
                    // set values so the player controller knows that it can cut
                    // set up cut option popup
                    canCut = true;
                    cuttablePlant = plants[i];

                    if(UIPopUpGO == null && !isCutting && !plants[i].GetComponent<Plant>().isChopped)
                    {
                        Vector3 prefabPosition = new Vector3(plants[i].transform.localPosition.x, plants[i].transform.localPosition.y + 1, plants[i].transform.localPosition.z);
                        UIPopUpGO = Instantiate(UIPopUpPrefab, prefabPosition, Quaternion.identity);
                    }
                }
            }
        }

        if(!canCut)
        {
            Destroy(UIPopUpGO);
        }

        if (FindAnyObjectByType<TreeCuttingUI>())
        {
            isCutting = true;
        }
        else
        {
            isCutting = false;
        }
    }

    static void SetCuttingUIPrefab(GameObject prefab)
    {
        cuttingUIPrefabGO = prefab; 
    }

    void AddPlants() 
    {
        // Get number of plants from other stage and plant them
        //TODO : implement when other stages have figured out how to tranfer that information over 
    }

    bool IsPlantCloseToPlayer(Transform plantTransform)
    {
        Vector3 plantVector = plantTransform.localPosition;
        Vector3 playerVector = playerTransform.localPosition;
        float distance = Vector3.Distance(playerVector, plantVector);
        if (distance < plantToPlayerDistance)
        {
            
            return true;
        }
        return false;
    }

    public void DestroyPopUp()
    {
        Destroy(UIPopUpGO);
    }

    

    public void StartCutting(Transform playerTransform)
    {
        Vector3Int CellPos = collisionTilemap.WorldToCell(cuttablePlant.transform.position);
        RemoveCollisionTileAt(CellPos);
        Destroy(UIPopUpGO);

        if (!cuttablePlant.GetComponent<Plant>().isChopped)
        {
            totalTrees--;
            MainMenuController.totalScore += 10;
        }
        treeCountText.text = totalTrees.ToString();
        audioSource.Play();

        Vector3 prefabPosition = new Vector3(playerTransform.localPosition.x, playerTransform.localPosition.y + 1, playerTransform.localPosition.z);
        

        GameObject cuttingUI = Instantiate(cuttingUIPrefabGO, prefabPosition, quaternion.identity);
        cuttingUI.GetComponent<TreeCuttingUI>().setPlant(cuttablePlant);

        if (totalTrees <= 0 && gameActive)
        {
            // Player won - cut all trees before time ran out
            GameOver(true);
        }
    }

    public void RemoveCollisionTileAt(Vector3Int cellPosition)
    {
        collisionTilemap.SetTile(cellPosition, null);
    }
    
    private void UpdateTimerDisplay()
    {
        int seconds = Mathf.CeilToInt(Mathf.Max(0, currentTime));
        
        if (timerTextTMP != null)
        {
            timerTextTMP.text = seconds.ToString();
        }
        else if (timerText != null)
        {
            timerText.text = seconds.ToString();
        }
    }
    
    private void GameOver(bool won)
    {
        gameActive = false;
        isGameOver = true;
        
        // Stop background music
        if (backgroundMusicSource != null && backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Stop();
        }
        
        // Disable player movement
        Stage3PlayerController playerController = FindFirstObjectByType<Stage3PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }
        
        // Show game over message
        if (gameOverText != null)
        {
            if (won)
            {
                gameOverText.text = "YOU WIN!";
            }
            else
            {
                gameOverText.text = "YOU LOSE";
            }
            gameOverText.gameObject.SetActive(true);
            Debug.Log("final score: " + MainMenuController.totalScore.ToString());
            finalScoreText.text = "Final Score: " + MainMenuController.totalScore.ToString();
            finalScoreText.gameObject.SetActive(true);
        }
    }
}
