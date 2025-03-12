using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Button methods that can save or load 
/// the player's game.
/// </summary>
public class scp_saveLoadButton : MonoBehaviour
{

    [SerializeField] scp_saveManager saveManager;
    
    public void Load()
    {
        saveManager.LoadScene();
    }

    public void Save()
    {
        saveManager.SaveGame();
    }
}
