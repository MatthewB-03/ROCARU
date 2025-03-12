using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StarterAssets;

/// <summary>
/// Main robot dog script that controls and 
/// animates the robot
/// </summary>
public class scp_robotDog : MonoBehaviour
{
    // Set in inspector
    [SerializeField] NavMeshAgent agent;
    [SerializeField] scp_holoPlanner holoScript;
    StarterAssetsInputs holoInput;
    StarterAssetsInputs dogInput;

    [Header("Dogs")]
    public GameObject dog;
    public Animator dogAnim;
    [SerializeField] GameObject holo;
    public GameObject holoDog;

    [Header("Cameras")]
    [SerializeField] GameObject dogCam;
    [SerializeField] GameObject holoCam;

    [SerializeField] scp_progressBar progressBar;
    [SerializeField] GameObject gameOverObject;

    [Header("Gun")]
    [SerializeField] scp_gunScript gun;
    [SerializeField] Transform gunTransform;
    [SerializeField] GameObject gunProjectile;
    public float gunForce;
    [SerializeField] Transform gunScanTransform;
    [SerializeField] LayerMask gunLayerMask;

    [Header("Repair")]
    [SerializeField] scp_repairArm armRepairScript;
    [SerializeField] scp_armMover armScript;

    [Header("Neerby")]
    [SerializeField] CapsuleCollider neerbyCollider;
    float neerRadius = 4;
    float searchRadius = 20;
    public GameObject[] neerbyObjects;

    // Pathing
    [Header("Pathing")]
    [SerializeField] GameObject[] path;
    [SerializeField] scp_holoPlanner.pathTags[] pathTag;

    int pathIndex;
    Vector3 targetPos;
    float targetStopDistance = 0.6f;

    // Signal
    [Header("Signal")]
    [SerializeField] GameObject malfunctionSprite;
    scp_malfunctionManager malfunctionManager;
    scp_signalStrengthManager signalStrengthManager;
    float signalStrength;
    Animator signalUI;
    Animator signalStatic;

    // Signal
    [Header("Audio")]
    [SerializeField] GameObject audioPrefab2D;
    [SerializeField] GameObject audioPrefab3D;
    [SerializeField] AudioClip[] gunSounds;
    [SerializeField] float volume;
    [SerializeField] AudioClip executeSound;
    [SerializeField] AudioClip malfunctionSound;
    [SerializeField] AudioClip haltSound;
    [SerializeField] AudioClip noHaltSound;

    // Other
    dogs activeDog = dogs.holo;
    enum dogs
    {
        holo,
        robo
    }
    states currentState;
    enum states
    {
        planning,
        moving,
        executing,
        gameOver
    }

    // Timing
    float repairTime = 8;
    float searchTime = 4;
    float aimTime = 1;

    bool canHalt = true;
    float haltCooldown = 5;


    // Default Methods
    private void Start()
    {
        currentState = states.planning;
        malfunctionSprite.SetActive(false);
        signalStrengthManager = GameObject.Find("BoosterManager").GetComponent<scp_signalStrengthManager>();
        signalUI = GameObject.Find("SignalStengthIndicator").GetComponent<Animator>();
        signalStatic = GameObject.Find("CamStatic").GetComponent<Animator>();
        malfunctionManager = GameObject.Find("MalfunctionManager").GetComponent < scp_malfunctionManager>();
        neerbyObjects = new GameObject[0];
        neerbyCollider.radius = neerRadius;
        holoInput = holoDog.GetComponent<StarterAssetsInputs>();
        dogInput = dog.GetComponent<StarterAssetsInputs>();
        dogInput.enabled = false;
        canHalt = true;
        Random.seed *= System.DateTime.Now.Millisecond;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != states.moving)
        {
            agent.speed = 0;
        }
        else
        {
            agent.speed = 3.5f;

        }
        if (currentState != states.planning)
        {
            if (GetDistance2(targetPos) <= targetStopDistance * targetStopDistance & currentState == states.moving)
            {
                if (malfunctionManager.RandomMalfunction(signalStrength))
                {
                    MalfunctionNode();
                }
                else
                {
                    CheckNodeType();
                }
            }
            if ((Input.GetKeyDown(KeyCode.Return) | Input.GetKeyDown(KeyCode.KeypadEnter)) & canHalt)
            {
                if (!(malfunctionManager.RandomMalfunction(signalStrength) | malfunctionManager.RandomMalfunction(signalStrength)))
                {
                    StartCoroutine(PlaySound(dog.transform, audioPrefab2D, haltSound, 0.7f, 0));
                    Halt();
                }
                else
                {
                    StartCoroutine(HaltMalfunction());
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return) | Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Execute();
        }
    }
    IEnumerator HaltMalfunction()
    {
        StartCoroutine(PlaySound(dog.transform, audioPrefab2D, noHaltSound, 0.7f, 0));
        canHalt = false;
        yield return new WaitForSeconds(haltCooldown);
        canHalt = true;

    }
    //Returns distance^2 to input position
    float GetDistance2(Vector3 position)
    {
        Vector3 delta = dog.transform.position - position;

        float d2 = delta.x * delta.x + delta.y * delta.y + delta.z * delta.z;

        return d2;
    }

    // - PATHING -

    //Start following the planned path
    void Execute()
    {
        StartCoroutine(PlaySound(dog.transform, audioPrefab2D, executeSound, 0.7f, 0));

        pathIndex = -1;
        path = holoScript.path;
        pathTag = holoScript.pathTag;

        holo.SetActive(false);
        dog.SetActive(true);

        holoCam.SetActive(false);
        dogCam.SetActive(true);

        holoInput.enabled = false;
        dogInput.enabled = true;

        activeDog = dogs.robo;

        NextNodeInPath();
    }
    // Stop the dog and program a new path
    void Halt()
    {

        dogAnim.SetBool("isMoving", false);

        currentState = states.planning;

        holoDog.transform.position = new Vector3(dog.transform.position.x, holoDog.transform.position.y, dog.transform.position.z);
        holoDog.transform.rotation = dog.transform.rotation;

        holoCam.transform.position = dogCam.transform.position;
        holoCam.transform.rotation = dogCam.transform.rotation;

        dog.SetActive(false);
        holo.SetActive(true);

        dogCam.SetActive(false);
        holoCam.SetActive(true);

        dogInput.enabled = false;
        holoInput.enabled = true;

        foreach (GameObject node in holoScript.path)
        {
            Destroy(node);
        }

        holoScript.path = new GameObject[0];
        holoScript.pathTag = new scp_holoPlanner.pathTags[0];

        activeDog = dogs.holo;
    }

    // Performs the associated action with the path node
    void CheckNodeType()
    {
        if (path.Length != 0 & pathIndex < path.Length)
        {
            currentState = states.executing;

            switch (pathTag[pathIndex])
            {

                case scp_holoPlanner.pathTags.walkTo:
                    NextNodeInPath();
                    break;

                case scp_holoPlanner.pathTags.repairAt:
                    AttemptRepair();
                    break;

                case scp_holoPlanner.pathTags.attackAt:
                    StartCoroutine(SearchForEnemy());
                    break;
            }
        } 
        else
        {
            Halt();
        }
    }

    // Start moving to the next node
    void NextNodeInPath()
    {
        if (path.Length != 0 & pathIndex+1 < path.Length)
        {
            malfunctionSprite.SetActive(false);
            pathIndex++;
            targetPos = path[pathIndex].transform.position;
            if (GetDistance2(targetPos) <= targetStopDistance * targetStopDistance) // Dont move if already at desired location
            {
                if (malfunctionManager.RandomMalfunction(signalStrength))
                {
                    MalfunctionNode();
                }
                else
                {
                    CheckNodeType();
                }
            }
            else
            {
                agent.SetDestination(targetPos);
                currentState = states.moving;
            }
        }
        else
        {
            Halt();
        }
    }

    // Start moving to the next node after a delay
    IEnumerator WaitThenNextNode(float time)
    {
        yield return new WaitForSeconds(time);
        NextNodeInPath();
    }


    // - SIGNAL -

    // Paths to a random malfunction node
    void MalfunctionNode()
    {
        StartCoroutine(PlaySound(dog.transform, audioPrefab2D, malfunctionSound, 0.7f, 0));
        malfunctionSprite.SetActive(true);
        if (path.Length != 0)
        {
            pathIndex++;
        }
        Transform target = malfunctionManager.PickNode(dog.transform.position);
        targetPos = target.position;
        agent.SetDestination(targetPos);
        currentState = states.moving;
    }
    // Sets the indicator to the current signal strength
    void UpdateSignalStrength()
    {
        if (activeDog == dogs.robo)
        {
            signalStrength = signalStrengthManager.CalculateStrength(dog.transform.position);
            signalStatic.SetFloat("strength", signalStrength);
        }
        else
        {
            signalStrength = signalStrengthManager.CalculateStrength(holoDog.transform.position);
            signalStatic.SetFloat("strength", 100);
        }
        signalUI.SetFloat("strength", signalStrength);
        //Debug.Log(signalStrength);
    }

    //Checks if the signal is lost
    void CheckSignalStrength()
    {
        if (signalStrength <= 0 & currentState != states.planning)
        {
            GameOver();
        }
    }

    //Starts the game over screen
    void GameOver()
    {
        gameOverObject.SetActive(true);
        currentState = states.gameOver;
    }

    // - AUDIO - 

    IEnumerator PlaySound(Transform audioTransform, GameObject audioPrefab, AudioClip sound, float volume, float delay)
    {
        yield return new WaitForSeconds(delay);
        AudioSource audio = Instantiate(audioPrefab, audioTransform.position, Quaternion.identity).GetComponent<AudioSource>();
        audio.volume = volume;
        audio.clip = sound;
    }

    // - NEERBY -

    //Returns true if other is in the neerby array
    public bool IsNeerby(GameObject other)
    {
        bool isThere = false;
        foreach (GameObject neer in neerbyObjects)
        {
            if (other == neer)
            {
                isThere = true;
            }
        }

        return isThere;
    }
    // Other is now neerby
    public void AddToNeerby(GameObject other)
    {
        //Debug.Log("add"+other.name);
        if (other != null)
        {
            if (!IsNeerby(other))
            {
                GameObject[] newArray = new GameObject[neerbyObjects.Length + 1];
                int i = 0;
                while (i < neerbyObjects.Length)
                {
                    newArray[i] = neerbyObjects[i];
                    i++;
                }
                newArray[i] = other;
                neerbyObjects = newArray;

            }
        }
    }
    // Other is no longer neerby
    public void RemoveFromNeerby(GameObject other)
    {
        if (neerbyObjects.Length > 1) // Ensure array is longer than 1 item
        {
            GameObject[] newArray = new GameObject[neerbyObjects.Length - 1];
            int i = 0;
            bool removed = false;
            while (i + 1 < neerbyObjects.Length)
            {
                if (removed)
                {
                    newArray[i] = neerbyObjects[i + 1];
                }
                else if (neerbyObjects[i] != other)
                {
                    newArray[i] = neerbyObjects[i];
                }
                else
                {
                    newArray[i] = neerbyObjects[i + 1];
                    removed = true;
                }
                i++;
            }
            neerbyObjects = newArray;
        }
        else if (neerbyObjects.Length == 1) // If exactly one item, just make an empty array.
        {
            neerbyObjects = new GameObject[0];
        }
    }

    // - REPAIR -

    // If any neerby objects are repairable, repair the best one.
    void AttemptRepair()
    {
        GameObject repair = GetBestRepair();

        if (repair != this.gameObject)
        {
            StartCoroutine(repair.GetComponent<scp_repairScript>().Repair(repairTime));
            StartCoroutine(RepairAnimations(repair));
            progressBar.gameObject.SetActive(true);
            progressBar.Begin(repairTime, scp_progressBar.textTypes.repair);
            StartCoroutine(WaitThenNextNode(repairTime));
        }
        else
        {
            NextNodeInPath();
        }
        
    }

    // Handles the timing and lerping of various elements of the repair animation
    IEnumerator RepairAnimations(GameObject repair)
    {
        armScript.LerpToExtension(1, repairTime * 0.1f);
        yield return new WaitForSeconds(repairTime * 0.1f);

        armRepairScript.StartRotating(GetRepairAngle(repair), repairTime * 0.1f);
        armRepairScript.Drop();
        yield return new WaitForSeconds(repairTime * 0.1f);

        armRepairScript.StartExtending(1, repairTime * 0.1f);
        yield return new WaitForSeconds(repairTime * 0.1f);

        //Sparks
        yield return new WaitForSeconds(repairTime * 0.05f);
        armRepairScript.Spark();
        yield return new WaitForSeconds(repairTime * 0.15f);
        armRepairScript.Spark();
        yield return new WaitForSeconds(repairTime * 0.15f);
        armRepairScript.Spark();
        yield return new WaitForSeconds(repairTime * 0.05f);
        //EndSparks

        armRepairScript.StartExtending(0, repairTime * 0.1f);
        yield return new WaitForSeconds(repairTime * 0.1f);

        armRepairScript.Raise();
        armRepairScript.StartRotating(dog.transform.eulerAngles.y, repairTime * 0.1f);
        yield return new WaitForSeconds(repairTime * 0.1f);

        armScript.LerpToExtension(0, repairTime * 0.1f);


    }

    float GetRepairAngle(GameObject repair)
    {
        Vector3 delta = -(armRepairScript.transform.position - repair.transform.position);
        return Mathf.Atan2(delta.x, delta.z) * 180 / Mathf.PI;
    }

    // Evaluate the best neerby object for repair
    GameObject GetBestRepair()
    {
        float bestScore = 0;
        GameObject bestRepair = this.gameObject;

        for (int i = 0; i<neerbyObjects.Length; i++)
        {
            float repairScore;
            if (neerbyObjects[i] != null)
            {
                scp_repairScript repairScript = neerbyObjects[i].GetComponent<scp_repairScript>();
                Vector3 delta = neerbyObjects[i].transform.position - dog.transform.position;

                if (repairScript == null)
                {
                    repairScore = -1;
                }
                else if (!repairScript.broken)
                {
                    repairScore = -1;
                }
                else if (delta.magnitude > neerRadius)
                {
                    repairScore = -1;
                }
                else
                {
                    repairScore = 1;

                    switch (repairScript.type)
                    {
                        case scp_repairScript.repairTypes.booster:
                            repairScore = 1;
                            break;
                        case scp_repairScript.repairTypes.objective:
                            repairScore = 2;
                            break;
                    }
                    repairScore *= GetDistance2(repairScript.transform.position);
                }
                if (repairScore >= bestScore)
                {
                    bestRepair = neerbyObjects[i];
                    bestScore = repairScore;
                }
            }
        }
        return bestRepair;
    } 

    // - ATTACK - 
    
    // Identifies and attacks all enemies currently in range
    IEnumerator SearchForEnemy()
    {
        neerbyCollider.radius = searchRadius;

        progressBar.gameObject.SetActive(true);
        progressBar.Begin(searchTime, scp_progressBar.textTypes.scan);

        gun.gunArm.LerpToExtension(1, searchTime);
        StartCoroutine(gun.DeployAfterDelay(searchTime*0.8f));

        StartCoroutine(PlaySound(gun.transform, audioPrefab3D, gunSounds[0], 0.7f, searchTime * 0.8f));//Deploy sound
        StartCoroutine(PlaySound(gun.transform, audioPrefab3D, gunSounds[Random.Range(1, 3)], 0.4f, searchTime * 0.8f + 0.2f));//Draw sound

        yield return new WaitForSeconds(searchTime);

        GameObject[] enemies = GetNeerEnemies();

        if (enemies.Length != 0)
        {
            StartCoroutine(AttackEnemies(enemies));
        }
        else
        {
            NextNodeInPath();
        }
    }

    // Returns all enemies currently in range
    GameObject[] GetNeerEnemies()
    {
        GameObject[] enemies = new GameObject[0];

        foreach (GameObject other in neerbyObjects)
        {
            if (other != null)
            {
                if (other.GetComponent<scp_enemyScript>() != null)
                {
                    GameObject[] newEnemies = new GameObject[enemies.Length + 1];
                    for (int i = 0; i < enemies.Length; i++)
                    {
                        newEnemies[i] = enemies[i];
                    }
                    enemies = newEnemies;
                    enemies[enemies.Length - 1] = other;
                }
            }
            else
            {
                RemoveFromNeerby(other);
            }
        }

        return enemies;
    }

    // Shoots at each of the enemies
    IEnumerator AttackEnemies(GameObject[] enemies)
    {
        foreach(GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                if (CheckLineOfSight(enemy))
                {
                    gun.TurnToFace(enemy, aimTime);
                    yield return new WaitForSeconds(aimTime);
                    gun.Fire();

                    StartCoroutine(PlaySound(gun.transform, audioPrefab3D, gunSounds[3], 1, 0)); //Shoot sound
                    StartCoroutine(PlaySound(gun.transform, audioPrefab3D, gunSounds[Random.Range(1, 3)], 0.4f, 0.1f)); //Draw sound

                    GameObject projectile = Instantiate(gunProjectile, gunTransform.position, gunTransform.rotation);
                    projectile.GetComponent<Rigidbody>().AddForce(gunForce * gunTransform.forward.normalized);

                    yield return new WaitForSeconds(aimTime*1.5f);
                }
            }
        }
        neerbyCollider.radius = neerRadius;
        gun.Retract(aimTime);
        gun.gunArm.LerpToExtension(0, searchTime);
        NextNodeInPath();
    }

    //Returns true if there is a line of sight
    bool CheckLineOfSight(GameObject other)
    {
        bool returnBool = false;

        RaycastHit hit;
        Vector3 direction = -(gunScanTransform.position - other.transform.position);
        Debug.DrawRay(gunScanTransform.position, direction, Color.red, 10);
        if (Physics.Raycast(gunScanTransform.position, direction, out hit, searchRadius, gunLayerMask, QueryTriggerInteraction.Ignore))
        {
            //Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject == other)
            {
                returnBool = true;
            }
        }
        return returnBool;
    }

    // - PHYSICS UPDATES -

    private void FixedUpdate()
    {
        UpdateSignalStrength();
        CheckSignalStrength();
    }
}
