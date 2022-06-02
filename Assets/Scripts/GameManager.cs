using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public List<GameObject> ClassPrefab;
    //public MapController MapController;
    public int NumClass;
    public float minX, minY, maxX, maxY;

    public List<Player> players = new List<Player>();
    public List<Enemy> enemies = new List<Enemy>();
    public GameObject EnemyGameObject;
    public PhotonView PV;
    public static GameManager Instance;
    public GameObject Spawner;

    private Zilot MyZilot;
    private Sniper MySniper;
    public Player MyPlayer;

    public PlayerTop Top;
    public Transform StartVictim;
    public Transform SpawnPosition;
    
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
        Top.SetText(players);
    }

    public void AddPlayer(Player player)
    {
        players.Add(player);
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
        MyPlayer = PlayerOb.GetComponent<Player>();
        switch (NumClass)
        {
            case 0:
                MyZilot = PlayerOb.GetComponent<Zilot>();
                break;
            case 1:
                MySniper = PlayerOb.GetComponent<Sniper>();
                break;
            default:
                break;
        }
    }

    public void InstEnemy()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Vector3 EnemyPosition = SpawnPosition.position;
            GameObject en = PhotonNetwork.InstantiateRoomObject(EnemyGameObject.name, EnemyPosition, Quaternion.identity);
            //GameObject en = PhotonNetwork.Instantiate(EnemyGameObject.name, EnemyPosition, Quaternion.identity);
            Enemy enemy = en.GetComponent<Enemy>();
            enemy.destinationSetter.target = StartVictim;
            enemies.Add(enemy);
        }
    }

    public void LevelUpStats(int NumStat, TMP_Text LevelText)
    {
        switch (NumClass)
        {
            case 0:
                MyZilot.LevelUpStat(NumStat);
                break;
            case 1:
                MySniper.LevelUpStat(NumStat,LevelText);
                break;
            default:
                break;
        }
    }
}
