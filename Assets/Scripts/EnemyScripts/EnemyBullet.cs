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

    public void SetEnemy(Enemy _enemy)
    {
        enemyDamage = _enemy.Damage;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Wall")
        {
            DestroyHimself();
        }
        if (coll.tag == "Player")
        {
            Player player = coll.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(enemyDamage);
                DestroyHimself();
            }
        }
    }
}
