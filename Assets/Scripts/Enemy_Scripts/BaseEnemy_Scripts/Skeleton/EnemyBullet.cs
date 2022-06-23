using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyBullet : Bullet
{
    private float enemyDamage;

    void Start()
    {
        StartBullet();
    }

    void Update()
    {
        UpdateBullet();
    }

    public void SetBulletDamage(float Damage)
    {
        enemyDamage = Damage;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Wall"))
        {
            DestroyHimself();
        }
        if (coll.CompareTag("Player") && !coll.isTrigger)
        {
            Player player = coll.GetComponentInParent<Player>();
            if (player != null)
            {
                player.TakeDamage(enemyDamage);
                DestroyHimself();
            }
        }
    }
}
