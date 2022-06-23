using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Sword : MonoBehaviour
{
    public Player player;
    public PhotonView photonView;
    public float KickbackForce;
    private List<BaseEnemy> enemyList = new List<BaseEnemy>();


    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Enemy")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                BaseEnemy enemy = coll.GetComponent<BaseEnemy>();
                if (!enemyList.Contains(enemy))
                {
                    enemy.TakeDamage(player.GetPlayerStat(StatType.Damage).Value, player);
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
