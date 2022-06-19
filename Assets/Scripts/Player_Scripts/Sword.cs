using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Sword : MonoBehaviour
{
    public Player player;
    public PhotonView photonView;
    public float KickbackForce;
    private List<Enemy> enemyList = new List<Enemy>();


    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Enemy")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Enemy enemy = coll.GetComponent<Enemy>();
                if (!enemyList.Contains(enemy))
                {
                    enemy.TakeDamage(player.GetDamage(), player);
                    enemyList.Add(enemy);
                }
            }
        }
    }

    public void ResetList()
    {
        if (PhotonNetwork.IsMasterClient)
            enemyList.Clear();
        else
            photonView.RPC("PunResetList", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void PunResetList()
    {
        enemyList.Clear();
    }
}
