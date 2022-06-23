using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SniperBullet : Bullet
{
    private Player player;

    void Start()
    {
        StartBullet();
    }

    void Update()
    {
        UpdateBullet();
    }

    public void SetPlayer(Player _player)
    {
        player = _player;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Wall")
        {
            DestroyHimself();

        }
        if (coll.tag == "Enemy")
        {
                Enemy enemy = coll.GetComponent<Enemy>();
                if (player != null)
                {
                    enemy.TakeDamage(player.GetPlayerStat(StatType.Damage).Value, player);
                }
                DestroyHimself(); 
        }
    }
}
