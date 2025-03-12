using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Faces the agent direction so that root motion
/// moves on the correct path
/// </summary>
public class scp_moveMain : MonoBehaviour
{
    Vector3 stayHere;
    [SerializeField] GameObject robo;
    Animator anim;
    UnityEngine.AI.NavMeshAgent agent;

    float targetAngle;
    float startAngle;
    float timeTurning;
    float totalTurnTime = 0.2f;

    public bool isRobo = false;
    Vector3 previousPos;

    [SerializeField] bool flipDirection = false;

    // Start is called before the first frame update
    void Start()
    {
        stayHere = transform.localPosition;
        previousPos = robo.transform.position;
        anim = robo.GetComponent<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        targetAngle = robo.transform.eulerAngles.y;
        startAngle = robo.transform.eulerAngles.y;
        timeTurning = totalTurnTime;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRobo) // Make sure the robot is facing direction the agent is trying to pathfind in
        {
            
            if (agent.speed > 0) // Make sure agent is moving
            {
                timeTurning += Time.deltaTime;
                if (timeTurning > 2 * totalTurnTime) // Short delay after lerping finishes, so it doesn't change it's mind on repeat
                {
                    if (!flipDirection)
                    {
                        targetAngle = 180 + Mathf.Atan2(agent.desiredVelocity.normalized.x, agent.desiredVelocity.normalized.z) * 180 / Mathf.PI;
                    }
                    else
                    {
                        targetAngle = Mathf.Atan2(agent.desiredVelocity.normalized.x, agent.desiredVelocity.normalized.z) * 180 / Mathf.PI;
                    }

                    if ((targetAngle- robo.transform.eulerAngles.y > 10) | (targetAngle - robo.transform.eulerAngles.y < -10)) // Make sure new angle is different enough to justify turning.
                    {
                        startAngle = robo.transform.eulerAngles.y;

                        // Prevent Long way round spinning
                        float lowerStart = startAngle - 360;
                        float higherStart = startAngle + 360;
                        if ((higherStart - targetAngle)*(higherStart - targetAngle) < (startAngle - targetAngle) * (startAngle - targetAngle))
                        {
                            startAngle = higherStart;
                        }
                        else if ((lowerStart - targetAngle) * (lowerStart - targetAngle) < (startAngle - targetAngle) * (startAngle - targetAngle))
                        {
                            startAngle = lowerStart;
                        }

                        timeTurning = 0;
                        //agent.speed = 0;
                        //Debug.Log(startAngle.ToString()+" - "+ targetAngle.ToString());
                    }
                }
                else if (timeTurning < totalTurnTime) // Lerp to new rotation
                {

                    //agent.speed = 0; 
                    robo.transform.eulerAngles = Vector3.Lerp(new Vector3(robo.transform.eulerAngles.x, startAngle, robo.transform.eulerAngles.z), new Vector3(robo.transform.eulerAngles.x, targetAngle, robo.transform.eulerAngles.z), timeTurning / totalTurnTime);

                }
            }

            //Animation
            if (agent.speed > 0)
            {
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }

            transform.localPosition = stayHere;
        }
    }
}
