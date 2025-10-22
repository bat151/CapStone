using UnityEngine;

public class AudioTest : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip testClip;

    void Start()
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource not assigned!");
            return;
        }
        if (testClip == null)
        {
            Debug.LogError("Test clip not assigned!");
            return;
        }

        audioSource.clip = testClip;
        audioSource.loop = true;
        audioSource.volume = 1f;
        audioSource.Play();

        Debug.Log("AudioTest started playing!");
    }
}
