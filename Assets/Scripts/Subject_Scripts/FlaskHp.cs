using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FlaskHp : Flask
{
    public override void TouchPlayer(Player player)
    {
        if (player.GetHp() < player.GetPlayerStat(StatType.MaxHp).Value)
        {
            player.GetPhotonView().RPC("SetHpPun", RpcTarget.AllBuffered, (player.GetHp() + Effect));
            DestroyHimself();
        }
    }
}
