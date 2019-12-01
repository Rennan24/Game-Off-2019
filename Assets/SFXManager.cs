using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviourSingleton<SFXManager>
{
    [SerializeField]
    private AudioSource audioSource;

    public void PlayAtPoint(Vector3 position, SimpleAudioEvent audioEvent)
    {
        audioEvent.PlayAtPoint(audioSource, position);
    }
}
