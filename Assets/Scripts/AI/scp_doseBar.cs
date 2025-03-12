using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Displays the enemy dosage in a progress bar
/// </summary>
public class scp_doseBar : MonoBehaviour
{
    [SerializeField] Animator anim;
    GameObject bar;
    scp_enemyScript enemy;

    float dose;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<scp_enemyScript>();
        bar = anim.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        string value = enemy.GetDoseValues();
        float percent;
        if (value != "asleep")
        {
            string[] values = value.Split(",");
            percent = float.Parse(values[0]) / float.Parse(values[1])*100;
        }
        else
        {
            percent = 100;
        }

        if ((percent <= 0) | (percent >= 100))
        {
            bar.SetActive(false);
        }
        else
        {
            bar.SetActive(true);
            anim.SetFloat("percent", percent);
        }
    }
}
