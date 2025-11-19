using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinMenuNickname : MonoBehaviour
{
    public TMP_InputField nicknameInput;

    public void OnSubmit()
    {
        string nickname = nicknameInput.text.ToUpper();
        if (nickname.Length != 3)
        {
            Debug.Log("Nickname must be 3 characters!");
            return;
        }

        Timer.Instance.SubmitBestTime(nickname);
        Debug.Log("Nickname submitted: " + nickname);
        // Optionally hide the input field after submission
        nicknameInput.gameObject.SetActive(false);
    }
}
