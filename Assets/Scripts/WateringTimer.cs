using UnityEngine;
using TMPro;

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

    void Update()
    {
        // First Space press starts timer AND hides instructions
        if (Input.GetKeyDown(KeyCode.Space) && !timerRunning && playerController.canWater)
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
    }
}