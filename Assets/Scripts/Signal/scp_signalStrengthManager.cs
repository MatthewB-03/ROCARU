using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of signal strength accross the map
/// </summary>
public class scp_signalStrengthManager : MonoBehaviour
{
    [SerializeField] scp_signalBooster[] boosters;

    private void Start()
    {
        boosters = GetComponentsInChildren<scp_signalBooster>();
    }

    /// <summary>
    /// Returns the best signal strength at a given position
    /// </summary>
    /// <param name="position">Position to use</param>
    /// <returns>Best signal strength at position</returns>
    public float CalculateStrength(Vector3 position)
    {
        float strength = 0;

        scp_signalBooster booster;

        float bStrength;

        // Find strongest booster
        for (int i = 0; i < boosters.Length; i++)
        {
            booster = boosters[i];

            if (booster.active)
            {
                bStrength = CalculateBoosterStength(position, booster);
                if (bStrength > strength)
                {
                    strength = bStrength;
                }
            }
        }

        // Only square root the return value
        return Mathf.Sqrt(strength);
    }

    //Returns distance^2 between positions A and B
    float CalculateDistance2(Vector3 A, Vector3 B)
    {
        Vector3 delta = A - B;
        return delta.x * delta.x + delta.y * delta.y + delta.z * delta.z;
    }

    //Returns the strength of the given booster at the given position 
    float CalculateBoosterStength(Vector3 position, scp_signalBooster booster)
    {
        float dist2 = booster.range*booster.range-CalculateDistance2(position, booster.transform.position);
        float strength = 0;
        if (dist2 > 0)
        {
            strength = dist2 * booster.strength;
        }
        return strength;
    }
}
