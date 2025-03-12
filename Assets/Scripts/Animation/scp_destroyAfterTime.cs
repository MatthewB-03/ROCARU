using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Waits for a delay then destroys the gameObject
/// </summary>
public class scp_destroyAfterTime : MonoBehaviour
{
    [SerializeField] float time;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitThenDestroy());
    }

    IEnumerator WaitThenDestroy()
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
