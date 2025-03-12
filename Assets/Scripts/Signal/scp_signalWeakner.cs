using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Weakens the signal strength of nearby boosters 
/// </summary>
public class scp_signalWeakner : MonoBehaviour
{
    CapsuleCollider collider;
    [SerializeField] float signalMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<CapsuleCollider>();
    }


    private void OnTriggerStay(Collider other)
    {
        scp_signalBooster booster = other.GetComponent<scp_signalBooster>();
        if (booster != null)
        {
            WeakenSignal(booster);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        scp_signalBooster booster = other.GetComponent<scp_signalBooster>();
        if (booster != null)
        {
            booster.AddWeakener(gameObject.name, 1);
        }
    }

    // Adds a weakener to the booster based on distance
    private void WeakenSignal(scp_signalBooster booster)
    {
        Vector3 delta = transform.position - booster.transform.position;

        float distance = Mathf.Sqrt(delta.x * delta.x + delta.y * delta.y + delta.z * delta.z);

        float distanceMultiplier = (collider.radius - distance) / collider.radius;
        if (distanceMultiplier < 0)
        {
            distanceMultiplier *= -1;
        }

        float strength;

        if (distanceMultiplier <= 0)
        {
            booster.AddWeakener(gameObject.name, signalMultiplier);
        }
        else if (distanceMultiplier < 1)
        {
            booster.AddWeakener(gameObject.name, signalMultiplier / distanceMultiplier);
        }
        else
        {
            booster.AddWeakener(gameObject.name, 1);
        }
    }
}
