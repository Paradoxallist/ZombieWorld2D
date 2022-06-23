using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class MenuManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField CreatInput;
    public TMP_InputField JoinInput;
    public TMP_InputField Name;
    public GameObject Menu;
    public GameObject PanelConnect;
    public GameObject ConnectingImageGameObject;


    private bool SetName;

    [SerializeField] ListItem itemPrefab; 
    [SerializeField] Transform content;
    List<ListItem> listItems;
    private void Start()
    {
        SetName = false;
        listItems = new List<ListItem>();
        Screen.fullScreen = true;
        if (!PhotonNetwork.IsConnected)
        {
            //PhotonNetwork.GameVersion = Application.version;
            //PhotonNetwork.ConnectToRegion("ru");
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.ConnectToRegion("ru");
            PhotonNetwork.GameVersion = Application.version;
        }
    }

    private void Update()
    {
        if (!SetName)
        {
            if (Input.GetKeyDown(KeyCode.Return)) //Press Enter
            {
                PushSingIn();
                SetName = true;
                Menu.SetActive(false);
                PanelConnect.SetActive(true);
            }
        }
    }

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(CreatInput.text, roomOptions, TypedLobby.Default);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(JoinInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
        //PhotonNetwork.LoadLevel(3);
    }

    public void PushSingIn()
    {
        PhotonNetwork.NickName = Name.text;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        /*foreach (var item in listItems)
        {
            Destroy(item.gameObject);
        }*/

        foreach (RoomInfo roomInfo in roomList)
        {
            ListItem listItem = Instantiate(itemPrefab, content);
            if (listItem != null)
                listItem.SetInfo(roomInfo);
        }
        //listItems.Clear();
    }

    public override void OnConnectedToMaster()
    {
        ConnectingImageGameObject.SetActive(false);
        Menu.SetActive(true);
        PhotonNetwork.JoinLobby();
    }
}
