using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sets an isMoving animation parameter to true if
/// the gameOnject has moved between frames.
/// </summary>
public class scp_amIMoving : MonoBehaviour
{
    Animator anim;
    Vector3 previousPosition;

    // Start is called before the first frame update
    void Start() 
    {
        anim = GetComponent<Animator>();
        previousPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 delta = previousPosition - transform.position;

        if (delta.x * delta.x + delta.y * delta.y + delta.z * delta.z >= 0.005*Time.deltaTime)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }

        previousPosition = transform.position;
    }
}
