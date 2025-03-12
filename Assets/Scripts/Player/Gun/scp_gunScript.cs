using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Turns to face a target and animates firing
/// </summary>
public class scp_gunScript : MonoBehaviour
{
    public scp_armMover gunArm;
    [SerializeField] Animator animator;
    [SerializeField] GameObject attachmentPoint;
    [SerializeField] Vector3 startRotation;
    Vector3 targetRotation;
    float timeSinceStart;
    float timeToTarget;

    Vector3 defaultRotation;

    GameObject faceObject;

    private void Start()
    {
        defaultRotation = transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = attachmentPoint.transform.position;
        if (faceObject != null)
        {
            timeSinceStart += Time.deltaTime;
            targetRotation = new Vector3(startRotation.x, GetTargetRotation(faceObject.transform), startRotation.z);
            LerpRotation();
        }
        else
        {
            timeSinceStart += Time.deltaTime;
            targetRotation = new Vector3(startRotation.x, defaultRotation.y, startRotation.z);
            LerpLocal();
        }
    }

    void LerpRotation()
    {
        float fraction = timeSinceStart / timeToTarget;
        transform.eulerAngles = Vector3.Lerp(startRotation, targetRotation, fraction);

        if (fraction >= 1)
        {
            transform.eulerAngles = targetRotation;
        }
    }
    void LerpLocal()
    {
        float fraction = timeSinceStart / timeToTarget;
        transform.localEulerAngles = Vector3.Lerp(startRotation, targetRotation, fraction);

        if (fraction >= 1)
        {
            transform.localEulerAngles = targetRotation;
        }
    }

    public void TurnToFace(GameObject other, float time)
    {
        timeSinceStart = 0;
        timeToTarget = time;
        if (other != null)
        {
            faceObject = other;
            startRotation = transform.eulerAngles;
        }
        else
        {
            faceObject = null;
            startRotation = transform.localEulerAngles;
        }
    }

    float GetTargetRotation(Transform faceThis)
    {
        Vector3 delta = -(transform.position - faceThis.position).normalized;
        float angle = Mathf.Atan2(delta.x, delta.z) * 180 / Mathf.PI;

        // Prevent Long way round spinning
        float lowerStart = startRotation.y - 360;
        float higherStart = startRotation.y + 360;
        if ((higherStart - angle) * (higherStart - angle) < (startRotation.y - angle) * (startRotation.y - angle))
        {
            startRotation = new Vector3(startRotation.x, higherStart, startRotation.z);
        }
        else if ((lowerStart - angle) * (lowerStart - angle) < (startRotation.y - angle) * (startRotation.y - angle))
        {
            startRotation = new Vector3(startRotation.x, lowerStart, startRotation.z);
        }

        return Mathf.Atan2(delta.x, delta.z) * 180 / Mathf.PI;
    }

    // Animation
    public IEnumerator DeployAfterDelay(float time)
    {
         yield return new WaitForSeconds(time);
         animator.SetBool("deployed", true);
    }

    public void Retract(float time)
    {
        animator.SetBool("deployed", false);
        TurnToFace(null, time);
    }

    public void Fire()
    {
        animator.SetTrigger("fire");
    }
}
