using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class Timer : MonoBehaviour
{
    public static Timer Instance;

    [Header("UI Element")]
    public TextMeshProUGUI timerText;

    private float elapsedTime = 0f;
    private bool isRunning = true;

    private string apiUrlSubmit = "http://localhost:3000/submit";

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        if (timerText != null)
            timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void StopTimerWithoutSending()
    {
        isRunning = false;
        Debug.Log("Timer stopped at: " + elapsedTime);
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    public void SubmitBestTime(string playerID)
    {
        float finalTime = GetElapsedTime();
        StartCoroutine(SendBestTime(playerID, finalTime));
    }

    private IEnumerator SendBestTime(string playerID, float bestTime)
    {
        string dateAchieved = DateTime.Now.ToString("yyyy-MM-dd");

        string jsonData = JsonUtility.ToJson(new ScoreData
        {
            playerID = playerID,
            bestTime = bestTime,
            dateAchieved = dateAchieved
        });

        Debug.Log("Sending JSON: " + jsonData);

        UnityWebRequest request = new UnityWebRequest(apiUrlSubmit, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            Debug.Log("Score submitted successfully!");
        else
            Debug.LogError("Error submitting score: " + request.error);
    }

    [Serializable]
    private class ScoreData
    {
        public string playerID;
        public float bestTime;
        public string dateAchieved;
    }
}
