using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    public Player myPlayer;
    [SerializeField]
    private ButtonBuyStats buttonBuyStatsPrefab;
    [SerializeField] 
    private Transform content;
    private List<ButtonBuyStats> buttonBuyStatsList;

    public static Store Instance;

    private void Start()
    {
        if (Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
        buttonBuyStatsList = new List<ButtonBuyStats>();
        InstButton();
    }

    public void SetMyPlayer(Player player)
    {
        myPlayer = player;
    }

    public void InstButton()
    {
        //myPlayer = GameManager.Instance.MyPlayer;
        for(int i = 0; i < myPlayer.Stats.Count; i++)
        {
            ButtonBuyStats buttonItem = Instantiate(buttonBuyStatsPrefab, content);
            buttonItem.NumStat = i;
            buttonBuyStatsList.Add(buttonItem);
            UpdateInfo(i);
        }
    }
    
    public void UpdateStat(int N)
    {
        if (myPlayer.Score >= myPlayer.Stats[N].ValuePrice && myPlayer.Stats[N].Level < myPlayer.Stats[N].MaxLevel)
        {
            myPlayer.Score -= myPlayer.Stats[N].ValuePrice;
            myPlayer.LevelUpStat((StatType)N);
            for (int i = 0; i < myPlayer.Stats.Count; i++)
            {
                UpdateInfo(i);
            }
        }
    }

    private void UpdateInfo(int i)
    {
        buttonBuyStatsList[i].UpdateInformation(myPlayer.Stats[i].StatType.ToString() + "(" + myPlayer.Stats[i].Value + ")" , myPlayer.Stats[i].Level + "(" + myPlayer.Stats[i].MaxLevel + ")", myPlayer.Stats[i].ValuePrice.ToString());
    }

}
