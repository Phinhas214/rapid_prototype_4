using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

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
    private int totalTrees;
    
    AudioSource audioSource;

    [SerializeField] private Tilemap collisionTilemap;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        totalTrees = plants.Length;
        AddPlants();
        SetCuttingUIPrefab(cuttingUIPrefab);
        treeCountText.text = totalTrees.ToString();
    }

    // Update is called once per frame
    void Update()
    {

        canCut = false;
        cuttablePlant = null;

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
        }
        treeCountText.text = totalTrees.ToString();
        audioSource.Play();

        Vector3 prefabPosition = new Vector3(playerTransform.localPosition.x, playerTransform.localPosition.y + 1, playerTransform.localPosition.z);
        

        GameObject cuttingUI = Instantiate(cuttingUIPrefabGO, prefabPosition, quaternion.identity);
        cuttingUI.GetComponent<TreeCuttingUI>().setPlant(cuttablePlant);

        if (totalTrees <= 0)
        {
            gameOverText.gameObject.SetActive(true);
        }
    }

    public void RemoveCollisionTileAt(Vector3Int cellPosition)
    {
        collisionTilemap.SetTile(cellPosition, null);
    }
}
