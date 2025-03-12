using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Projectile that adds dosage to a hit enemy 
/// </summary>
public class scp_projectile : MonoBehaviour
{
    public float projectileDose;

    private void OnCollisionEnter(Collision collision)
    {
        scp_enemyScript enemy = collision.collider.gameObject.GetComponent<scp_enemyScript>();

        if (enemy != null)
        {
            enemy.HitByProjectile(projectileDose);
        }
        Destroy(this.gameObject);
    }
}
