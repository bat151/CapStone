using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class Timer : MonoBehaviour
{
    // Singelton to make sure any script can access the timer
    public static Timer Instance;

    // UI element, display minutes and seconds
    public TextMeshProUGUI timerText;

    private float elapsedTime = 0f; // how long the timer has been running
    private bool isRunning = true; // is the timer running

    // URL for the backend of my website where the best time and date will be sent
    private string apiUrlSubmit = "https://localhost:3000/submit";

    private void Awake()
    {
        // Singleton pattern to make sure timer persist through scenes, used to help with win screen send data
        if (Instance == null)
        {
            // make sure timer doesnt destroy through scenes
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // if their is a timer in another scene delete it
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // only count time when main level is running
        if (!isRunning) return;

        // up the time
        elapsedTime += Time.deltaTime;

        // make sure time shows up as minutes and seconds
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        // update the timer UI
        if (timerText != null)
            timerText.text = $"{minutes:00}:{seconds:00}";
    }

    // stops the timer when player escapes but doesnt send JSON data yet
    public void StopTimerWithoutSending()
    {
        isRunning = false;
        Debug.Log("Timer stopped at: " + elapsedTime);
    }

    // get the elasped time
    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    // method to submit the time that the player had at the end of the level
    public void SubmitBestTime(string playerID)
    {
        float finalTime = GetElapsedTime();
        StartCoroutine(SendBestTime(playerID, finalTime));
    }

    // coroutine to send both the final timer and date to backend API
    private IEnumerator SendBestTime(string playerID, float bestTime)
    {
        string dateAchieved = DateTime.Now.ToString("yyyy-MM-dd");

        // convert the data to JSON
        string jsonData = JsonUtility.ToJson(new ScoreData
        {
            playerID = playerID,
            bestTime = bestTime,
            dateAchieved = dateAchieved
        });

        Debug.Log("Sending JSON: " + jsonData);

        // build the POST request
        UnityWebRequest request = new UnityWebRequest(apiUrlSubmit, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        // let the backend know JSON data is being sent
        request.SetRequestHeader("Content-Type", "application/json");

        // send the request 
        yield return request.SendWebRequest();

        // debug to make sure that data was sent succsefully
        if (request.result == UnityWebRequest.Result.Success)
            Debug.Log("Score submitted successfully!");
        else
            Debug.LogError("Error submitting score: " + request.error);
    }

    // structure of the JSON being sent
    [Serializable]
    private class ScoreData
    {
        public string playerID;
        public float bestTime;
        public string dateAchieved;
    }
}
