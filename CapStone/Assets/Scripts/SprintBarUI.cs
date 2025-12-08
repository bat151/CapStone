using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SprintBarUI : MonoBehaviour
{
    // Reference to player movement, and UI for sprint bar
    public PlayerMovement player; 
    public RectTransform fillBar; 

    // Keeps the original scale of the of the fill bar, for consistancy
    private Vector3 originalScale;

    void Start()
    {
        // Make sure fill bar exsist then store its scale
        if (fillBar != null)
            originalScale = fillBar.localScale;
    }

    void Update()
    {
        // Make sure player and ui bar exsist then update the bar
        if (player != null && fillBar != null)
        {
            // Makes the stamina bar horizontal and makes sure that the fill only works horizontal and the y & z scale stay the same
            float staminaPercent = player.currentStamina / player.maxStamina;
            fillBar.localScale = new Vector3(staminaPercent * originalScale.x, originalScale.y, originalScale.z);
        }
    }
}
