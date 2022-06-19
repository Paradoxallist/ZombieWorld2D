using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ButtonLeaveRoom : MonoBehaviour
{
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);
    }
}
