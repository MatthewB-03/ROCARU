using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Extends, rotates, and sparks the repair arm.
/// </summary>
public class scp_repairArm : MonoBehaviour
{
    [SerializeField] GameObject arm;
    [SerializeField] GameObject attachmentPoint;
    [SerializeField] GameObject sparkPoint;
    [SerializeField] GameObject sparkPrefab;
    Animator animator;

    Vector3 armStartRotation;
    Vector3 armTargetRotation;
    float armTime;
    float armLerpTime;

    float armExtension;
    float targetExtension;
    float startExtension;

    float extensionTime;
    float extensionLerpTime;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        armStartRotation = arm.transform.eulerAngles;
        armTargetRotation = arm.transform.eulerAngles;
        armLerpTime = 1;

        armExtension = 0;
        targetExtension = 0;
        startExtension = 0;
        extensionLerpTime = 1;
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent.position = attachmentPoint.transform.position;
        transform.parent.eulerAngles = arm.transform.eulerAngles;
        animator.SetFloat("extension", armExtension);
        LerpArmRotation();
        LerpExtension();
    }

    void LerpArmRotation()
    {
        armTime += Time.deltaTime;
        if (armTime > armLerpTime)
        {
            armTime = armLerpTime;
        }
        arm.transform.eulerAngles = Vector3.Lerp(armStartRotation, armTargetRotation, armTime / armLerpTime);
    }
    void LerpExtension()
    {
        extensionTime += Time.deltaTime;
        if (extensionTime > extensionLerpTime)
        {
            extensionTime = extensionLerpTime;
        }
        armExtension = Mathf.Lerp(startExtension, targetExtension, extensionTime / extensionLerpTime);
    }

    public void StartRotating(float rotation, float time)
    {
        armTime = 0;
        armLerpTime = time;
        armStartRotation = arm.transform.eulerAngles;
        // Prevent Long way round spinning
        float lowerStart = armStartRotation.y - 360;
        float higherStart = armStartRotation.y + 360;
        if ((higherStart - rotation) * (higherStart - rotation) < (armStartRotation.y - rotation) * (armStartRotation.y - rotation))
        {
            armStartRotation = new Vector3(armStartRotation.x, higherStart, armStartRotation.z);
        }
        else if ((lowerStart - rotation) * (lowerStart - rotation) < (armStartRotation.y - rotation) * (armStartRotation.y - rotation))
        {
            armStartRotation = new Vector3(armStartRotation.x, lowerStart, armStartRotation.z);
        }
        armTargetRotation = new Vector3(arm.transform.eulerAngles.x, rotation, arm.transform.eulerAngles.z);
    }


    public void StartExtending(float extension, float time)
    {
        extensionTime = 0;
        extensionLerpTime = time;
        startExtension = armExtension;
        targetExtension = extension;
    }

    public void Drop()
    {
        animator.SetBool("down", true);
    }

    public void Raise()
    {
        animator.SetBool("down", false);
    }

    public void Spark()
    {
        Instantiate(sparkPrefab, sparkPoint.transform.position, sparkPoint.transform.rotation);
    }
}
