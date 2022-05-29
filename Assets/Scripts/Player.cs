using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Player : MonoBehaviour, IPunObservable
{
    public float MaxHp;
    public float Hp;
    public float Damage;
    public float Speed;

    public PhotonView photonView;
    private float angel;
    [SerializeField]
    private Rigidbody2D rb;

    public Text NicknameText;
    public HpBar hpBar;
    public List<Sprite> Sprites;
    public SpriteRenderer spriteBody;

    public void StartPlayer()
    {
        NicknameText.text = (photonView.Owner.NickName);
        if (!photonView.IsMine)
        {
            NicknameText.color = Color.green;
        }

        hpBar.SetMaxHealth(MaxHp);
        Hp = MaxHp;
    }

    public void UpdatePlayer()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        if (photonView.IsMine)
        {
            Move(moveHorizontal, moveVertical);
            SpriteChange(moveHorizontal, moveVertical);
        }
        hpBar.SetHealth(Hp);
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

    public void SpriteChange(float _moveHorizontal, float _moveVertical)
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
    }

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
