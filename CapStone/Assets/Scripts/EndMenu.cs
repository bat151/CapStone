using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class EndMenu : MonoBehaviour
{
    [Header("References")]
    public Button mainMenuButton;

    [Header("Scene Name")]
    public string mainMenuSceneName = "StartScene"; // Name of your start screen scene

    private void Start()
    {
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(() => SceneManager.LoadScene(mainMenuSceneName));
        else
            Debug.LogWarning("MainMenuButton not assigned in EndMenu!");
    }
}
