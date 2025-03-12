using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plays random sounds at random times
/// </summary>
public class scp_randomAudio : MonoBehaviour
{
    [SerializeField] GameObject audioPrefab;
    [SerializeField] AudioClip[] sounds;
    [SerializeField] Vector2 timeRange;
    [SerializeField] Vector2 pitchRange;
    [SerializeField] float volume;
    [SerializeField] bool repeat;

    void PlaySound(AudioClip sound)
    {
        if ((audioPrefab != null) & (sound != null))
        {
            AudioSource audio = Instantiate(audioPrefab, transform.position, Quaternion.identity).GetComponent<AudioSource>();
            audio.volume = volume;
            audio.clip = sound;
            audio.pitch = Random.Range(pitchRange.x, pitchRange.y);
        }
    }
    IEnumerator WaitThenPlaySound()
    {
        yield return new WaitForSeconds(Random.Range(timeRange.x, timeRange.y));
        PlaySound(sounds[Random.Range(0, sounds.Length)]);

        if (repeat)
        {
            StartCoroutine(WaitThenPlaySound());
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(WaitThenPlaySound());
        Random.seed *= System.DateTime.Now.Millisecond;
    }

}
