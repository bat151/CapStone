using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Escape Settings")]
    public GameObject escapeObject;    // The object to show when all are collected
    public int totalCollectibles = 3;  // Number of collectibles needed

    private int collectedCount = 0;

    [Header("Scene Names")]
    public string winSceneName = "Win";
    public string loseSceneName = "Lose";

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Hide the escape object at start
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
        // Unlock and show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(winSceneName);
    }

    public void LoseGame()
    {
        // Unlock and show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(loseSceneName);
    }

}