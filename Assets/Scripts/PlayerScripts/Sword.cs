using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public Player player;
    public float KickbackForce;


    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Enemy")
        {
            Enemy enemy = coll.GetComponent<Enemy>();
            /*Vector3 dir = (enemy.transform.position - transform.position).normalized * KickbackForce;
            enemy.transform.position += dir;*/
            enemy.TakeDamage(player.Damage, player);
        }
    }
}
