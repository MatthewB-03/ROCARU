using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores a booster's signal strength and range
/// </summary>
public class scp_signalBooster : MonoBehaviour
{
    public float strength;
    public float range;
    public bool active;

    string[] weakNames;
    float[] weakeners;

    // Necessary for signal weakener to function 
    float originalStrength;

    // Start is called before the first frame update
    private void Start()
    {
        originalStrength = strength;
        weakNames = new string[0];
        weakeners = new float[0];
    }

    // Fixed update is called every physics update
    private void FixedUpdate()
    {
        // Apply lowest weakener to signal strength
        float lowest = 1;
        foreach (float weakener in weakeners)
        {
            if (weakener < lowest)
            {
                lowest = weakener;
            }
        }
        strength = lowest * originalStrength;
    }

    /// <summary>
    /// Adds a signal weakener to the booster, or updates it if it's already added
    /// </summary>
    /// <param name="name">The name of the weakener</param>
    /// <param name="weakener">The value of the weakener</param>
    public void AddWeakener(string name, float weakener)
    {
        if (weakNames.Length > 0)
        {
            // Update weakener value if it exists
            bool found = false;
            for (int i = 0; i < weakNames.Length; i++)
            {
                if (weakNames[i] == name)
                {
                    weakeners[i] = weakener;
                    found = true;
                }
            }

            // Add new weakener if it does not exist
            if (found == false)
            {
                string[] newNames = new string[weakNames.Length + 1];
                float[] newWeak = new float[newNames.Length];
                for (int i = 0; i < weakNames.Length; i++)
                {
                    newNames[i] = weakNames[i];
                    newWeak[i] = weakeners[i];
                }
                newNames[newNames.Length-1] = name;
                newWeak[newNames.Length-1] = weakener;

                weakNames = newNames;
                weakeners = newWeak;
            }
        }
        else // If there are no weakeners, make this the first weakener
        {
            weakNames = new string[1];
            weakeners = new float[1];

            weakNames[0] = name;
            weakeners[0] = weakener;
        }
    }
}
