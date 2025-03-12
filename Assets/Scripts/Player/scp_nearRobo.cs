using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of any objects nearby the robot 
/// </summary>
public class scp_nearRobo : MonoBehaviour
{
    [SerializeField] scp_robotDog script;

    private void OnTriggerEnter(Collider other)
    {
        script.AddToNeerby(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        script.RemoveFromNeerby(other.gameObject);
    }

}
