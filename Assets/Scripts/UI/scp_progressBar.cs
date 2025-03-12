using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves a progress bar from 0 to 100 percent
/// complete over a period of time.
/// </summary>
public class scp_progressBar : MonoBehaviour
{
    Animator anim;
    float timeSinceStart;
    float timeWhenDone = 0;

    textTypes activeText;
    public enum textTypes
    {
        repair,
        scan
    }
    [SerializeField] GameObject repairText;
    [SerializeField] GameObject scanText;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeWhenDone > 0 & timeSinceStart < timeWhenDone)
        {
            timeSinceStart += Time.deltaTime;
            anim.SetFloat("percent", PercentDone());
        }
        else if (timeSinceStart >= timeWhenDone)
        {
            Finish();
        }
    }

    // Returns the time passed as a percentage of the total time to fill the bar.
    float PercentDone()
    {
        return timeSinceStart / timeWhenDone * 100;
    }

    //Begins the progress bar (Game object should be set as active first)
    public void Begin(float time, textTypes text)
    {
        timeSinceStart = 0;
        timeWhenDone = time;
        activeText = text;
        switch (activeText)
        {
            case textTypes.repair:
                repairText.SetActive(true);
                scanText.SetActive(false);
                break;
            case textTypes.scan:
                scanText.SetActive(true);
                repairText.SetActive(false);
                break;
        }
    }

    // Called when the progress bar is done
    void Finish()
    {
        timeWhenDone = 0;
        timeSinceStart = 0;

        switch (activeText)
        {
            case textTypes.repair:
                repairText.SetActive(false);
                break;
            case textTypes.scan:
                scanText.SetActive(false);
                break;
        }

        this.gameObject.SetActive(false);
    }
}
