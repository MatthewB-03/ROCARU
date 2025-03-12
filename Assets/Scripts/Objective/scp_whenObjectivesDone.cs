using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activates and deactivates objects when all objectives
/// are finished.
/// </summary>
public class scp_whenObjectivesDone : MonoBehaviour
{

    [SerializeField] scp_objectiveManager[] objectives;
    [SerializeField] GameObject activate;
    [SerializeField] GameObject[] deactivate;
    bool done;


    // Update is called once per frame
    void Update()
    {
        done = true;
        foreach (scp_objectiveManager objective in objectives)
        {
            if (objective.counter > 0)
            {
                done = false;
            }
        }

        if (done)
        {
            activate.SetActive(true);
            foreach (GameObject obj in deactivate)
            {
                obj.SetActive(false);
            }
        }
    }
}
