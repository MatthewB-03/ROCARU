using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Sets the object as inactive unless the player
/// level is greater than a minimum.
/// </summary>
public class scp_levelDependent : MonoBehaviour
{
    [SerializeField] int minimum;

    private void Start()
    {
        if (PlayerPrefs.GetInt("CL") < minimum)
        {
            gameObject.SetActive(false);
        }
    }
}
