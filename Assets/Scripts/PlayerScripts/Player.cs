using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public abstract class Player : MonoBehaviour//, IPunObservable
{
    public float MaxHp;
    public float Hp;
    public float MaxMana;
    public float Mana;
    public float HpRegen;
    public float ManaRegen;
    public float Damage;
    public float Speed;

    public float CostAbilityOne;
    public float CostAbilityTwo;

    public List<PlayerStat> Stats;

    public PhotonView photonView;

    public Rigidbody2D rb;

    public Text NicknameText;
    public BarOverCharacter hpBar;
    public BarOverCharacter manaBar;
    public GameObject PlayerBody;
    public SpriteRenderer PlayerSpriteBody;

    public float Score;
    public float delayAttack;
    private float timeWitoutAttack;

    public void StartPlayer()
    {
        NicknameText.text = (photonView.Owner.NickName);
        if (!photonView.IsMine)
        {
            NicknameText.color = Color.green;
        }
        UpdateStat();
        Hp = MaxHp;
        hpBar.SetMaxValue(MaxHp,Hp);
        Mana = MaxMana;
        manaBar.SetMaxValue(MaxMana,Mana);
        timeWitoutAttack = 0;
        GameManager.Instance.AddPlayer(this);
    }

    public void UpdatePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (photonView.IsMine)
        {
            if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                rb.velocity = new Vector2(0, 0);
            }
            else {
                Move(horizontal, vertical);
            }
            if (Input.GetKeyDown(KeyCode.Q)){
                AbilityOne();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                AbilityTwo();
            }
            Rotate();
            timeWitoutAttack += Time.deltaTime;
            //Move(horizontal, vertical);
        }
        Hp += HpRegen * Time.deltaTime;
        Mana += ManaRegen * Time.deltaTime;
        if (Hp > MaxHp)
            Hp = MaxHp;
        hpBar.SetValue(Hp);
        if (Mana > MaxMana)
            Mana = MaxMana;
        manaBar.SetValue(Mana);
    }

    public void Rotate()
    {
        Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angel = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        PlayerBody.transform.rotation = Quaternion.Euler(0, 0, angel - 90);
        //photonView.RPC("SetRotatePun", RpcTarget.AllBuffered, angel);
    }

    public void SetTimeWitoutAttack(float _timeWitoutAttack)
    {
        timeWitoutAttack = _timeWitoutAttack;
    }

    public float GetTimeWitoutAttack()
    {
        return timeWitoutAttack;
    }

    [PunRPC]
    public void SetRotatePun(float angel)
    {
        SetRotate(angel);
    }

    public void SetRotate(float angel)
    {
        PlayerBody.transform.rotation = Quaternion.Euler(0, 0, angel - 90);
    }

    [PunRPC]
    public void SetManaPun(float mana)
    {
        Mana = mana;
    }

    public void Move(float _moveHorizontal, float _moveVertical)
    {
        rb.velocity = new Vector2(_moveHorizontal, _moveVertical).normalized * Speed;
    }

    public abstract void Attack();

    public void SendScore()
    {
        photonView.RPC("SetScore", RpcTarget.AllBuffered, Score);
    }

    [PunRPC]
    public void SetScore(int _Score)
    {
        Score = _Score;
    }

    [PunRPC]
    public void SetColor(float R, float G, float B)
    {
        PlayerSpriteBody.color = new Color(R, G, B);
    }

    public abstract void TakeDamage(float _damage);

    public abstract void AbilityOne();

    public abstract void AbilityTwo();

    public void Alive()
    {
        if (Hp <= 0)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel(1);
        }
    }

    public PlayerStat GetPlayerStat(StatType statType)
    {
        return Stats.First(stat => stat.StatType == statType);
    }

    public void UpdateStat()
    {
        Damage = GetPlayerStat(StatType.Damage).Value;
        if (MaxHp != GetPlayerStat(StatType.MaxHp).Value)
        {
            Hp += GetPlayerStat(StatType.MaxHp).Value - MaxHp;
            MaxHp = GetPlayerStat(StatType.MaxHp).Value;
            hpBar.SetMaxValue(MaxHp, Hp);
        }
        Speed = GetPlayerStat(StatType.Speed).Value;
    }

    public virtual void LevelUpStat(StatType statType)
    {
        PlayerStat playerStat = GetPlayerStat(statType);
        playerStat.Update();
        UpdateStat();
        photonView.RPC("SetDataCharacteristics", RpcTarget.AllBuffered, Hp, Damage, Speed);
    }

    [PunRPC]
    public void SetDataCharacteristics(float _Hp, float _Damage, float _Speed)
    {
        Hp = _Hp;
        Damage = _Damage;
        Speed = _Speed;
    }

    /*public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(angel);
        }
        else
        {
            angel = (float)stream.ReceiveNext();

        }
    }*/
}
