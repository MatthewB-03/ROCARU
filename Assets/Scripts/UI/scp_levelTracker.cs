using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Saves the player's level to the current scene
/// if it's higher than the previouly saved scene
/// </summary>
public class scp_levelTracker : MonoBehaviour
{
    public int currentLevel;
    // Start is called before the first frame update
    void Start()
    {
        currentLevel = PlayerPrefs.GetInt("CL");
        
        int index = SceneManager.GetActiveScene().buildIndex;
        if (currentLevel < index)
        {
            currentLevel = index;
            PlayerPrefs.SetInt("CL", currentLevel);
        }
      
    }
}
