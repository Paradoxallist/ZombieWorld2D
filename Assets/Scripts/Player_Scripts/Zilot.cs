using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Zilot : Player
{
    public float Defens;
    public float AbilityDefens;
    public Sword SwordAttack;
    public float LifeTimeSwordAttack;
    public float SpeedDash;
    public float LengthDesh;
    public float TimeStun;
    private float StunDamage;

    private bool abilityOneActive;
    private bool abilityTwoActive;

    private float time;
    private Vector2 StartPosition;
    private Vector2 DeshPosition;
    private bool inWall;

    void Start()
    {
        StartPlayer();
        abilityOneActive = false;
        abilityTwoActive = false;
        inWall = false;
    }

    void Update()
    {
        UpdatePlayer();
        if (photonView.IsMine)
        {
            if (Input.GetMouseButton(0) && GetTimeWitoutAttack() > AttackSpeed)
            {
                SetTimeWitoutAttack(0);
                Attack();
            }
            if (abilityOneActive)
            {
                time += SpeedDash * Time.deltaTime;
                transform.position = Vector2.MoveTowards(StartPosition, DeshPosition, time);
                if ((Vector2)transform.position == DeshPosition)
                {
                    StopDash();
                }
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

    public override void TakeDamage(float _damage)
    {
        if (abilityOneActive) { return; }

        float takenDamage = _damage - Defens;
        if (takenDamage > 0)
        {
            Hp -= takenDamage;
            if(Hp < 0)
            {
                abilityOneActive = false;
                abilityTwoActive = false;
            }
            Alive();
            photonView.RPC("SetHpPun", RpcTarget.AllBuffered, Hp);
        }
    }

    public override void Attack()
    {
        SwordAttack.gameObject.SetActive(true);
        SwordAttack.ResetList();
        photonView.RPC("SetSwordActive", RpcTarget.AllBuffered,true);
        Invoke("DeactiveSword", LifeTimeSwordAttack);
    }

    private void DeactiveSword()
    {
        SwordAttack.gameObject.SetActive(false);
        photonView.RPC("SetSwordActive", RpcTarget.AllBuffered, false);
    }

    [PunRPC]
    public void SetSwordActive(bool status)
    {
        SwordAttack.gameObject.SetActive(status);
    }

    public override void AbilityOne()
    {
        
        if (Mana > CostAbilityOne && !abilityOneActive && !inWall)
        {
            Mana -= CostAbilityOne;
            photonView.RPC("SetManaPun", RpcTarget.AllBuffered, Mana);
            abilityOneActive = true;
            StartPosition = transform.position;
            Vector2 direction = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position).normalized;
            DeshPosition = direction * LengthDesh + (Vector2)transform.position;
            PlayerSpriteBody.color = Color.red;
            photonView.RPC("SetColor", RpcTarget.AllBuffered, 1f, 0f, 0f);
        }
    }

    public override void AbilityTwo()
    {
        if (abilityTwoActive)
        {
            abilityTwoActive = false;
            PlayerSpriteBody.color = Color.white;
            photonView.RPC("SetColor", RpcTarget.AllBuffered, 1f,1f,1f);
            Defens -= AbilityDefens;
        }
        else if(!abilityTwoActive & Mana > CostAbilityTwo)
        {
            abilityTwoActive = true;
            PlayerSpriteBody.color = new Color(0f,1f,1f);
            photonView.RPC("SetColor", RpcTarget.AllBuffered, 0f,1f,1f);
            Defens += AbilityDefens;
        }
        photonView.RPC("DefensSynchronization", RpcTarget.AllBuffered, Defens);
    }

    /*public override void Die()
    {
        gameObject.SetActive(false);
    }

    public override void Resurrection()
    {
        photonView.gameObject.SetActive(true);
        Hp = MaxHp;
        Mana = MaxMana;
        abilityOneActive = false;
        abilityTwoActive = false;
    }*/

    [PunRPC]
    public void DefensSynchronization(float _Defens)
    {
        Defens = _Defens;
    }

    private void StopDash()
    {
        time = 0;
        abilityOneActive = false;
        photonView.RPC("SetColor", RpcTarget.AllBuffered, 1f, 1f, 1f);
    }
    public override void UpdateStats()
    {
        UpdateStandartStats();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Wall"))
        {
            if (abilityOneActive)
            {
                StopDash();
            }
            inWall = true;
        }
        if (coll.CompareTag("Enemy"))
        {
            if (abilityOneActive)
            {
                Enemy enemy = coll.GetComponent<Enemy>();
                StunDamage = Damage * 2;
                enemy.TakeDamage(StunDamage,this);
                enemy.PV.RPC("StunPun", RpcTarget.AllBuffered,TimeStun);
                StopDash();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Wall")
        {
            inWall = false;
        }
    }
}
