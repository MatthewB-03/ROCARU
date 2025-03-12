using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sets a broken parameter to the repair script boolean
/// </summary>
public class scp_repairAnimation : MonoBehaviour
{
    [SerializeField] scp_repairScript repair;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("broken", repair.broken);
    }
}
  