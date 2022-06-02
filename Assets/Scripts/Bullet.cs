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
    private Player player;

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

    public void SetTargetPositon(Vector2 targetPos, Player _player)
    {
        Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
        print(direction);
        print(targetPos);
        targetPosition = direction * Range + (Vector2)transform.position;
        startPos = transform.position;
        isMove = true;
        player = _player;
        float angel = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angel);
    }

    public void DestroyHimself()
    {
        PhotonNetwork.Destroy(PV);
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
            Enemy enemy = coll.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
            if (enemy == null || enemy.Hp <= 0)
            {
                player.Score++;
            }
            player.SendScore();
            DestroyHimself();
        }
    }
}
