using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Waits until an audio clip has been assigned,
/// then plays it, and destroys the game object
/// </summary>
public class scp_playAudioOnce : MonoBehaviour
{
    [SerializeField] AudioSource audio;
    [SerializeField] bool realTime;

    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(PlayThenDestroy());
    }
    IEnumerator PlayThenDestroy()
    {
        if (audio.clip != null)
        {
            audio.Play();
            if (realTime)
                yield return new WaitForSecondsRealtime(audio.clip.length);
            else
                yield return new WaitForSeconds(audio.clip.length);
            Destroy(gameObject);
        }
        else
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(PlayThenDestroy());
        }
    }
}
