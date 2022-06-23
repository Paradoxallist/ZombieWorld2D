using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Pathfinding;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField]
    protected float MaxHp;
    protected float Hp;
    [SerializeField]
    protected float Damage;
    [SerializeField]
    protected float RangeAttack;
    [SerializeField]
    protected float AttackSpeed;
    [SerializeField]
    protected float Speed;
    [SerializeField]
    protected float RotationSpeed;
    [SerializeField]
    protected float Prize;
    [SerializeField]
    protected float PrizeEX;
    [SerializeField]
    protected PhotonView PV;
    [SerializeField]
    protected AIPath aIPath;
    [SerializeField]
    protected AIDestinationSetter destinationSetter;
    [SerializeField]
    protected BarCharacter hpBar;
    [SerializeField]
    protected Text HP_Text;
    [SerializeField]
    protected GameObject EnemyBody;
    [SerializeField]
    protected SpriteRenderer EnemySpriteBody;
    [SerializeField]
    protected float AgrRange;

    public float xMax, yMax, xMin, yMin;//potom

    protected Player victim;
    protected float timeWithoutAttack;


    public void StartEnemy()
    {
        Hp = MaxHp;
        hpBar.SetMaxValue(MaxHp,Hp);
        aIPath.maxSpeed = Speed;
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

    [PunRPC]
    public void SetHp(float _hp)
    {
        Hp = _hp;
        HP_Text.text = Hp + "/" + MaxHp;
    }

    public void CheckAlive()
    {
        if(Hp <= 0)
        {
            DestroyHimself();
        }
    }

    public abstract void DestroyHimself();

    [PunRPC]
    public void DestroyEnemy()
    {
        if (PV.IsMine)
            PhotonNetwork.Destroy(PV);
    }

    private void SearchVictim()
    {
        Player nearestPlayer = null;
        float minDistance = Mathf.Infinity;
        foreach (Player player in GameManager.Instance.players)
        {
            if (player != null && player.gameObject.activeSelf)
            {
                float distance = Vector2.Distance(transform.position, player.transform.position);
                if (distance < minDistance && distance < AgrRange)
                {
                    minDistance = distance;
                    nearestPlayer = player;
                }
            }
        }
        if (nearestPlayer != null)
        {
            victim = nearestPlayer;
        }
    }
    public PhotonView photonView => PV;
}