using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates an array of malfunction nodes
/// the robot can pathfind to if the signal 
/// is too weak.
/// </summary>
public class scp_malfunctionManager : MonoBehaviour
{
    Transform[] nodes;

    // Start is called before the first frame update
    void Start()
    {
        nodes = GetComponentsInChildren<Transform>();

        // Parent transform has to be removed
        Transform[] newNodes = new Transform[nodes.Length - 1];
        int i = 0;
        foreach (Transform node in nodes)
        {
            if (node != transform)
            {
                newNodes[i] = node;
                i++;
            }
        }
        nodes = newNodes;
        Random.seed *= System.DateTime.Now.Millisecond;
    }

    // Random chance of a malfunction based on the signal strength
    public bool RandomMalfunction(float strength)
    {
        float shouldMalfunction = Random.Range(1, 100);
        if (shouldMalfunction > strength)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Returns the nodes in order of closest to furthest away
    Transform[] OrderNodesByDistance(Vector3 position)
    {
        Transform[] inOrder = new Transform[nodes.Length];
        int count = 0;
        foreach (Transform node in nodes)
        {
            int i = count;
            if (count != 0)
            {
                bool found = false;
                while (i > 0 & found == false)
                {
                    if (CalculateDistance2(inOrder[i - 1].position, position) > CalculateDistance2(node.position, position))
                    {
                        i--;
                    }
                    else
                    {
                        found = true;
                    }
                }
                for (int i2 = count; i2 > i; i2--)
                {
                    inOrder[i2] = inOrder[i2 - 1];
                }
            }
            inOrder[i] = node;
            count++;
        }
        return inOrder;
    }

    //Returns one of the closest nodes to the position
    public Transform PickNode(Vector3 position)
    {
        nodes = OrderNodesByDistance(position);
        return nodes[Random.Range(0, 3)];
    }

    //Returns distance^2 between positions A and B
    float CalculateDistance2(Vector3 A, Vector3 B)
    {
        Vector3 delta = A - B;
        return delta.x * delta.x + delta.y * delta.y + delta.z * delta.z;
    }
}
