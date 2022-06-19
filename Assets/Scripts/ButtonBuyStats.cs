using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonBuyStats : MonoBehaviour
{
    private int NumStat;
    
    [SerializeField] TMP_Text TextDescription;
    [SerializeField] TMP_Text TextLevel;
    [SerializeField] TMP_Text TextPrice;
    [SerializeField] Image ImageStat;

    public void UpdateInformation(string description, string level, string price, Sprite SpriteStat)
    {
        TextDescription.text = description;
        TextLevel.text = (level).ToString();
        TextPrice.text = price;
        ImageStat.sprite = SpriteStat;
    }

    public void SetNumStat(int Num) => NumStat = Num;

    public void PushButton()
    {
        Store.Instance.UpdateStat(NumStat);
    }
    
}
