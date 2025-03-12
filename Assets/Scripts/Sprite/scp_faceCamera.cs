using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes a 2d sprite face the camera at all times
/// </summary>
public class scp_faceCamera : MonoBehaviour
{
    [SerializeField] GameObject camera;
    float xAngle;
    float yAngle;
    float zAngle;

    void UpdateRotation()
    {
        Vector3 delta = camera.transform.position - transform.position;
        //xAngle = Mathf.Atan(delta.y / delta.z) * 180 / Mathf.PI;
        yAngle = 180+Mathf.Atan2(delta.x, delta.z) * 180 / Mathf.PI;
        //zAngle = Mathf.Atan(delta.x / delta.y) * 180 / Mathf.PI;

        transform.eulerAngles = new Vector3(xAngle, yAngle, zAngle);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRotation();
    }

    private void Start()
    {
        camera = GameObject.Find("MainCamera");
    }
}
