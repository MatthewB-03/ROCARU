using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// UI button that can activate & deactivate
/// objects, as well as load scenes.
/// </summary>
public class scp_button : MonoBehaviour
{

    [SerializeField] buttonTypes buttonType;
    enum buttonTypes
    {
        activate,
        load,
        reload,
        fadeLoad
    }

    [SerializeField] int sceneIndex;
    [SerializeField] GameObject[] activateThese;
    [SerializeField] GameObject[] deactivateThese;
    [SerializeField] scp_fadeOut fadeOut;

    [SerializeField] GameObject audioPrefab;
    [SerializeField] AudioClip sound;
    [SerializeField] float volume;

    void PlaySound()
    {
        if ((audioPrefab != null) & (sound != null))
        {
            AudioSource audio = Instantiate(audioPrefab, transform.position, Quaternion.identity).GetComponent<AudioSource>();
            audio.volume = volume;
            audio.clip = sound;
        }
    }

    public void WhenClicked() // Reference in button component
    {
        PlaySound();
        switch (buttonType)
        {
            case buttonTypes.activate:
                ActivateOnClick();
                break;
            case buttonTypes.load:
                LoadOnClick();
                break;
            case buttonTypes.reload:
                ReLoadOnClick();
                break;
            case buttonTypes.fadeLoad:
                FadeOnClick();
                break;
        }
    }

    void ActivateOnClick()
    {
        if (activateThese.Length > 0)
        {
            SetAllActive(activateThese, true);
        }
        if (deactivateThese.Length > 0)
        {
            SetAllActive(deactivateThese, false);
        }
    }

    void SetAllActive(GameObject[] setThese, bool active)
    {
        foreach (GameObject obj in setThese)
        {
            obj.SetActive(active);
        }
    }

    void LoadOnClick()
    {
        SceneManager.LoadScene(sceneIndex);
    }

    void ReLoadOnClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    void FadeOnClick()
    {
        fadeOut.SetScene(sceneIndex);
        fadeOut.gameObject.SetActive(true);
    }
}
