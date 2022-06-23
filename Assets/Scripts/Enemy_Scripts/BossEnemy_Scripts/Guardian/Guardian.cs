using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Guardian : BossEnemy
{
    [SerializeField]
    private GuardianShield shield;
    private List<GuardianShield> shields = new();
    //public float Range;

    void Start()
    {
        StartBoss();
        InstantiateShield(new Vector2(transform.position.x, transform.position.y + 1), 0, true);
        InstantiateShield(new Vector2(transform.position.x, transform.position.y - 1), 0, true);
        InstantiateShield(new Vector2(transform.position.x + 1, transform.position.y), 90, false);
        InstantiateShield(new Vector2(transform.position.x - 1, transform.position.y), 90, false);
    }

    private void InstantiateShield(Vector2 position, float angelZ, bool status)
    {
        GameObject shieldGameobject = PhotonNetwork.InstantiateRoomObject(shield.name, position, Quaternion.Euler(0, 0, angelZ));  
        GuardianShield guardianShield = shieldGameobject.GetComponent<GuardianShield>();
        shields.Add(guardianShield);
        guardianShield.SetGuardian(this);
        guardianShield.gameObject.transform.SetParent(transform);
        guardianShield.SetActiveStatus(status); 
    }

    void Update()
    {
        UpdateEnemy();
        if (PhotonNetwork.IsMasterClient)
        {
            if (victim != null)
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

    public override void TakeDamage(float _damage, Player player)
    {
        Hp -= _damage;
        if (Hp > 0)
        {
            if (victim == null)
                victim = player;
            PV.RPC("SetHp", RpcTarget.AllBuffered, Hp);
            if(Hp <= MaxHp / 2)
            {
                SecondStage();
            }
        }
        else
        {
            player.SetPlusPrize(Prize, PrizeEX);
            DestroyHimself();
        }
    }
    public override void DestroyHimself()
    {
        //logic item drop

        PV.RPC("DestroyEnemy", RpcTarget.MasterClient);
    }
    public override void Attack()
    {
        victim.TakeDamage(Damage);
        SetTimeWithoutAttack(0);
    }

    private void SecondStage()
    {
        shields[2].SetActiveStatus(true);
        shields[3].SetActiveStatus(true);
    }
}
