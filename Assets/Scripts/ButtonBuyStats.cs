using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonBuyStats : MonoBehaviour
{
    public int NumStat;
    
    [SerializeField] TMP_Text TextDescription;
    [SerializeField] TMP_Text TextLevel;
    [SerializeField] TMP_Text TextPrice;

    public void UpdateInformation(string description, string level, string price)
    {
        TextDescription.text = description;
        TextLevel.text = (level).ToString();
        TextPrice.text = price;
    }

    public void PushButton()
    {
        Store.Instance.UpdateStat(NumStat);
    }
    
}
