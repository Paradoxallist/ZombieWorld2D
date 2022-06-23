using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    [SerializeField]
    private GameObject InventoryPanel;
    [SerializeField]
    private GameObject GameInterface;
    [SerializeField]
    private GameObject ScorePanel;
    [SerializeField]
    private TMP_Text TextScore;

    [SerializeField]
    private InformationUpdate info;
    [SerializeField]
    private Store store;

    private bool inStore;
    private Player myPlayer;

    void Start()
    {
        inStore = false;
    }

    void Update()
    {
        if (myPlayer == null) { return; }
        TextScore.text = myPlayer.GetScore().ToString();
        if(Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab))
        {
            Push();
        }
    }

    public void Push()
    {
        inStore = !inStore;
        InventoryPanel.SetActive(inStore);
        store.UpdateTextLevel();
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
