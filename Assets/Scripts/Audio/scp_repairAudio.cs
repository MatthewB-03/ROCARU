using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Changes audio source volume when the object 
/// is repaired/damaged.
/// </summary>
public class scp_repairAudio : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] scp_repairScript repair;
    [SerializeField] float volumeChange;
    [SerializeField] float minVolume;
    [SerializeField] float maxVolume;


    // Update is called once per frame
    void Update()
    {
        int direction = 1;
        if (repair.broken)
        {
            direction = -1;
        }

        float volume = source.volume;
        volume += volumeChange*direction*Time.deltaTime;
        if (volume > maxVolume)
        {
            volume = maxVolume;
        }
        if (volume < minVolume)
        {
            volume = minVolume;
        }
        source.volume = volume;
    }
}
