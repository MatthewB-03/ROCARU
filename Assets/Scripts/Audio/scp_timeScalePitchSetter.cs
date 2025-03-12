using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sets the audio pitch to the current time scale
/// </summary>
public class scp_timeScalePitchSetter : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    float defaultPitch;
    float previousTimeScale;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        defaultPitch = audioSource.pitch;
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource.pitch != defaultPitch*previousTimeScale)
            defaultPitch = audioSource.pitch;

        audioSource.pitch = defaultPitch*Time.timeScale;
        previousTimeScale = Time.timeScale;
    }
}
