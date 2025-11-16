using UnityEngine;
using UnityEngine.UI;

public class CollectiblesUI : MonoBehaviour
{
    public GameManager gameManager; 
    public Text collectiblesText;

    private void Start()
    {
        if (gameManager == null)
            gameManager = GameManager.Instance;

        UpdateText();
    }

    public void UpdateText()
    {
        if (gameManager == null || collectiblesText == null) return;

        if (gameManager.collectedCount < gameManager.totalCollectibles)
        {
            collectiblesText.text = $"Collect {gameManager.totalCollectibles - gameManager.collectedCount} objects";
        }
        else
        {
            collectiblesText.text = "Go to the Escape!";
        }
    }
}
