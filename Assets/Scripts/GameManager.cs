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
    //public int Kek;
    public static GameManager Instance;
    public GameObject Spawner;
    float ttime;

    private void Start()
    {
        //PhotonView = GetComponent<PhotonView>();
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
        //if (players.Count > 0)
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
        /*for (int i = 0; i < _players.Count; i++)
        {
            foreach (Player player in players)
            {
                if (_players[i] != player)
                    players.Add(player);
            }
        }
        for (int i = 0; i < _enemies.Count; i++)
        {
            foreach (Enemy enemy in enemies)
            {
                if (_enemies[i] != enemy)
                    enemies.Add(enemy);
            }
        }*/
        print("good");
    }

    /*public void UpdateHpBarEnemies()
    {
        if (enemies.Count > 0)
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.Alive();
            }
        }
    }*/
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

    /*public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(players);
            stream.SendNext(enemies);
        }
        else
        {

            List<Player> playersSynhronize = (List<Player>)stream.ReceiveNext();
            for (int i = 0; i < playersSynhronize.Count; i++)
            {
                foreach (Player player in players)
                {
                    if (playersSynhronize[i] == player)
                        players.Add(player);
                }
            }
            List<Enemy> enemiesSynhronize = (List<Enemy>)stream.ReceiveNext();
            for (int i = 0; i < enemiesSynhronize.Count; i++)
            {
                foreach (Enemy enemy in enemies)
                {
                    if (enemiesSynhronize[i] == enemy)
                        enemies.Add(enemy);
                }
            }

        }
    }*/

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
