using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Enemy AI that moves around randomly and can be incapacitated
/// </summary>
public class scp_enemyScript : MonoBehaviour
{
    [Header("Dosage")]
    public float doseRequired;
    float doseAdministered;
    public float doseDrain;

    GameObject pathToThis;

    NavMeshAgent agent;

    [Header("Scripts")]
    [SerializeField] scp_objectiveManager objective;
    [SerializeField] scp_malfunctionManager nodeScript;

    [Header("RandomChances")]
    public Vector2 pathTimeRange;
    public int breaksPercent;

    [Header("Animation")]
    [SerializeField] Animator anim;

    [Header("Sound")]

    [SerializeField] GameObject audioPrefab;
    [SerializeField] AudioClip[] idleSounds;
    [SerializeField] AudioClip[] hurtSounds;
    [SerializeField] AudioClip lickSound;
    [SerializeField] AudioClip dropSound;



    states state;
    enum states
    {
        idle,
        roaming,
        incapacitated
    }

    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        objective.counter += 1;
        state = states.idle;
        StartCoroutine(ActAfterDelay(0.1f));
        Random.seed *= System.DateTime.Now.Millisecond;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(state);
        if (state != states.incapacitated)
        {
            DrainDose();
            if (ReachedDestination())
            {
                state = states.idle;
            }
            if (state == states.roaming)
            {
                agent.speed = 3.5f;
            }
            else
            {
                agent.speed = 0;
            }
        }
    }

    void PlaySound(AudioClip sound, float volume)
    {
        if ((audioPrefab != null) & (sound != null))
        {
            AudioSource audio = Instantiate(audioPrefab, transform.position, Quaternion.identity).GetComponent<AudioSource>();
            audio.volume = volume;
            audio.clip = sound;
        }
    }

    // - ACTION -

    void Act()
    {
        if (state != states.incapacitated)
        {
            PickRandomAction();
        }
    }
    IEnumerator ActAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);
        Act();
    }

    bool ReachedDestination()
    {
        if (pathToThis != null)
        {
            Vector3 delta = pathToThis.transform.position - transform.position;
            delta.x *= delta.x;
            delta.y *= delta.y;
            delta.z *= delta.z;
            if (delta.x + delta.y + delta.z <= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    void PickRandomAction()
    {
        int rNumber = Random.Range(0, 3);
        if (rNumber == 0)
        {
            PathToObject();
        }
        else if (rNumber == 1)
        {
            state = states.idle;
            StartCoroutine(ActAfterDelay(Random.Range(pathTimeRange.x, pathTimeRange.y) / 10));
            anim.SetTrigger("lick");
            PlaySound(lickSound, 0.6f);
        }
        else
        {
            state = states.idle;
            StartCoroutine(ActAfterDelay(Random.Range(pathTimeRange.x, pathTimeRange.y)/10));
            PlaySound(idleSounds[Random.Range(0, idleSounds.Length)], 0.6f);
        }
    }

    void PathToObject()
    {
        pathToThis = nodeScript.PickNode(transform.position).gameObject;
        agent.SetDestination(pathToThis.transform.position);
        state = states.roaming;
        StartCoroutine(ActAfterDelay(Random.Range(pathTimeRange.x, pathTimeRange.y)));
    }

    // - DOSAGE - 
    void DrainDose()
    {
        doseAdministered -= doseDrain * Time.deltaTime;
        if (doseAdministered < 0)
        {
            doseAdministered = 0;
        }
    }

    public void HitByProjectile(float dose)
    {
        //Debug.Log("ouch");
        doseAdministered += dose;
        anim.SetTrigger("hit");
        PlaySound(hurtSounds[Random.Range(0, hurtSounds.Length)], 0.6f);
        if (doseAdministered >= doseRequired)
        {
            Incapacitate();
            StartCoroutine(DropSound());
        }
        else
        {
            PathToObject();
        }
    }

    void Incapacitate()
    {
        if (state != states.incapacitated)
        {
            anim.SetBool("incapacitated", true);
            objective.counter -= 1;
            state = states.incapacitated;
        }
        //Debug.Log("die");
        agent.speed = 0;

        BoxCollider bCollider = GetComponent<BoxCollider>();

        bCollider.size = new Vector3(1.5f, 0.5f, 1);
    }
    IEnumerator DropSound()
    {
        yield return new WaitForSeconds(2.6f);
        PlaySound(dropSound, 0.7f);
    }

    public string GetDoseValues()
    {
        if (state == states.incapacitated)
        {
            return "asleep";
        }
        {
            return doseAdministered.ToString() + "," + doseRequired.ToString();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (state != states.incapacitated)
        {
            scp_repairScript repairScript = other.gameObject.GetComponent<scp_repairScript>();

            if (repairScript != null)
            {
                int randomNumber = Random.Range(0, 100);
                if (randomNumber <= breaksPercent & !repairScript.broken)
                {
                    repairScript.Break();
                }
            }
        }
    }

    public string GetSaveValues() // Called when saving
    {
        string text = "";
        text = text + "," + anim.transform.position.x.ToString();
        text = text + "," + anim.transform.position.y.ToString();
        text = text + "," + anim.transform.position.z.ToString();
        text = text + "," + anim.transform.eulerAngles.x.ToString();
        text = text + "," + anim.transform.eulerAngles.y.ToString();
        text = text + "," + anim.transform.eulerAngles.z.ToString();
        text = text + "," + doseAdministered.ToString();
        switch (state)
        {
            case states.idle:
                text = text + ",i";
                break;
            case states.incapacitated:
                text = text + ",s";
                break;
            case states.roaming:
                text = text + ",r";
                break;
        }
        if (pathToThis != null)
        {
            text = text + "," + pathToThis.name;
        }
        else
        {
            text = text + ",null";
        }
        return text;
    }

    public void SetSaveValues(string currentDose, string currentState, string pathTarget, Vector3 parentPosition, Vector3 parentRotation) // Called when loading
    {
        anim.transform.position = parentPosition;
        anim.transform.eulerAngles = parentRotation;
        //Debug.Log(currentDose);
        doseAdministered = float.Parse(currentDose);
        switch (currentState)
        {
            case "i":
                state = states.idle;
                break;
            case "s":
                anim.SetBool("startsIncapacitated", true);
                Incapacitate();
                break;
            case "r":
                state = states.roaming;
                break;
        }
        if (pathTarget != "null")
        {
            pathToThis = GameObject.Find(pathTarget);
            agent.SetDestination(pathToThis.transform.position);
        }
        agent.enabled = false;
        agent.enabled = true;
    }
}
