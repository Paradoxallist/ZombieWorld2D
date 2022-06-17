using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public abstract class Player : MonoBehaviour//, IPunObservable
{
    private float maxHp;
    private float hp;
    private float maxMana;
    private float mana;
    private float hpRegen;
    private float manaRegen;
    private float damage;
    private float attackSpeed;
    private float speed;

    public float EX;
    public int LVL;
    public int LVLPoint;

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
    private float timeWitoutAttack;
    public float xMax, yMax, xMin, yMin;

    private int ID;

    public float MaxHp { get => maxHp; set => maxHp = value; }
    public float Hp { get => hp; set => hp = value; }
    public float MaxMana { get => maxMana; set => maxMana = value; }
    public float Mana { get => mana; set => mana = value; }
    public float HpRegen { get => hpRegen; set => hpRegen = value; }
    public float ManaRegen { get => manaRegen; set => manaRegen = value; }
    public float Damage { get => damage; set => damage = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public float Speed { get => speed; set => speed = value; }

    public void StartPlayer()
    {
        NicknameText.text = (photonView.Owner.NickName);
        if (!photonView.IsMine)
        {
            NicknameText.color = Color.green;
        }
        UpdateStats();
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
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, xMin, xMax), Mathf.Clamp(transform.position.y, yMin, yMax), transform.position.z);
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

    public void SetScore(float _Score)
    {
        Score = _Score;
        photonView.RPC("SetScorePun", RpcTarget.AllBuffered, Score);
    }

    [PunRPC]
    public void SetScorePun(float _Score)
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
            GameManager.Instance.PlayerDie(ID);
        }
    }

    public void Die()
    {
        photonView.gameObject.SetActive(false);
        photonView.RPC("SetActivePun", RpcTarget.AllBuffered, false);
    }

    [PunRPC]
    public void SetActivePun(bool active)
    {
        gameObject.SetActive(active);
    }

    public void Resurrection()
    {
        photonView.gameObject.SetActive(true);
        photonView.RPC("SetActivePun", RpcTarget.AllBuffered, true);
    }

    public PlayerStat GetPlayerStat(StatType statType)
    {
        return Stats.First(stat => stat.StatType == statType);
    }

    public abstract void UpdateStats();

    public void UpdateStandartStats()
    {
        if (MaxHp != GetPlayerStat(StatType.MaxHp).Value)
        {
            Hp += GetPlayerStat(StatType.MaxHp).Value - MaxHp;
            MaxHp = GetPlayerStat(StatType.MaxHp).Value;
            hpBar.SetMaxValue(MaxHp, Hp);
        }
        HpRegen = GetPlayerStat(StatType.HpRagen).Value;
        if (MaxMana != GetPlayerStat(StatType.MaxMana).Value)
        {
            Mana += GetPlayerStat(StatType.MaxMana).Value - MaxMana;
            MaxMana = GetPlayerStat(StatType.MaxMana).Value;
            manaBar.SetMaxValue(MaxMana, Mana);
        }
        ManaRegen = GetPlayerStat(StatType.ManaRegen).Value;
        Damage = GetPlayerStat(StatType.Damage).Value;
        Speed = GetPlayerStat(StatType.Speed).Value;
        AttackSpeed = GetPlayerStat(StatType.AttackSpeed).Value;
    }

    public virtual void LevelUpStat(StatType statType)
    {
        PlayerStat playerStat = GetPlayerStat(statType);
        playerStat.Update();
        UpdateStats();
    }

    [PunRPC]
    public void HpSynchronization(float _Hp)
    {
        Hp = _Hp;
    }

    [PunRPC]
    public void SetDataCharacteristics(float _Hp, float _Damage, float _Speed)
    {
        Hp = _Hp;
        Damage = _Damage;
        Speed = _Speed;
    }

    public void SetID(int _ID)
    {
        ID = _ID;
    }
    
    public int GetID()
    {
        return ID;
    }

    
}
