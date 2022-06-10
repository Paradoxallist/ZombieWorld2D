using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyBullet : Bullet
{
    private Enemy enemy;

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
        enemy = _enemy;
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
            player.TakeDamage(enemy.Damage);
            DestroyHimself();
        }
    }
}
