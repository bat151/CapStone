using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Game object and how many collectibles and if theyve been picked up
    public GameObject escapeObject;
    public int totalCollectibles = 3;
    public int collectedCount = 0;

    // Reference to the timer script
    public Timer timer;

    // Scene names 
    public string winSceneName = "Win";
    public string loseSceneName = "Lose";

    private void Awake()
    {
        // For the timer to carry over through scenes
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Hide the escape object
        if (escapeObject != null)
            escapeObject.SetActive(false);
    }

    public void AddCollectible()
    {
        collectedCount++;
        Debug.Log($"Collected {collectedCount}/{totalCollectibles}");

        // Once all collectibles are found open the escape
        if (collectedCount >= totalCollectibles)
            UnlockEscape();
    }

    // Make the escape visable
    private void UnlockEscape()
    {
        Debug.Log("All items collected! Escape object revealed!");
        if (escapeObject != null)
            escapeObject.SetActive(true);
    }

    // When player makes it to the escape end game
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

    // When player loses send them to the lose screen
    public void LoseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(loseSceneName);
    }
}
