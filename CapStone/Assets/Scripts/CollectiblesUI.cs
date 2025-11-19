using UnityEngine;
using UnityEngine.UI;

public class CollectiblesUI : MonoBehaviour
{
    // referecne to gamemanager script, and collectibles UI element
    public GameManager gameManager; 
    public Text collectiblesText;

    private void Start()
    {
        // if game manager isnt assigned then use global instance
        if (gameManager == null)
            gameManager = GameManager.Instance;

        UpdateText();
    }

    // update the UI text for each collectible picked up
    public void UpdateText()
    {
        // make sure the references exsist before update
        if (gameManager == null || collectiblesText == null) 
            return;

        // update the collectibles while player is still picking them up
        if (gameManager.collectedCount < gameManager.totalCollectibles)
        {
            collectiblesText.text = $"Collect {gameManager.totalCollectibles - gameManager.collectedCount} objects";
        }
        else
        {
            // if no collectibles remain update the text to say go to escape
            collectiblesText.text = "Go to the Escape!";
        }
    }
}
