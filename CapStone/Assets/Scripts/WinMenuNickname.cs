using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinMenuNickname : MonoBehaviour
{
    // input field where player will enter nickname
    public TMP_InputField nicknameInput;

    // when player clicks submit
    public void OnSubmit()
    {
        // convert text to upper case
        string nickname = nicknameInput.text.ToUpper();
        if (nickname.Length != 3)
        {
            // make sure nickname is 3 chars
            Debug.Log("Nickname must be 3 characters!");
            return;
        }

        // submit the player time 
        Timer.Instance.SubmitBestTime(nickname);
        Debug.Log("Nickname submitted: " + nickname);

        // hide the input field after submission
        nicknameInput.gameObject.SetActive(false);
    }
}
