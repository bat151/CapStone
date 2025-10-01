using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    //Make sure the flashlight is off
    public Light flashlight;
    private bool On = false;

    // Start is called before the first frame update
    void Start()
    {
        
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
        if(flashlight != null)
        {
            // change flashlight to on and off
            On = !On;

            // enable the actual light component
            flashlight.enabled = On;
        }
    }
}
