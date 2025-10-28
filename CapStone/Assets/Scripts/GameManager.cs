using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Escape Settings")]
    public GameObject escapeObject;    // The object to show when all are collected
    public int totalCollectibles = 3;  // Number of collectibles needed

    private int collectedCount = 0;

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
}