using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Sniper : Player
{
    public GameObject BulletOb;
    //private Vector3 bulletFuturePosition;

    public float RangeAttack;

    public int CountBullet;
    public float angel;

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
            if (Input.GetMouseButton(0) && GetTimeWitoutAttack() > AttackSpeed)
            {
                SetTimeWitoutAttack(0);
                /*bulletFuturePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                bulletFuturePosition.z = transform.position.z;*/
                Attack();
            }
            if (abilityTwoActive)
            {
                Mana -= CostAbilityTwo * Time.deltaTime;
                photonView.RPC("SetManaPun", RpcTarget.AllBuffered, Mana);
                if (Mana < 0)
                {
                    AbilityTwo();
                }
            }
        }
    }

    public override void Attack()
    {
        Vector3 bulletFuturePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bulletFuturePosition.z = transform.position.z;
        GameObject bulletGameoject = PhotonNetwork.Instantiate(BulletOb.name, transform.position, Quaternion.identity);
        SniperBullet b = bulletGameoject.GetComponent<SniperBullet>();
        b.SetRange(RangeAttack);
        b.SetPlayer(this);
        b.SetTargetPositon(bulletFuturePosition);
        //rb.bodyType = RigidbodyType2D.Dynamic;
    }

    public override void TakeDamage(float _damage)
    {
        Hp -= _damage;
        Alive();
        photonView.RPC("HpSynchronization", RpcTarget.AllBuffered, Hp);
    }

    public override void AbilityOne()
    {
        if (Mana > CostAbilityOne)
        {
            Mana -= CostAbilityOne;
            photonView.RPC("SetManaPun", RpcTarget.AllBuffered, Mana);
            Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float startAngel = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            for (float i = - ((float)CountBullet - 1) /2; i <= ((float)CountBullet - 1) / 2; i++)
            {
                GameObject bulletGameoject = PhotonNetwork.Instantiate(BulletOb.name, transform.position, Quaternion.identity);
                SniperBullet b = bulletGameoject.GetComponent<SniperBullet>();
                b.SetRange(RangeAttack);
                b.SetPlayer(this);
                Vector2 target = new Vector2(Mathf.Cos((Mathf.PI / 180) * ((angel / CountBullet) * i + startAngel)), Mathf.Sin((Mathf.PI / 180) * ((angel / CountBullet) * i + startAngel))) + (Vector2)transform.position;
                b.SetTargetPositon(target);
            }
        }
    }
    public override void AbilityTwo() // ychituvat procaxhky
    {
        if (abilityTwoActive)
        {
            abilityTwoActive = false;
            PlayerSpriteBody.color = Color.white;
            photonView.RPC("SetColor", RpcTarget.AllBuffered, 1f, 1f, 1f);
            AttackSpeed *= 3;
        }
        else if (!abilityTwoActive & Mana > CostAbilityTwo)
        {
            abilityTwoActive = true;
            PlayerSpriteBody.color = new Color(1f, 0.8f, 0f);
            photonView.RPC("SetColor", RpcTarget.AllBuffered, 1f, 0.8f, 0f);
            AttackSpeed /= 3;
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

    public override void UpdateStats()
    {
        UpdateStandartStats();
        if (abilityTwoActive)
        {
            AttackSpeed /= 3;
        }
    }


    public override void LevelUpStat(StatType statType)
    {
        base.LevelUpStat(statType);
    }
}