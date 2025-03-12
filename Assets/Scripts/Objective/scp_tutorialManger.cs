using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps tutorial text on screen until the player
/// has acted on it.
/// </summary>
public class scp_tutorialManger : MonoBehaviour
{
    [SerializeField] GameObject next;
    bool[] conditionMet;
    [SerializeField] tutorialConditions[] conditions;
    enum tutorialConditions
    {
        WASD,
        GoTo,
        Repair,
        Shoot,
        Execute,
        Mouse
    }

    // Start is called before the first frame update
    void Start()
    {
        conditionMet = new bool[conditions.Length];
        for (int i = 0; i < conditions.Length; i++)
        {
            conditionMet[i] = false;
        }
        StartCoroutine(CheckTimer());
    }

    // Update is called once per frame
    void Update()
    {
        MoveConditions();
    }
    void ClearCondition(tutorialConditions condition)
    {
        for (int i = 0; i<conditions.Length; i++)
        {
            if (conditions[i] == condition)
            {
                conditionMet[i] = true;
            }
        }
    }
    IEnumerator CheckTimer()
    {
        yield return new WaitForSeconds(1);
        if (CheckComplete())
        {
            Done();
        }
        else
        {
            StartCoroutine(CheckTimer());
        }
    }
    void Done()
    {
        this.gameObject.SetActive(false);
        next.SetActive(true);
        Destroy(this.gameObject);
    }

    bool CheckComplete()
    {
        bool complete = true;
        foreach (bool condition in conditionMet)
        {
            if (!condition)
            {
                complete = false;
            }
        }
        return complete;
    }
    void MoveConditions()
    {
        if (Input.GetKey(KeyCode.W) | Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.S) | Input.GetKey(KeyCode.D))
        {
            ClearCondition(tutorialConditions.WASD);
        }
        if (Input.GetKey(KeyCode.Alpha1))
        {
            ClearCondition(tutorialConditions.GoTo);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            ClearCondition(tutorialConditions.Repair);
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            ClearCondition(tutorialConditions.Shoot);
        }
        if ((Input.GetKey(KeyCode.Return) | Input.GetKey(KeyCode.KeypadEnter)))
        {
            ClearCondition(tutorialConditions.Execute);
        }
        if ((Input.GetAxis("Horizontal") != 0) | (Input.GetAxis("Vertical") != 0))
        {
            ClearCondition(tutorialConditions.Mouse);
        }
    }
}
