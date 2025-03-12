using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lerps an arm animation parameter to extend it
/// </summary>
public class scp_armMover : MonoBehaviour
{
    float startExtension = 0;
    float targetExtension = 0;
    float extension = 0;
    float timePassed = 0;
    float lerpTime = 1;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > lerpTime)
        {
            timePassed = lerpTime;
        }
        if (lerpTime != 0)
        {
            extension = Mathf.Lerp(startExtension, targetExtension, timePassed / lerpTime);
        }

        animator.SetFloat("extension", extension);
    }

    public void LerpToExtension(float toExtension, float time)
    {
        startExtension = extension;
        targetExtension = toExtension;

        timePassed = 0;
        lerpTime = time;
    }

}
