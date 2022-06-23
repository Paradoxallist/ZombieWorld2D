using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Sniper : Player
{
    public SniperBullet BulletObject;

    public float RangeAttack;

    public int CountBullet;
    public float angel;
    public float attackSpeedBoost;
    

    private bool abilityTwoActive;

    void Start()
    {
        abilityTwoActive = false;
        StartPlayer();
    }

    void Update()
    {
        UpdatePlayer();
        if (photonView.IsMine)
        {
            if (!abilityTwoActive)
            {
                if (Input.GetMouseButton(0) && GetTimeWitoutAttack() > GetPlayerStat(StatType.AttackSpeed).Value)
                {
                    SetTimeWitoutAttack(0);
                    Attack();
                }
            }
            else 
            {
                if (Input.GetMouseButton(0) && GetTimeWitoutAttack() > GetPlayerStat(StatType.AttackSpeed).Value / attackSpeedBoost)
                {
                    SetTimeWitoutAttack(0);
                    Attack();
                }

                Mana -= GetPlayerStat(StatType.CostAbilityTwo).Value * Time.deltaTime;
                photonView.RPC("SetManaPun", RpcTarget.AllBuffered, Mana);
                if (Mana < 0)
                {
                    UseAbilityTwo();
                }
            }
        }
    }

    public override void Attack()
    {
        Vector2 bulletFuturePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        InstantiateBullet(bulletFuturePosition);
    }

    public override void TakeDamage(float _damage)
    {
        Hp -= _damage;
        if (Hp < 0)
        {
            abilityTwoActive = false;
        }
        CheckAlive();
        photonView.RPC("SetHpPun", RpcTarget.AllBuffered, Hp);
    }

    public override void UseAbilityOne()
    {
        if (Mana > GetPlayerStat(StatType.CostAbilityOne).Value)
        {
            Mana -= GetPlayerStat(StatType.CostAbilityOne).Value;
            photonView.RPC("SetManaPun", RpcTarget.AllBuffered, Mana);
            Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float startAngel = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            for (float i = - ((float)CountBullet - 1) /2; i <= ((float)CountBullet - 1) / 2; i++)
            {
                Vector2 target = new Vector2(Mathf.Cos((Mathf.PI / 180) * ((angel / CountBullet) * i + startAngel)), Mathf.Sin((Mathf.PI / 180) * ((angel / CountBullet) * i + startAngel))) + (Vector2)transform.position;
                InstantiateBullet(target);
            }
        }
    }

    private void InstantiateBullet(Vector2 targetPosition)
    {
        GameObject bulletGameoject = PhotonNetwork.Instantiate(BulletObject.name, transform.position, Quaternion.identity);
        SniperBullet b = bulletGameoject.GetComponent<SniperBullet>();
        b.SetRange(RangeAttack);
        b.SetPlayer(this);
        b.SetTargetPositon(targetPosition);
    }

    public override void UseAbilityTwo()
    {
        if (abilityTwoActive)
        {
            abilityTwoActive = false;
            //PlayerSpriteBody.color = Color.white;
            photonView.RPC("SetColor", RpcTarget.AllBuffered, 1f, 1f, 1f);
        }
        else if (Mana > GetPlayerStat(StatType.CostAbilityTwo).Value)
        {
            abilityTwoActive = true;
            //PlayerSpriteBody.color = new Color(1f, 0.8f, 0f);
            photonView.RPC("SetColor", RpcTarget.AllBuffered, 1f, 0.8f, 0f);
        }
    }

    /*public override void Die()
    {
        photonView.gameObject.SetActive(false);
        photonView.RPC("SetActivePun", RpcTarget.AllBuffered, false);
    }

    public override void Resurrection()
    {
        photonView.gameObject.SetActive(true);
        Hp = MaxHp;
        Mana = MaxMana;
        abilityTwoActive = false;
    }*/



    public override void LevelUpStat(StatType statType)
    {
        base.LevelUpStat(statType);
    }
}
