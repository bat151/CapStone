using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class SoundEventManager
{
    // trigger event when an a sound is made
    // enemy will get the players location and range/loudness
    public static event Action<Vector3, float> OnSoundMade;

    // broadcast the position and range/loudness
    public static void BroadcastSound(Vector3 position, float loudness)
    {
        // invoke the event only if their are subscribers
        OnSoundMade?.Invoke(position, loudness);
    }
}
