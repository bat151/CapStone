using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    //Make sure the flashlight is off
    public Light flashlight;
    private bool isOn = false;

    // audio for flashlight
    public AudioClip togglesound;
    private AudioSource audioSource;

    void Start()
    {
        // Get or add AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Start with light off
        flashlight.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // when Left mosue is clicked turn on and off flashlight
        if (Input.GetMouseButtonDown(0))
        {
            ToggleFlashlight();
        }
        
    }

    void ToggleFlashlight()
    {
        // Flip flashlight state
        isOn = !isOn;
        flashlight.enabled = isOn;

        // Play toggle sound
        if (togglesound != null)
        {
            audioSource.PlayOneShot(togglesound);
        }
    }
}
