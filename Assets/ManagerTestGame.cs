using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class ManagerTestGame : MonoBehaviour
{
    public TMP_Text ttext;
    public TMP_InputField inputField;
    public PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        //PhotonNetwork.InstantiateRoomObject()
    }

    public void PushButton()
    {
        PV.RPC("SendData", RpcTarget.AllBuffered, inputField.text);
        ttext.text = inputField.text;
    }

    [PunRPC]
    void SendData(string massege)
    {
        ttext.text = massege;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
