using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    public float Speed;

    private Vector2 targetPosition;
    private Vector2 startPos;
    private bool isMove = false;
    private float range;
    private float time;
    public PhotonView PV;

    public void StartBullet()
    {
        //PV = GetComponent<PhotonView>();
        time = 0;
    }

    public void UpdateBullet()
    {
        if (!isMove) {return; }
        time += Speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(startPos, targetPosition, time);
        if ((Vector2)transform.position == targetPosition)
        {
            DestroyHimself();
        }
    }

    public void SetRange(float _range)
    {
        range =_range;
    }

    public void SetTargetPositon(Vector2 targetPos)
    {
        Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
        targetPosition = direction * range + (Vector2)transform.position;
        startPos = transform.position;
        isMove = true;
        float angel = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angel);
    }

    public void DestroyHimself()
    {
        PV.RPC("DestroyBullet", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void DestroyBullet()
    {
        if (PV.IsMine)
            PhotonNetwork.Destroy(PV);
    }
}
