using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Animates text by cycling through an array of
/// text frames.
/// </summary>
public class scp_textAnimator : MonoBehaviour
{
    [TextArea(15, 20)] [SerializeField] string[] textArray;
    [SerializeField] float changeDelay;
    [SerializeField] bool loops;

    int textIndex = 0;

    TextMeshProUGUI text;


    [SerializeField] GameObject audioPrefab;
    [SerializeField] AudioClip updateSound;
    [SerializeField] float volume;

    [SerializeField] bool realTime;


    // Start is called before the first frame update
    void OnEnable()
    {
        text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(UpdateTextAndWait());
    }

    IEnumerator UpdateTextAndWait()
    {
        text.text = textArray[textIndex];
        PlaySound();

        if (realTime)
            yield return new WaitForSecondsRealtime(changeDelay);
        else

            yield return new WaitForSeconds(changeDelay);

        textIndex++;
        if (textIndex < textArray.Length)
        {
            StartCoroutine(UpdateTextAndWait());
        }
        else if (loops)
        {
            textIndex = 0;
            StartCoroutine(UpdateTextAndWait());
        }
    }

    void PlaySound()
    {
        if ((audioPrefab != null) & (updateSound != null))
        {
            if (textIndex != 0)
            {
                if (textArray[textIndex] != textArray[textIndex - 1]) 
                { 
                    AudioSource audio = Instantiate(audioPrefab, transform.position, Quaternion.identity).GetComponent<AudioSource>();
                    audio.volume = volume;
                    audio.clip = updateSound;
                }
            }
        }
    }
}
