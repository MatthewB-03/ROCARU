using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Allows the player to increase or decrease the time
/// scale.
/// </summary>
public class scp_timeScaleManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] AudioClip upSound;
    [SerializeField] AudioClip downSound;
    [SerializeField] AudioClip noSound;
    [SerializeField] AudioSource source;

    [SerializeField] float defaultTimeScale = 1;
    [SerializeField] float timeScaleStep = 0.25f;
    [SerializeField] float minTimeScale = 0;
    [SerializeField] float maxTimeScale = 4
        ;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = defaultTimeScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKeyDown(KeyCode.Underscore)) 
        {
            Time.timeScale -= timeScaleStep;
            if (Time.timeScale < minTimeScale)
            {
                Time.timeScale = minTimeScale;
                source.PlayOneShot(noSound);
            }
            else
            {
                source.PlayOneShot(downSound);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.Equals))
        {
            Time.timeScale += timeScaleStep;
            if (Time.timeScale > maxTimeScale)
            {
                Time.timeScale = maxTimeScale;
                source.PlayOneShot(noSound);
            }
            else
            {
                source.PlayOneShot(upSound);
            }
        }

        string txt = Time.timeScale.ToString();
        if (txt.Length == 1)
            txt += ".00";
        else if (txt.Length == 3)
            txt += "0";
        text.text = "PLAYBACK: - " + txt + "X +";
    }
}
