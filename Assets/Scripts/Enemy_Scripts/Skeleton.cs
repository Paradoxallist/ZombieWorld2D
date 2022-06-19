using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Skeleton : Enemy
{
    public GameObject EnemyBulletOb;
    public float RangeBulletFlying;
    //public float MinRangeAttack;

    private float timeWithoutChangeRandomTarget;
    private GameObject RandomPosition;

    void Start()
    {
        StartEnemy();
        RandomPosition = new GameObject();
        RandomPosition.transform.SetParent(transform);
        timeWithoutChangeRandomTarget = 0;
    }

    void Update()
    {
        UpdateEnemy();
        if (PhotonNetwork.IsMasterClient)
        {
            if (victim != null && !GetVariableStun())
            {
                if (Vector2.Distance(victim.transform.position, transform.position) < AgrRange && Vector2.Distance(victim.transform.position, transform.position) > RangeAttack)
                {
                    SetTarget(victim.transform);
                }
                else// if(Vector2.Distance(victim.transform.position, transform.position) > MinRangeAttack)
                {
                    timeWithoutChangeRandomTarget += Time.deltaTime;
                    if (timeWithoutChangeRandomTarget > 1)
                        GoRandomPosition();
                }
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

    private void GoRandomPosition()
    {
        RandomPosition.transform.position = transform.position;
        RandomPosition.transform.position += new Vector3(Random.Range(0,5), Random.Range(0, 5), 0);
        timeWithoutChangeRandomTarget = 0;
        SetTarget(RandomPosition.transform);
    }

    public override void Attack()
    {
        GameObject bulletGameoject = PhotonNetwork.InstantiateRoomObject(EnemyBulletOb.name, transform.position, Quaternion.identity);
        EnemyBullet b = bulletGameoject.GetComponent<EnemyBullet>();
        b.SetRange(RangeBulletFlying);
        b.SetEnemy(this);
        b.SetTargetPositon(victim.transform.position);
        SetTimeWithoutAttack(0);
    }

    public override void TakeDamage(float _damage,Player player)
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
        Damage = Damage + (Wave - 1) * 1.5f;
        MaxHp = MaxHp + (Wave - 1) * 2f;
        PV.RPC("SynchronizingDataEnemy", RpcTarget.AllBuffered, MaxHp, Hp,Damage);
        //Speed = Speed + (Wave - 1) * 0.175f;
        //AttackSpeed = Mathf.Clamp(AttackSpeed - (Wave - 1) * 0.025f, 0.5f, 100f);
    }
}
