using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    private bool inStore;
    public GameObject Store;
    public GameObject GameInterface;
    public GameObject ScorePanel;
    public TMP_Text TextScore;

    public InformationUpdate info;
    public Store store;

    private Player myPlayer;

    void Start()
    {
        inStore = false;
    }

    void Update()
    {
        if (myPlayer == null) { return; }
        TextScore.text = myPlayer.Score.ToString();
        if(Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab))
        {
            Push();
        }
    }

    public void Push()
    {
        inStore = !inStore;
        Store.SetActive(inStore);
        GameInterface.SetActive(!inStore);
    }
    
    public void SetMyPlayer(Player player)
    {
        myPlayer = player;
        info.SetMyPlayer(myPlayer);
        store.SetMyPlayer(myPlayer);
        ScorePanel.SetActive(true);
    }
}
