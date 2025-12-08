using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Name of the scene that will load
    public string gameSceneName = "Level";

    // Buttons for starting the game and closing the game
    public Button startButton; 
    public Button quitButton;  

    private void Start()
    {
        // Listener for the start button
        if (startButton != null)
            startButton.onClick.AddListener(StartGame);

        // Listner for the quit button
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
    }

    // Method to load the level when start is clciked
    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Close the game 
    public void QuitGame()
    {
        Debug.Log("Quit pressed — closing game.");
        Application.Quit();
    }
}
