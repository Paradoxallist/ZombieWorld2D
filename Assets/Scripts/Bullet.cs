using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    private Vector2 targetPosition;
    private Vector2 startPos;
    private bool isMove = false;
    public float Speed;
    public float damage;
    public float Range;
    private float time;
    public PhotonView PV;

    void Start()
    {
        time = 0;
    }

    void Update()
    {
        if (!isMove) {return; }
        time += Speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(startPos, targetPosition, time);
        if ((Vector2)transform.position == targetPosition)
        {
            DestroyHimself();
        }
    }

    public void SetTargetPositon(Vector2 targetPos)
    {
        Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
        print(direction);
        print(targetPos);
        targetPosition = direction * Range + (Vector2)transform.position;
        startPos = transform.position;
        isMove = true;
    }

    public void DestroyHimself()
    {
        PhotonNetwork.Destroy(PV);
        /*if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.Destroy(PV);
        else
        {
            Destroy(PV);
            PV.RPC("DestroyBullet", RpcTarget.MasterClient);
        }*/
    }
    [PunRPC]
    public void DestroyBullet()
    {
        PhotonNetwork.Destroy(PV);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Wall")
        {
            DestroyHimself();
        }
        if (coll.tag == "Enemy")
        {
            //if(e)
            Enemy enemy = coll.GetComponent<Enemy>();
            //GameManager.Instance.UpdateHpBarEnemies();
            enemy.TakeDamage(damage);
            DestroyHimself();
        }
    }
}
