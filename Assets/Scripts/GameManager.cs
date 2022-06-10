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
    public List<GameObject> EnemyPrefab;
    public int NumClass;
    public float minX, minY, maxX, maxY;

    public List<Player> players = new List<Player>();
    public List<Enemy> enemies = new List<Enemy>();
    public PhotonView PV;
    public static GameManager Instance;
    public List<Transform> Spawner;

    public Player MyPlayer;

    public PlayerTop Top;

    [SerializeField] TMP_Text TextHpDescription;
    [SerializeField] TMP_Text TextDamageDescription;
    [SerializeField] TMP_Text TextSpeedDescription;

    [SerializeField] TMP_Text TextWave;

    private int Wave;
    public float TimeBetweenWaves;
    private float timeToWave;

    private void Start()
    {
        Wave = 0;
        players = new List<Player>();
        if (Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    private void Update()
    {
        Top.SetText(players);
        if(MyPlayer != null)
        {
            TextHpDescription.text = "Hp - " + (int)MyPlayer.Hp + "/" + MyPlayer.GetPlayerStat(StatType.MaxHp).Value.ToString();
            TextDamageDescription.text = "Damage - " + (int)MyPlayer.GetPlayerStat(StatType.Damage).Value;
            TextSpeedDescription.text = "Speed - " + MyPlayer.GetPlayerStat(StatType.Speed).Value;
        }
        timeToWave += Time.deltaTime;
        TextWave.text = "Wave:" + Wave + "(" +(int)(TimeBetweenWaves + Wave - timeToWave)  +")"; 
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

    public void SetNumClass(int N)///player player
    {
        NumClass = N;
    }

    void Inicilization(GameObject create)
    {
        Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
        GameObject PlayerOb = PhotonNetwork.Instantiate(create.name, randomPosition, Quaternion.identity);
        Camera.main.GetComponent<CameraControl>().target = PlayerOb.transform;
        MyPlayer = PlayerOb.GetComponent<Player>();
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnWave();
        }
    }

    public void SpawnWave()
    {
        Wave++;
        for (int i = 0;i < Wave; i++)
        {
            Invoke("InstEnemy", i);
        }
        timeToWave = 0;
        PV.RPC("SetInfoWave", RpcTarget.AllBuffered, Wave);
        Invoke("SpawnWave", (Wave + TimeBetweenWaves));
    }

    [PunRPC]
    public void SetInfoWave(int _Wave)
    {
        timeToWave = 0;
        Wave = _Wave;
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
