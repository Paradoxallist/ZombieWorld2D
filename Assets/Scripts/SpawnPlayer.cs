using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayer : MonoBehaviour
{
    public List<GameObject> ClassPrefab; 
    public float minX, minY, maxX, maxY;

    private Player MyPlayer;
    private int NumClass;
    public InformationUpdate Info;
    public Store store;

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
        Camera.main.GetComponent<CameraControl>().target = PlayerOb.transform;
        MyPlayer = PlayerOb.GetComponent<Player>();
        Info.SetMyPlayer(MyPlayer);
        store.SetMyPlayer(MyPlayer);
        MyPlayer.SetID(GameManager.Instance.players.Count);
        if (PhotonNetwork.IsMasterClient)
        {
            GameManager.Instance.SpawnWave();
        }
    }
}
