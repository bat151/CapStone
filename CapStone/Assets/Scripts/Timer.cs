using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class Timer : MonoBehaviour
{
    // UI Element to display the timer
    public TextMeshProUGUI timerText;

    // How much time has passed and flag for if timer is running
    private float elapsedTime = 0f;
    private bool isRunning = true;

    // Backend API URL for submitting scores
    private string apiUrlSubmit = "http://localhost:3000/submit";

    // Called once per frame
    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;

        // Calculate minutes and seconds from the elapsed time
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        // Update the text component
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    // Method to stop the timer
    public void StopTimer()
    {
        isRunning = false;

        float finalTime = GetElapsedTime();
        string playerID = "Player1"; // Replace with actual player ID if you have it

        // Send the score
        StartCoroutine(SendBestTime(playerID, finalTime));
    }

    // Method to start timer
    public void StartTimer()
    {
        isRunning = true;
        elapsedTime = 0f; // Reset timer when starting
    }

    // Method to retrieve elapsed time in seconds
    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    // Coroutine to send best time to backend
    private IEnumerator SendBestTime(string playerID, float bestTime)
    {
        string dateAchieved = DateTime.Now.ToString("yyyy-MM-dd");

        // Create JSON payload
        string jsonData = JsonUtility.ToJson(new ScoreData
        {
            playerID = playerID,
            bestTime = bestTime,
            dateAchieved = dateAchieved
        });

        // Debug to see what JSON is being sent
        Debug.Log("Sending JSON: " + jsonData);

        UnityWebRequest request = new UnityWebRequest(apiUrlSubmit, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)

        {
            Debug.Log("Score submitted successfully!");
        }
        else
        {
            Debug.LogError("Error submitting score: " + request.error);
        }
    }

    [Serializable]
    private class ScoreData
    {
        public string playerID;
        public float bestTime;
        public string dateAchieved;
    }
}
