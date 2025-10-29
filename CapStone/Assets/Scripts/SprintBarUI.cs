using UnityEngine;
using UnityEngine.UI;

public class SprintBarUI : MonoBehaviour
{
    public PlayerMovement player;
    public RectTransform fillBar; // Assign SprintBarFill RectTransform

    private Vector3 originalScale;

    void Start()
    {
        if (fillBar != null)
            originalScale = fillBar.localScale;
    }

    void Update()
    {
        if (player != null && fillBar != null)
        {
            float staminaPercent = player.currentStamina / player.maxStamina;
            fillBar.localScale = new Vector3(staminaPercent * originalScale.x, originalScale.y, originalScale.z);
        }
    }
}
