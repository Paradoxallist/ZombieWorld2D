using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject player;
    public float minX, minY, maxX, maxY;
    // Start is called before the first frame update
    void Start()
    {
        /*Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
        PhotonNetwork.Instantiate(player.name, randomPosition, Quaternion.identity);*/
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PushStart()
    {
        Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
        GameObject PlayerOb = PhotonNetwork.Instantiate(player.name, randomPosition, Quaternion.identity);
        Camera.main.GetComponent<CameraControl>().target = PlayerOb.transform;
    }
}
