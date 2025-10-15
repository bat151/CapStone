using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    //UI Element to display the timer
    public TextMeshProUGUI timerText;

    //How much time has passed and flag for if timer is running
    private float elapsedTime = 0f;
    private bool isRunning = true;

    // Called once per frame
    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;

        //Calculate minutes and seconds from the elapsed time
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        //Update the text component
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    //Method to stop the timer
    public void StopTimer()
    {
        isRunning = false;
    }

    //Method to start timer
    public void StartTimer()
    {
        isRunning = true;
    }

    //Method to retrieve elapsed time in seconds
    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}
