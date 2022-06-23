using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public abstract class Flask : MonoBehaviour
{
    private PhotonView PV;
    public float Effect;
    [Range(0,100)]
    public float DropChance;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player") && !coll.isTrigger)
        {
            Player player = coll.GetComponentInParent<Player>();
            if (player != null)
            {
                TouchPlayer(player);
            }
        }
    }
    public abstract void TouchPlayer(Player player);

    public void DestroyHimself()
    {
        PV.RPC("DestroyObject", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void DestroyObject()
    {
        if (PV.IsMine && PV != null)
            PhotonNetwork.Destroy(PV);
    }
}
