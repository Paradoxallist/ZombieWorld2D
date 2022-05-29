using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class ListItem : MonoBehaviour
{
    [SerializeField] TMP_Text textRoomName;
    [SerializeField] TMP_Text textPlayerCount;

    public void SetInfo(RoomInfo info)
    {
        textRoomName.text = info.Name;
        textPlayerCount.text = info.PlayerCount + "/" + info.MaxPlayers;
    }

    public void JoinToListRoom()
    {
        PhotonNetwork.JoinRoom(textRoomName.text);
    }
}
