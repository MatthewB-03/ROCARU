using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plays the audio source when a public method is called
/// </summary>
public class scp_audioOnEvent : MonoBehaviour
{
    [SerializeField] AudioSource audio;
    public void PlayAudio()
    {
        audio.Play();
    }
}
