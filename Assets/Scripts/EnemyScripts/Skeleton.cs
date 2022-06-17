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
            player.SetScore(player.Score + Prize);
            DestroyHimself();
        }
    }
}
