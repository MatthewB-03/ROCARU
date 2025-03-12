using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Exits the game when the escape key is pressed.
/// </summary>
public class scp_exitGame : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Exit();
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
