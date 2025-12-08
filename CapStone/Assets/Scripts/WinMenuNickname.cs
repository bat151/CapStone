using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinMenuNickname : MonoBehaviour
{
    // Input field where player will enter nickname
    public TMP_InputField nicknameInput;

    // When player clicks submit
    public void OnSubmit()
    {
        // Convert text to upper case
        string nickname = nicknameInput.text.ToUpper();
        if (nickname.Length != 3)
        {
            // Make sure nickname is 3 chars
            Debug.Log("Nickname must be 3 characters!");
            return;
        }

        // Submit the player time 
        Timer.Instance.SubmitBestTime(nickname);
        Debug.Log("Nickname submitted: " + nickname);

        // Hide the input field after submission
        nicknameInput.gameObject.SetActive(false);
    }
}
