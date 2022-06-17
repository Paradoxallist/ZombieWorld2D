using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Zombie : Enemy
{
    void Start()
    {
        StartEnemy();
    }

    void Update()
    {
        UpdateEnemy();
        if (PhotonNetwork.IsMasterClient)
        {
            if (victim != null && !GetVariableStun())
            {
                SetTarget(victim.transform);
                if (AttackSpeed < GetTimeWithoutAttack())
                {
                    if (Vector2.Distance(victim.transform.position, transform.position) < RangeAttack)
                    {
                        Attack();
                    }
                }
            }
        }
    }

    public override void Attack()
    {
        victim.TakeDamage(Damage);
        SetTimeWithoutAttack(0);
    }

    public override void TakeDamage(float _damage, Player player)
    {
        Hp -= _damage;
        if (Hp > 0)
        {
            if (victim == null)
                victim = player;
            PV.RPC("SetHp", RpcTarget.AllBuffered, Hp);
        }
        else
        {
            player.SetScore(player.Score + Prize);
            DestroyHimself();
        }
    }
    public void SetDamage()
    {

    }

}