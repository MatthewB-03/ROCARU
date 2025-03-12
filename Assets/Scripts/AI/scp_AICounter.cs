using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates an array of every enemy child object
/// </summary>
public class scp_AICounter : MonoBehaviour
{
    public GameObject[] enemies;
    scp_enemyScript[] enemyScripts;

    // Start is called before the first frame update
    void Awake()
    {
        enemyScripts = GetComponentsInChildren<scp_enemyScript>();
        enemies = new GameObject[enemyScripts.Length];
        for (int i=0; i<enemyScripts.Length; i++)
        {
            enemies[i] = enemyScripts[i].gameObject;
        }
    }
}
