using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of whether an object needs to
/// be/has been repaired.
/// </summary>
public class scp_repairScript : MonoBehaviour
{
    public bool broken = false;
    public bool canBeBroken = true;

    [SerializeField] GameObject brokenSprite;
    [SerializeField] GameObject fixedSprite;

    [SerializeField] scp_objectiveManager objective;

    public repairTypes type;
    public enum repairTypes
    {
        objective,
        booster
    }

    [SerializeField] GameObject audioPrefab;
    [SerializeField] AudioClip breakSound;
    [SerializeField] float volume;

    void PlaySound()
    {
        if ((audioPrefab != null) & (breakSound != null))
        {
            AudioSource audio = Instantiate(audioPrefab, transform.position, Quaternion.identity).GetComponent<AudioSource>();
            audio.volume = volume;
            audio.clip = breakSound;
        }
    }

    // Called when the robot fixes it
    public IEnumerator Repair(float repairTime)
    {
        yield return new WaitForSeconds(repairTime);
        broken = false;
        brokenSprite.SetActive(false);
        fixedSprite.SetActive(true);
        if (type == repairTypes.booster)
        {
            GetComponent<scp_signalBooster>().active = true;

        }
        objective.counter -= 1;
    }
    // Called by something that breaks it
    public void Break()
    {
        if (canBeBroken)
        {
            PlaySound();
            broken = true;
            brokenSprite.SetActive(true);
            fixedSprite.SetActive(false);
            if (type == repairTypes.booster)
            {
                GetComponent<scp_signalBooster>().active = false;
            }
            objective.counter += 1;
        }
    }
    // Start called before first frame update
    private void Start()
    {
        if (!broken)
        {
            brokenSprite.SetActive(false);
        }
        else
        {
            fixedSprite.SetActive(false);
            objective.counter += 1;
        }
    }

    public void ShowFixedSprite()
    {
        fixedSprite.SetActive(true);
    }
}
