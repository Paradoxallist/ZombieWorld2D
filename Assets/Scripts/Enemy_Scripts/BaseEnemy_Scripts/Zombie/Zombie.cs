using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Zombie : BaseEnemy
{
    void Start()
    {
        StartEnemy();
    }

    void Update()
    {
        UpdateBaseEnemy();
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
            player.SetPlusPrize(Prize, PrizeEX);
            DestroyHimself();
        }
    }
    public override void LevelUpWave(int Wave)
    {
        /*Damage = Mathf.Round(Mathf.Pow(Wave, 1 / 4f) * Damage);
        MaxHp = Mathf.Round(Mathf.Pow(Wave, 1 / 4f) * MaxHp);
        Speed = Mathf.Round(Mathf.Pow(Wave, 1 / 4f) * Speed * 100) * 0.01f;
        AttackSpeed = Mathf.Round(AttackSpeed / Mathf.Pow(Wave, 1 / 6f) * 1000)* 0.001f;*/
        Damage = Damage + (Wave - 1) * 2f;
        MaxHp = MaxHp + (Wave - 1) * 4f;
        Hp = MaxHp;
        PV.RPC("SynchronizingDataEnemy", RpcTarget.AllBuffered, MaxHp, Hp, Damage);
        //Speed = Speed + (Wave - 1) * 0.175f;
        //AttackSpeed = Mathf.Clamp(AttackSpeed - (Wave - 1) * 0.025f, 0.5f, 100f);
    }
}
