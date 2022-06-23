using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayer : MonoBehaviour
{
    public List<GameObject> ClassPrefab; 
    public float minX, minY, maxX, maxY;//??

    [SerializeField]
    private CameraControl CameraControl;

    private Player MyPlayer;
    private int NumClass;
    public CanvasManager CanvasManager;

    public void SetNumClass(int N)
    {
        NumClass = N;
    }

    public void PushStart()
    {
        Inicilization(ClassPrefab[NumClass]);
    }

    void Inicilization(GameObject create)
    {
        Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
        GameObject PlayerOb = PhotonNetwork.Instantiate(create.name, randomPosition, Quaternion.identity);
        CameraControl.SetTarget(PlayerOb.transform);
        MyPlayer = PlayerOb.GetComponent<Player>();
        MyPlayer.SetID(GameManager.Instance.players.Count);
        CanvasManager.SetMyPlayer(MyPlayer);
        if (PhotonNetwork.IsMasterClient)
        {
            GameManager.Instance.SpawnWave();
            GameManager.Instance.MasterClientSetID();
        }
        else
        {
            GameManager.Instance.PV.RPC("MasterClientSetID", RpcTarget.MasterClient);
        }
    }
}
