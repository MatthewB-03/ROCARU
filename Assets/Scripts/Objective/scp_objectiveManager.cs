using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Displays the number of a specific objective
/// type left to complete.
/// </summary>
public class scp_objectiveManager : MonoBehaviour
{
    public int counter;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] string description;

    // Update is called once per frame
    void Update()
    {
        text.text = description + " (" + counter.ToString() + ")";
    }
}
