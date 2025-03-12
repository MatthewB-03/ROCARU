using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows the player to create an array of path &
/// command nodes.
/// </summary>
public class scp_holoPlanner : MonoBehaviour
{
    public GameObject[] path;
    public pathTags[] pathTag;

    public GameObject nodePrefab;
    public bool canShoot = true;

    [SerializeField] Transform holoTransform;

    [SerializeField] GameObject audioPrefab;
    [SerializeField] AudioClip yesSound;
    [SerializeField] AudioClip noSound;

    void PlaySound(AudioClip sound, float volume)
    {
        if ((audioPrefab != null) & (sound != null))
        {
            AudioSource audio = Instantiate(audioPrefab, transform.position, Quaternion.identity).GetComponent<AudioSource>();
            audio.volume = volume;
            audio.clip = sound;
        }
    }

    public enum pathTags
    {
        walkTo,
        repairAt,
        attackAt
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) | Input.GetKeyDown(KeyCode.Keypad1))
        {
            SetNodeInPath(pathTags.walkTo);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) | Input.GetKeyDown(KeyCode.Keypad2))
        {
            SetNodeInPath(pathTags.repairAt);
        }
        else if ((Input.GetKeyDown(KeyCode.Alpha3) | Input.GetKeyDown(KeyCode.Keypad3)) & canShoot)
        {
            SetNodeInPath(pathTags.attackAt);
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            UndoNode();
        }
    }

    //Set a node and action for the robot to pathfind to
    void SetNodeInPath(pathTags tag)
    {
        GameObject[] newPath = new GameObject[path.Length + 1];
        pathTags[] newPathTag = new pathTags[path.Length + 1];


        if (path.Length != 0)
        {
            for (int i = 0; i < path.Length; i++)
            {
                newPath[i] = path[i];
                newPathTag[i] = pathTag[i];
            }
        }

        newPath[path.Length] = Instantiate(nodePrefab, holoTransform.position, Quaternion.identity);
        newPathTag[path.Length] = tag;

        path = newPath;
        pathTag = newPathTag;
        PlaySound(yesSound, 0.5f);
    }

    //Remove the previous node from the path
    void UndoNode()
    {
        if (path.Length != 0)
        {
            GameObject[] newPath = new GameObject[path.Length - 1];
            pathTags[] newPathTag = new pathTags[path.Length - 1];

            for (int i = 0; i < newPath.Length; i++)
            {
                newPath[i] = path[i];
                newPathTag[i] = pathTag[i];
            }

            Destroy(path[path.Length-1]);

            path = newPath;
            pathTag = newPathTag;
        }
        PlaySound(noSound, 0.5f);
    }
}
