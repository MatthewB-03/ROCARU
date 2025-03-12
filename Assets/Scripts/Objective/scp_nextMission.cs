using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Starts fading into the next level if the player
/// completes all objectives and enters the trigger.
/// </summary>
public class scp_nextMission : MonoBehaviour
{

    [SerializeField] scp_objectiveManager[] objectives;
    [SerializeField] GameObject playerCollider;
    [SerializeField] scp_fadeOut fadeOut;


    bool CheckObjectives()
    {
        bool completed = true;

        foreach (scp_objectiveManager objective in objectives)
        {
            if (objective.counter > 0)
            {
                completed = false;
            }
        }

        return completed;
    }

    void LoadNextIfCompleted()
    {
        if (CheckObjectives())
        {
            fadeOut.SetScene(SceneManager.GetActiveScene().buildIndex + 1);
            fadeOut.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerCollider)
        {
            LoadNextIfCompleted();
        }
    }
}
