using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string gameSceneName = "Stage-1"; // Change this to your game scene name
    
    [Header("Audio (Optional)")]
    [SerializeField] private AudioSource buttonClickSound;
    
    public void StartGame()
    {
        // Play sound if available
        if (buttonClickSound != null)
        {
            buttonClickSound.Play();
        }
        
        // Load the game scene
        if (!string.IsNullOrEmpty(gameSceneName))
        {
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogWarning("Game scene name not set in MainMenuController!");
        }
    }
    
    public void ExitGame()
    {
        // Play sound if available
        if (buttonClickSound != null)
        {
            buttonClickSound.Play();
        }
        
        // Exit the application
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}

