using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Pathfinding;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    public float MaxHp;
    public float Hp;
    public float Damage;
    public float RangeAttack;
    public float AttackSpeed;
    private float timeWithoutAttack;
    public float Speed;

    public float xMax, yMax, xMin, yMin;

    public float RotationSpeed;

    public float Prize;
    public float PrizeEX;

    //private List<Player> attackedPlayers;
    //private Rigidbody2D rb;
    private AIPath aIPath;
    private AIDestinationSetter destinationSetter; 
    //public Transform victim;

    public BarCharacter hpBar;
    public PhotonView PV;
    //public EnemyAgr agr;

    public Text HP_Text;
    public GameObject EnemyBody;
    public SpriteRenderer EnemySpriteBody;
    public Player victim;

    public float AgrRange;

    private bool stun;

    public void StartEnemy()
    {
        //rb = GetComponent<Rigidbody2D>();
        aIPath = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        Hp = MaxHp;
        hpBar.SetMaxValue(MaxHp,Hp);
        //attackedPlayers = new List<Player>();
        aIPath.maxSpeed = Speed;
        //Send();
    }

    public void SetTarget(Transform target)
    {
        destinationSetter.target = target;
    }

    public void SetTimeWithoutAttack(float newTime)
    {
        timeWithoutAttack = newTime;
    }

    public float GetTimeWithoutAttack()
    {
        return timeWithoutAttack;
    }

    public void UpdateEnemy()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            timeWithoutAttack += Time.deltaTime;
        }
        Rotate();
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, xMin, xMax), Mathf.Clamp(transform.position.y, yMin, yMax), transform.position.z);
        hpBar.SetValue(Hp);
        HP_Text.text = Hp + "/" + MaxHp;
        SearchVictim();
    }

    public void Rotate()
    {
        if (victim != null)
        {
            Vector3 direction = victim.transform.position - transform.position;
            float angel = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //EnemyBody.transform.rotation = Quaternion.Euler(0, 0, angel - 90);
            EnemyBody.transform.rotation = Quaternion.Lerp(EnemyBody.transform.rotation, Quaternion.Euler(0, 0, angel - 90), Time.deltaTime * RotationSpeed);
        }
    }

    public abstract void Attack();

    public abstract void TakeDamage(float _damage, Player player);

    public abstract void LevelUpWave(int Wave);
    public bool GetVariableStun()
    {
        return stun;
    }

    [PunRPC]
    public void StunPun(float timeStun)
    {
        EnemySpriteBody.color = Color.black;
        stun = true;
        aIPath.maxSpeed = 0;
        Invoke("StopStun", timeStun);
    }

    public void StopStun()
    {
        EnemySpriteBody.color = Color.white;
        aIPath.maxSpeed = Speed;
        stun = false;
    }

    [PunRPC]
    public void SetHp(float _hp)
    {
        Hp = _hp;
        HP_Text.text = Hp + "/" + MaxHp;
    }

    public void Alive()
    {
        if(Hp <= 0)
        {
            DestroyHimself();
        }
    }

    public void DestroyHimself()
    {
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.Destroy(PV);
        else
            PV.RPC("DestroyEnemy", RpcTarget.MasterClient);

    }
    [PunRPC]
    public void DestroyEnemy()
    {
        PhotonNetwork.Destroy(PV);
    }

    [PunRPC]
    public void SynchronizingDataEnemy(float maxHp,float hp, float damage)
    {
        MaxHp = maxHp;
        Hp = hp;
        Damage = damage;
    }

    private void SearchVictim()
    {
        Player NearPlaer = null;
        float distanse = Mathf.Infinity;
        foreach (Player player in GameManager.Instance.players)
        {
            if (player != null && player.gameObject.activeSelf)
            {
                float dist = Vector2.Distance(transform.position, player.transform.position);
                if (dist < distanse && dist < AgrRange)
                {
                    distanse = dist;
                    NearPlaer = player;
                }
            }
        }
        if (NearPlaer != null)
        {
            victim = NearPlaer;
        }
    }
}
