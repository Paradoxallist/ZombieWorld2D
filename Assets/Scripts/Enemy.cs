using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Pathfinding;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float MaxHp;
    public float Hp;
    public float Damage;
    public float AttackSpeed;
    private float timeWithoutAttack;
    public float Speed;

    [SerializeField] private List<Sprite> Sprites;
    public SpriteRenderer spriteRenderer;
    private List<Player> attackedPlayers;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private AIPath aIPath;
    [SerializeField] private AIDestinationSetter destinationSetter; 
    public Transform victim;

    public HpBar hpBar;
    public PhotonView PV;
    public EnemyAgr agr;

    public Text HP_Text;
    public void StartEnemy()
    {
        //PhotonViewZ = GetComponent<PhotonView>();
        hpBar.SetMaxHealth(MaxHp);
        Hp = MaxHp;
        attackedPlayers = new List<Player>();
        aIPath.maxSpeed = Speed;
        //Send();
    }

    public void UpdateEnemy()
    {
        timeWithoutAttack += Time.deltaTime;
        if (victim != null)
        { 
                Vector3 direction = victim.position - transform.position;
                float angel = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                SpriteChange(angel);
                destinationSetter.target = victim;
        }
        else
        {
            if (agr.victim != null)
                victim = agr.victim.transform;
        }
        if (AttackSpeed < timeWithoutAttack && attackedPlayers.Count > 0)
        {
            foreach (var player in attackedPlayers)
            {
                if (Vector2.Distance(player.transform.position, transform.position) < 1f)
                    player.TakeDamage(Damage);
            }
            timeWithoutAttack = 0;
        }
        hpBar.SetHealth(Hp);
        HP_Text.text = Hp + "/" + MaxHp;
    }

    private void SpriteChange(float _angel)
    {
        if (_angel > -45 && _angel < 45)
        {
            spriteRenderer.sprite = Sprites[0];
        }
        else if (_angel > 45 && _angel < 135)
        {
            spriteRenderer.sprite = Sprites[1];
        }
        else if (_angel > 135 && _angel < 225)
        {
            spriteRenderer.sprite = Sprites[2];
        }
        else
        {
            spriteRenderer.sprite = Sprites[3];
        }
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        /*if (coll.tag == "Bullet")
        {
            Bullet bullet = coll.GetComponent<Bullet>();
            //GameManager.Instance.UpdateHpBarEnemies();
            TakeDamage(bullet.damage);
            bullet.DestroyHimself();
            print("estProbitie");
        }*/
        if (coll.CompareTag("Player"))
        {
            Player p = coll.GetComponent<Player>();
            if (p != null && attackedPlayers != null)
            {
                bool add = true;
                foreach (var player in attackedPlayers)
                {
                    if (p == player)
                        add = false;
                }
                if (add)
                    attackedPlayers.Add(p);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            Player p = coll.GetComponent<Player>();
            attackedPlayers.Remove(p);
        }
        /*if (coll.CompareTag("Bullet"))
        {
            Bullet b = coll.GetComponent<Bullet>();
            TakeDamage(b.damage);
            b.DestroyHimself();
        }*/
    }

    public void TakeDamage(float _damage)
    {
        Hp -= _damage;
        //GameManager.Instance.Send(this);
        PV.RPC("SetHp", RpcTarget.AllBuffered, Hp);

        Alive();
    }
    [PunRPC]
    public void SetHp(float _hp)
    {
        Hp = _hp;
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

    public void Send()
    {
        //if (players.Count > 0)
        PV.RPC("SynchronizingDataEnemy", RpcTarget.AllBuffered, 5f);
    }

    [PunRPC]
    private void SynchronizingDataEnemy(float _hp)
    {
        print(_hp);
        //Hp = _hp;
        //Alive();
    }

    /*public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Hp);
        }
        else
        {
            if (Hp != (float)stream.ReceiveNext())
                Hp = (float)stream.ReceiveNext();

        }
    }*/
}
