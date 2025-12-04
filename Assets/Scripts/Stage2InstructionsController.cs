using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Stage2InstructionsController : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string stage1SceneName = "Stage-1";
    [SerializeField] private string stage2SceneName = "Stage_2";

    [Header("Audio (Optional)")]
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private float soundDelay = 0.3f;

    private AudioSource audioSource;
    private bool isProcessing = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    private void PlayButtonSound()
    {
        if (buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }

    public void GoToStage2()
    {
        if (isProcessing) return;
        isProcessing = true;

        PlayButtonSound();
        StartCoroutine(LoadSceneAfterDelay(stage2SceneName));
    }

    public void BackToStage1()
    {
        if (isProcessing) return;
        isProcessing = true;

        PlayButtonSound();
        StartCoroutine(LoadSceneAfterDelay(stage1SceneName));
    }

    private IEnumerator LoadSceneAfterDelay(string sceneName)
    {
        if (soundDelay > 0f)
        {
            yield return new WaitForSeconds(soundDelay);
        }

        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Stage2InstructionsController: Scene name is empty.");
            isProcessing = false;
        }
    }
}

