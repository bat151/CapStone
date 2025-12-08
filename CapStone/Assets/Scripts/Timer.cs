using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;

public class Timer : MonoBehaviour
{
    // Singelton to make sure any script can access the timer
    public static Timer Instance;

    // UI element, display minutes and seconds
    public TextMeshProUGUI timerText;

    // How long the timer has been running and check if it is running
    private float elapsedTime = 0f; 
    private bool isRunning = true; 

    // URL for the backend of my website where the best time and date will be sent
    private string apiUrlSubmit = "http://localhost:3000/submit";

    // Secure session API + storage
    private string apiStartSession = "http://localhost:3000/start-session"; 
    private string sessionId;    
    private string sessionToken; 

    private void Awake()
    {
        // Singleton pattern to make sure timer persist through scenes, used to help with win screen send data
        if (Instance == null)
        {
            // Make sure timer doesnt destroy through scenes
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If their is a timer in another scene delete it
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Ask server for a secure session + token
        StartCoroutine(StartSession());
    }

    private void Update()
    {
        // Only count time when main level is running
        if (!isRunning) return;

        // Up the time
        elapsedTime += Time.deltaTime;

        // Make sure time shows up as minutes and seconds
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        // Update the timer UI
        if (timerText != null)
            timerText.text = $"{minutes:00}:{seconds:00}";
    }

    // Stops the timer when player escapes but doesnt send JSON data yet
    public void StopTimerWithoutSending()
    {
        isRunning = false;
        Debug.Log("Timer stopped at: " + elapsedTime);
    }

    // Get the elasped time
    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    // Method to submit the time that the player had at the end of the level
    public void SubmitBestTime(string playerID)
    {
        // Now sends secure token instead of bestTime
        StartCoroutine(SendSecureSubmission(playerID));
    }

    // Request secure session from backend
    private IEnumerator StartSession()
    {
        UnityWebRequest req = UnityWebRequest.Get(apiStartSession);
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to start session: " + req.error);
        }
        else
        {
            Debug.Log("Session response: " + req.downloadHandler.text);

            SessionResponse resp = JsonUtility.FromJson<SessionResponse>(req.downloadHandler.text);

            sessionId = resp.sessionId;
            sessionToken = resp.token;

            Debug.Log("Stored secure token: " + sessionToken);
        }
    }

    // Submit secure token
    private IEnumerator SendSecureSubmission(string playerID)
    {
        var payload = new SecureSubmitData
        {
            playerID = playerID,
            sessionId = sessionId,
            token = sessionToken
        };

        string jsonData = JsonUtility.ToJson(payload);
        Debug.Log("Sending secure JSON: " + jsonData);

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

    // Secure token structures
    [Serializable]
    private class SessionResponse
    {
        public string sessionId;
        public string token;
        public long startTime;
    }

    [Serializable]
    private class SecureSubmitData
    {
        public string playerID;
        public string sessionId;
        public string token;
    }

}
