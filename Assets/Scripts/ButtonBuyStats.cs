using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonBuyStats : MonoBehaviour
{
    [SerializeField] 
    private TMP_Text TextDescription;
    [SerializeField] 
    private TMP_Text TextLevel;
    [SerializeField] 
    private TMP_Text TextPrice;
    [SerializeField] 
    private Image ImageStat;

    private int numStat;

    public void UpdateInformation(Player myPlayer, int i, float modifireLevel)
    {
        TextDescription.text = myPlayer.Stats[i].StatType.ToString() + $"({Mathf.Round(myPlayer.Stats[i].Value * 100) * 0.01f})";
        TextLevel.text = "lvl:" + myPlayer.Stats[i].Level + $"({(myPlayer.Level.Level * 2 < myPlayer.Stats[i].MaxLevel ? myPlayer.Level.Level * modifireLevel : myPlayer.Stats[i].MaxLevel)})";
        TextPrice.text = myPlayer.Stats[i].ValuePrice.ToString() + " gold";
        ImageStat.sprite = myPlayer.Stats[i].SpriteStat;
    }

    public void SetNumStat(int Num) => numStat = Num;

    public void UpdateStoreStat()
    {
        Store.Instance.UpdateStat(numStat);
    }

}
