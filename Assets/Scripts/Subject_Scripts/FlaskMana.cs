using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FlaskMana : Flask
{
    public override void TouchPlayer(Player player)
    {
        if (player.GetMana() < player.GetPlayerStat(StatType.MaxMana).Value)
        {
            player.GetPhotonView().RPC("SetManaPun", RpcTarget.AllBuffered, (player.GetMana() + Effect));
            DestroyHimself();
        }
    }
}
