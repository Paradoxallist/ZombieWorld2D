using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Store : MonoBehaviour
{
    public static Store Instance;

    [SerializeField]
    private ButtonBuyStats buttonBuyStatsPrefab;
    [SerializeField] 
    private Transform content;
    [SerializeField]
    private TMP_Text textLevel;
    [SerializeField]
    private float modifireLevel;

    private Player myPlayer;
    private List<ButtonBuyStats> buttonBuyStatsList;

    private void Start()
    {
        if (Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
        buttonBuyStatsList = new List<ButtonBuyStats>();
        InstantiateButton();
        UpdateTextLevel();
    }

    public void SetMyPlayer(Player player)
    {
        myPlayer = player;
    }

    public void InstantiateButton()
    {
        for(int i = 0; i < myPlayer.Stats.Count; i++)
        {
            ButtonBuyStats buttonItem = Instantiate(buttonBuyStatsPrefab, content);
            buttonItem.SetNumStat(i);
            buttonBuyStatsList.Add(buttonItem);
            UpdateInfo(i);
        }
    }
    
    public void UpdateStat(int i)
    {
        if (myPlayer.CanBuyStat(i, modifireLevel))
        {
            myPlayer.LevelUpStat((StatType)i);
            UpdateInfo(i);
        }
    }

    private void UpdateInfo(int i)
    {
        buttonBuyStatsList[i].UpdateInformation(myPlayer,i,modifireLevel);
    }


    public void UpdateTextLevel()
    {
        textLevel.text = $"Level: {myPlayer.Level.Level} ({myPlayer.GetEx()} \\ {myPlayer.Level.ValuePriceEx})"; 
    }
}
