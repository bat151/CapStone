using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class EndMenu : MonoBehaviour
{
    // button to take player back to main menu
    public Button mainMenuButton;

    // name of the scene that will load
    public string mainMenuSceneName = "StartScene"; 

    private void Start()
    {
        // listner for the main menu button 
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(() => SceneManager.LoadScene(mainMenuSceneName));
    }
}
