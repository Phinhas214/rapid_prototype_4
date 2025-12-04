using UnityEngine;
using TMPro;
using System.Threading;

public class WateringTimer : MonoBehaviour
{
    public float timeRemaining = 30f;
    public TextMeshProUGUI timerText;
    public PlayerController playerController;
    public TextMeshProUGUI instructionText; // ⭐ New instruction UI text

    private bool timerRunning = false;

    void Start()
    {
        timerText.text = Mathf.Ceil(timeRemaining).ToString();
        instructionText.gameObject.SetActive(true); // Show instructions at start
    }

    bool startTimeCondition()
    {
        return Input.GetKeyDown(KeyCode.Space) ||
                Input.GetKeyDown(KeyCode.W) ||
                Input.GetKeyDown(KeyCode.A) || 
                Input.GetKeyDown(KeyCode.S) || 
                Input.GetKeyDown(KeyCode.D) &&
                !timerRunning && playerController.canWater;
    }

    void Update()
    {
        // First Space press starts timer AND hides instructions
        if (startTimeCondition() == true)
        {
            timerRunning = true;
            instructionText.gameObject.SetActive(false);
        }

        if (timerRunning)
        {
            if (timeRemaining > 0f)
            {
                timeRemaining -= Time.deltaTime;
                timerText.text = Mathf.Ceil(timeRemaining).ToString();
            }
            else
            {
                timeRemaining = 0f;
                timerRunning = false;
                playerController.canWater = false;
            }
        }
        else if (timeRemaining <= 0f)
        {
            //Time.timeScale = 0f;
        }
    }
}