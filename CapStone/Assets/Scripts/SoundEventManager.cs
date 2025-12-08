using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class SoundEventManager
{
    // Trigger event when an a sound is made
    // Enemy will get the players location and range/loudness
    public static event Action<Vector3, float> OnSoundMade;

    // Broadcast the position and range/loudness
    public static void BroadcastSound(Vector3 position, float loudness)
    {
        // Invoke the event only if their are subscribers
        OnSoundMade?.Invoke(position, loudness);
    }
}
