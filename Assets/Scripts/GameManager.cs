using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public List<GameObject> ClassPrefab;
    //public MapController MapController;
    public int NumClass;
    public float minX, minY, maxX, maxY;

    //public List<Player> players = new List<Player>();
    public List<Enemy> enemies = new List<Enemy>();
    public GameObject EnemyGameObject;
    public PhotonView PV;
    public static GameManager Instance;
    public GameObject Spawner;
    float ttime;

    private void Start()
    {
        if (Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    private void Update()
    {
        /*if (PhotonNetwork.MasterClient == null)
        {
            //PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel(1);
        }*/
    }

    public void Send(Enemy enemy)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            
        }
        else
        {
            PV.RPC("SynchronizingDataPlayers", RpcTarget.MasterClient, enemy.Hp);//, enemies);
        }
    }

    [PunRPC]
    private void SynchronizingDataPlayers(string gg)//List<Player> _players, List<Enemy> _enemies)
    {
        print("good");
    }

    public void UpdateHpBarEnemy(Enemy enemy)
    {
        enemy.Alive();
    }

    /*public void AppPlayer(Player _player)
    {
        players.Add(_player);
    }*/

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(1);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        PV.RPC("SynchronizingDataPlayers", RpcTarget.AllBuffered, "gg");
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
    }

    public void PushStart()
    {
        Inicilization(ClassPrefab[NumClass]);
    }

    public void SetNumClass(int N)
    {
        NumClass = N;
    }

    void Inicilization(GameObject create)
    {
        Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
        GameObject PlayerOb = PhotonNetwork.Instantiate(create.name, randomPosition, Quaternion.identity);
        Camera.main.GetComponent<CameraControl>().target = PlayerOb.transform;
        Player pl = PlayerOb.GetComponent<Player>();
        //AppPlayer(pl);

    }

    public void InstEnemy()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Vector3 randomPosition = new Vector3(0, 0, 0);
            GameObject en = PhotonNetwork.InstantiateRoomObject(EnemyGameObject.name, randomPosition, Quaternion.identity);
            Enemy enemy = en.GetComponent<Enemy>();
            enemies.Add(enemy);
        }
    }
}
