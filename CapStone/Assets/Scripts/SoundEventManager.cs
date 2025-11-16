using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class SoundEventManager
{
    // Trigger when an a sound is made
    public static event Action<Vector3, float> OnSoundMade;

    // Broadcast the Position and Range/Volume
    public static void BroadcastSound(Vector3 position, float loudness)
    {
        OnSoundMade?.Invoke(position, loudness);
    }
}
