using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public List<GameObject> EnemyPrefab;

    public float minX, minY, maxX, maxY;

    public List<Player> players = new List<Player>();
    private List<int> diePlayersID = new List<int>();
    public List<Enemy> enemies = new List<Enemy>();
    public PhotonView PV;
    public static GameManager Instance;
    public List<Transform> Spawner;

    public int Wave;
    public float TimeBetweenWaves;
    public float timeToWave;

    private bool startSpawnWave;

    private void Awake()
    {
        Wave = 0;
        startSpawnWave = false;
        players = new List<Player>();
        if (Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    private void Start()
    {
        PV.RPC("RequestMasterDataWave", RpcTarget.MasterClient);
    }

    private void Update()
    {
        timeToWave += Time.deltaTime;
        if (PhotonNetwork.IsMasterClient && !startSpawnWave && (TimeBetweenWaves + Wave * 3 - timeToWave) < 0)
        {
            SpawnWave();
        }
    }

    public void AddPlayer(Player player)
    {
        players.Add(player);
        player.SetID(players.Count - 1);
    }

    public void UpdateHpBarEnemy(Enemy enemy)
    {
        enemy.Alive();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(1);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)/////Poleznay Shtuka
    {
        base.OnPlayerEnteredRoom(newPlayer);
        //PV.RPC("SynchronizingDataPlayers", RpcTarget.AllBuffered, "gg");
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {

    }

    /*[PunRPC]
    public void MasterClientSetID()
    {
        for (int i = 0; i < players.Count; i++) {
            PV.RPC("ClientSetID", RpcTarget.MasterClient,i);
        }
    }

    [PunRPC]
    public void ClientSetID(int ID)
    {
        //MyPlayer.SetID(ID);
    }*/

    public void SpawnWave()
    {
        startSpawnWave = true;
        Wave++;
        for (int i = 0;i < Wave; i++)
        {
            Invoke("InstEnemy", i);
        }
        timeToWave = 0;
        PV.RPC("SetInfoWave", RpcTarget.AllBuffered, Wave);
        Resurrection();
        Invoke("SpawnWave", (Wave * 3 + TimeBetweenWaves));
    }

    [PunRPC]
    public void SetInfoWave(int _Wave)
    {
        timeToWave = 0;
        Wave = _Wave;
    }

    [PunRPC]
    public void RequestMasterDataWave()
    {
        PV.RPC("SynchronizingDataWave", RpcTarget.AllBuffered,Wave,timeToWave);
    }

    [PunRPC]
    public void SynchronizingDataWave(int _Wave, float _timeToWave)
    {
        timeToWave = _timeToWave;
        Wave = _Wave;
    }

    public void PlayerDie(int ID)
    {
        players[ID].Die();
        diePlayersID.Add(ID);
    }

    public void Resurrection()
    {
        for(int i = 0; i < diePlayersID.Count; i++)
        {
            players[diePlayersID[i]].Resurrection();
            players[diePlayersID[i]].Hp = players[diePlayersID[i]].MaxHp;
            players[diePlayersID[i]].Mana = players[diePlayersID[i]].MaxMana;
            Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
            players[diePlayersID[i]].transform.position = randomPosition;
        }
        diePlayersID.Clear();
    }

    public void InstEnemy()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < Spawner.Count; i++)
            {
                Vector3 EnemyPosition = Spawner[i].transform.position;
                int RandomIndex = Random.Range(0, EnemyPrefab.Count);
                GameObject en = PhotonNetwork.InstantiateRoomObject(EnemyPrefab[RandomIndex].name, EnemyPosition, Quaternion.identity);
                //GameObject en = PhotonNetwork.Instantiate(EnemyGameObject.name, EnemyPosition, Quaternion.identity);
                Enemy enemy = en.GetComponent<Enemy>();
                enemies.Add(enemy);
            }
        }
    }
}
