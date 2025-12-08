using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class EndMenu : MonoBehaviour
{
    // Button to take player back to main menu
    public Button mainMenuButton;

    // Name of the scene that will load
    public string mainMenuSceneName = "StartScene"; 

    private void Start()
    {
        // Listner for the main menu button 
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(() => SceneManager.LoadScene(mainMenuSceneName));
    }
}
