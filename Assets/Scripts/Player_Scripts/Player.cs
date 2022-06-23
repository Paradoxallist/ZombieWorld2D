using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public abstract class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerFactory player;

    private PlayerLevel level;
    private List<PlayerStat> stats;


    [SerializeField]
    private float Ex;

    [SerializeField]
    protected PhotonView photonView;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Text NicknameText;
    [SerializeField]
    private BarCharacter hpBar;
    [SerializeField]
    private BarCharacter manaBar;
    [SerializeField]
    private GameObject PlayerBody;
    [SerializeField]
    private SpriteRenderer PlayerSpriteBody;
    [SerializeField]
    protected float Score;

    protected float Hp;
    protected float Mana;

    private float timeWitoutAttack, timeWitoutAbilityOne, timeWitoutAbilityTwo;
    public float xMax, yMax, xMin, yMin;//

    private int ID;

    public void StartPlayer()
    {
        SetPlayerCharacteristics();
        NicknameText.text = (photonView.Owner.NickName);
        if (!photonView.IsMine)
        {
            NicknameText.color = Color.green;
        }
        Hp = GetPlayerStat(StatType.MaxHp).Value;
        hpBar.SetMaxValue(GetPlayerStat(StatType.MaxHp).Value, Hp);
        Mana = GetPlayerStat(StatType.MaxMana).Value;
        manaBar.SetMaxValue(GetPlayerStat(StatType.MaxMana).Value, Mana);
        timeWitoutAttack = 0;
        timeWitoutAbilityOne = 0;
        timeWitoutAbilityTwo = 0;
        GameManager.Instance.AddPlayer(this);
    }

    private void SetPlayerCharacteristics()
    {
        stats = new List<PlayerStat>();
        for (int i = 0; i < player.Stats.Count; i++)
        {
            stats.Add(new PlayerStat(player.Stats[i].Modifier, player.Stats[i].ValuePrice, player.Stats[i].Value, player.Stats[i].MaxLevel, player.Stats[i].Level, player.Stats[i].StatType, player.Stats[i].SpriteStat));
        }
        level = new PlayerLevel(player.Level.ValuePriceEx, player.Level.ModifierPriceEx, player.Level.Level, player.Level.MaxLevel);
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
            else 
            {
                Move(horizontal, vertical);
            }
            if (Input.GetKeyDown(KeyCode.Q) && timeWitoutAbilityOne > GetPlayerStat(StatType.CooldownAbilityOne).Value)
            {
                UseAbilityOne();
                timeWitoutAbilityOne = 0;
            }
            if (Input.GetKeyDown(KeyCode.E) && timeWitoutAbilityTwo > GetPlayerStat(StatType.CooldownAbilityTwo).Value)
            {
                UseAbilityTwo();
                timeWitoutAbilityTwo = 0;
            }
            Rotate();
            timeWitoutAttack += Time.deltaTime;
            timeWitoutAbilityOne += Time.deltaTime;
            timeWitoutAbilityTwo += Time.deltaTime;
            //Move(horizontal, vertical);
        }
        Hp += GetPlayerStat(StatType.HpRagen).Value * Time.deltaTime;
        Mana += GetPlayerStat(StatType.ManaRegen).Value * Time.deltaTime;
        Hp = Mathf.Clamp(Hp, 0f, GetPlayerStat(StatType.MaxHp).Value);
        hpBar.SetValue(Hp);
        Mana = Mathf.Clamp(Mana, 0f, GetPlayerStat(StatType.MaxMana).Value);
        manaBar.SetValue(Mana);
    }

    public void Rotate()
    {
        Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angel = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        PlayerBody.transform.rotation = Quaternion.Euler(0, 0, angel - 90);
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
        PlayerBody.transform.rotation = Quaternion.Euler(0, 0, angel - 90);
    }

    [PunRPC]
    public void SetManaPun(float _mana)
    {
        Mana = Mathf.Clamp(_mana, 0, GetPlayerStat(StatType.MaxMana).Value);
    }

    [PunRPC]
    public void SetHpPun(float _Hp)
    {
        Hp = Mathf.Clamp(_Hp, 0, GetPlayerStat(StatType.MaxHp).Value);
    }

    public void Move(float _moveHorizontal, float _moveVertical)
    {
        rb.velocity = new Vector2(_moveHorizontal, _moveVertical).normalized * GetPlayerStat(StatType.Speed).Value;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, xMin, xMax), Mathf.Clamp(transform.position.y, yMin, yMax), transform.position.z);
    }

    public abstract void Attack();

    public void SetPlusPrize(float PlusScore,float PlusEX)
    {
        photonView.RPC("SetScorePun", RpcTarget.AllBuffered, Score + PlusScore);
        photonView.RPC("SetExPun", RpcTarget.AllBuffered, Ex + PlusEX);
    }

    [PunRPC]
    public void SetScorePun(float _Score)
    {
        Score = _Score;
    }

    [PunRPC]
    public void SetExPun(float _EX)
    {
        Ex = _EX;
        while(Ex >= Level.ValuePriceEx)
        {
            Ex -= Level.ValuePriceEx;
            Level.LevelUP();
            photonView.RPC("SetHpPun", RpcTarget.AllBuffered, GetPlayerStat(StatType.MaxHp).Value);
            photonView.RPC("SetManaPun", RpcTarget.AllBuffered, GetPlayerStat(StatType.MaxMana).Value);
        }
    }

    [PunRPC]
    public void SetColor(float R, float G, float B)
    {
        PlayerSpriteBody.color = new Color(R, G, B);
    }

    public abstract void TakeDamage(float _damage);

    public abstract void UseAbilityOne();

    public abstract void UseAbilityTwo();

    public void CheckAlive()
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

    public void Resurrect()
    {
        photonView.gameObject.SetActive(true);
        photonView.RPC("SetActivePun", RpcTarget.AllBuffered, true);
        photonView.RPC("SetHpPun", RpcTarget.AllBuffered, GetPlayerStat(StatType.MaxHp).Value);
        photonView.RPC("SetManaPun", RpcTarget.AllBuffered, GetPlayerStat(StatType.MaxMana).Value);
    }

    public PlayerStat GetPlayerStat(StatType statType)
    {
        return Stats.First(stat => stat.StatType == statType);
    }

    public virtual void LevelUpStat(StatType statType)
    {
        PlayerStat playerStat = GetPlayerStat(statType);
        Score -= GetPlayerStat(statType).ValuePrice;
        playerStat.Update();
        if (statType == StatType.MaxHp)
        {
            Hp += playerStat.Modifier;
        }
        if (statType == StatType.MaxMana)
        {
            Mana += playerStat.Modifier;
        }
    }

    public bool CanBuyStat(int i, float modifireLevel)
    {
        return Score >= Stats[i].ValuePrice && Stats[i].Level < Stats[i].MaxLevel && Stats[i].Level < Level.Level * modifireLevel;
    }

    public void SetID(int _ID) => ID = _ID;
    public float GetHp() => Hp;
    public float GetMana() => Mana;
    public PhotonView GetPhotonView() => photonView;
    public float GetEx() => Ex;
    public float GetScore() => Score;

    public List<PlayerStat> Stats => stats;

    public PlayerLevel Level => level;

}
