using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Player : MonoBehaviour, IPunObservable
{
    public float MaxHp;
    public int LVLMaxHp;
    public float Hp;
    public float HpRegen;
    public float LVLRegen;
    public float Damage;
    public int LVLDamage;
    public float Speed;
    public int LVLSpeed;

    public PhotonView photonView;
    private float angel;

    public Rigidbody2D rb;

    public Text NicknameText;
    public HpBar hpBar;
    //public List<Sprite> Sprites;
    public SpriteRenderer SriteBody; 

    public int Score;

    public Animator animator;

    public void StartPlayer()
    {
        NicknameText.text = (photonView.Owner.NickName);
        if (!photonView.IsMine)
        {
            NicknameText.color = Color.green;
        }

        hpBar.SetMaxHealth(MaxHp);
        Hp = MaxHp;
        LVLMaxHp = 0;
        LVLRegen = 0;
        LVLDamage = 0;
        LVLSpeed = 0;
        Score = 0;
        GameManager.Instance.AddPlayer(this);
    }

    public void UpdatePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (photonView.IsMine)
        {
            Move(horizontal, vertical);
        }
        Hp += HpRegen * Time.deltaTime;
        if (Hp > MaxHp)
            Hp = MaxHp;
        hpBar.SetHealth(Hp);

        if (horizontal > 0)
        {
            Flip(false);
        }
        else if (horizontal < 0)
        {
            Flip(true);
        }
        bool move = false;
        if (Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0)
            move = true;
        animator.SetBool("Move",move);
    }

    public void Flip(bool facing)
    {
        SriteBody.flipX = facing;
    }

    public void Move(float _moveHorizontal, float _moveVertical)
    {
        rb.velocity = new Vector2(_moveHorizontal, _moveVertical) * Speed;
        //Vector2 nextPosition = (Vector2)transform.position + new Vector2(_moveHorizontal, _moveVertical) * Speed;
        //agent.SetDestination(nextPosition);
    }

    /*private void Rotate()
    {
        Vector3 direction = targetPosition - transform.position;    
        angel = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0, 0, angel - 90);
        //SpriteChange(angel);
    }*/

    public void SendScore()
    {
        photonView.RPC("SetScore", RpcTarget.AllBuffered, Score);
    }

    [PunRPC]
    public void SetScore(int _Score)
    {
        Score = _Score;
    }

    /*public void SpriteChange(float _moveHorizontal, float _moveVertical)
    {
        if (_moveHorizontal > 0)
        {
            spriteBody.sprite = Sprites[0];
        }
        else if (_moveHorizontal < 0)
        {
            spriteBody.sprite = Sprites[1];
        }
        else if (_moveVertical > 0)
        {
            spriteBody.sprite = Sprites[2];
        }
        else
        {
            spriteBody.sprite = Sprites[3];
        }
    }*/

    public void TakeDamage(float _damage)
    {
        Hp -= _damage;
        Alive();
    }

    public void Alive()
    {
        if (Hp <= 0)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel(1);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(angel);
        }
        else
        {
            angel = (float)stream.ReceiveNext();
            
        }
    }
}
