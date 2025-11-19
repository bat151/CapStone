using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Escape / Win Settings")]
    public GameObject escapeObject;
    public int totalCollectibles = 3;
    public int collectedCount = 0;

    public Timer timer;

    [Header("Scene Names")]
    public string winSceneName = "Win";
    public string loseSceneName = "Lose";

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (escapeObject != null)
            escapeObject.SetActive(false);
    }

    public void AddCollectible()
    {
        collectedCount++;
        Debug.Log($"Collected {collectedCount}/{totalCollectibles}");

        if (collectedCount >= totalCollectibles)
            UnlockEscape();
    }

    private void UnlockEscape()
    {
        Debug.Log("All items collected! Escape object revealed!");
        if (escapeObject != null)
            escapeObject.SetActive(true);
    }

    public void WinGame()
    {
        // Stop timer but do NOT send JSON yet
        if (timer != null)
            timer.StopTimerWithoutSending();

        // Unlock cursor for input
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Load the Win scene
        SceneManager.LoadScene(winSceneName);


    }

    public void LoseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(loseSceneName);
    }
}
