using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    public List<GameObject> EnemyPrefab;
    public List<GameObject> BossPrefab;
    private int bossIndex;

    public float minX, minY, maxX, maxY;

    public List<Player> players = new();
    private List<int> diePlayersID = new();
    public List<BaseEnemy> enemies = new();
    public PhotonView PV;
    public List<Transform> Spawner;

    public int Wave;
    public float TimeBetweenWaves;
    private float timeToWave;
    public float TimeToWave => timeToWave;

    private bool startSpawnWave;

    private void Awake()
    {
        Wave = 0;
        bossIndex = 0;  
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
        if(Wave + TimeBetweenWaves < timeToWave && startSpawnWave && PhotonNetwork.IsMasterClient)
        {
            SpawnWave();
        }
        /*if (PhotonNetwork.IsMasterClient && !startSpawnWave && (TimeBetweenWaves + Wave * 3 - timeToWave) < 0)
        {
            SpawnWave();
        }*/
    }

    public void AddPlayer(Player player)
    {
        players.Add(player);
        player.SetID(players.Count - 1);
    }

    public void UpdateHpBarEnemy(BaseEnemy enemy)
    {
        enemy.CheckAlive();
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

    [PunRPC]
    public void MasterClientSetID()
    {
        for (int i = 0; i < players.Count; i++) {
            PV.RPC("ClientSetID", RpcTarget.MasterClient,i);
        }
    }

    [PunRPC]
    public void ClientSetID(int ID)
    {
        players[ID].SetID(ID);
    }

    public void SpawnWave()
    {
        startSpawnWave = true;
        Wave++;
        for (int i = 0;i < Wave; i++)
        {
            Invoke(nameof(InstantiateEnemy), i);
        }
        if (Wave == 15)
            InstantiateBoss();
        timeToWave = 0;
        PV.RPC("SetInfoWave", RpcTarget.AllBuffered, Wave);
        Resurrection();
        //Invoke(nameof(SpawnWave), (Wave + TimeBetweenWaves));
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
            if (players[diePlayersID[i]] != null)
            {
                PV.RPC("RespawnPositionPlayerPun", RpcTarget.AllBuffered, diePlayersID[i]);
                players[diePlayersID[i]].Resurrect();
            }
        }
        diePlayersID.Clear();
    }

    [PunRPC]
    public void RespawnPositionPlayerPun(int i)//Peredelat tochno !!!!!!!
    {
        Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
        players[i].transform.position = randomPosition;
    }

    public void InstantiateEnemy()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < Spawner.Count; i++)
            {
                Vector3 EnemyPosition = Spawner[i].transform.position;
                int RandomIndex = Random.Range(0, EnemyPrefab.Count);
                GameObject en = PhotonNetwork.InstantiateRoomObject(EnemyPrefab[RandomIndex].name, EnemyPosition, Quaternion.identity);
                BaseEnemy enemy = en.GetComponent<BaseEnemy>();
                enemy.LevelUpWave(Wave);
                enemies.Add(enemy);
            }
        }
    }

    public void InstantiateBoss()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int RandomIndex = Random.Range(0, Spawner.Count);
            Vector2 BossPosition = Spawner[RandomIndex].transform.position;
            GameObject boss = PhotonNetwork.InstantiateRoomObject(BossPrefab[bossIndex].name, BossPosition, Quaternion.identity);
        }
    }


    public void EnemyDie(BaseEnemy enemy)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            enemies.Remove(enemy);
            if (enemies.Count == 0)
            {
                if(Wave + TimeBetweenWaves > timeToWave + 10)
                {
                    timeToWave = Wave + TimeBetweenWaves + Wave - 10;
                    PV.RPC("SynchronizingDataWave", RpcTarget.AllBuffered, Wave, timeToWave);
                }
            }
        }
    }
}
