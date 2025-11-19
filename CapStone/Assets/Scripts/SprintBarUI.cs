using UnityEngine;
using UnityEngine.UI;

public class SprintBarUI : MonoBehaviour
{
    public PlayerMovement player; // player movement script reference
    public RectTransform fillBar; // UI element for sprint bar fill

    // keeps the original scale of the of the fill bar, for consistancy
    private Vector3 originalScale;

    void Start()
    {
        // make sure fill bar exsist then store its scale
        if (fillBar != null)
            originalScale = fillBar.localScale;
    }

    void Update()
    {
        // make sure player and ui bar exsist then update the bar
        if (player != null && fillBar != null)
        {
            // makes the stamina bar horizontal and makes sure that the fill only works horizontal and the y & z scale stay the same
            float staminaPercent = player.currentStamina / player.maxStamina;
            fillBar.localScale = new Vector3(staminaPercent * originalScale.x, originalScale.y, originalScale.z);
        }
    }
}
