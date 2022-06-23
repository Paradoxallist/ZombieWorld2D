using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InformationUpdate : MonoBehaviour
{
    [SerializeField]
    private BarCharacter HpBar;
    [SerializeField]
    private TMP_Text TextHpDescription;
    [SerializeField]
    private BarCharacter ManaBar;
    [SerializeField]
    private TMP_Text TextManaDescription;
    [SerializeField]
    private BarCharacter ExBar;
    [SerializeField]
    private TMP_Text TextLevelDescription;
    [SerializeField]
    private TMP_Text TextWave;

    private Player myPlayer;

    public void SetMyPlayer(Player player)
    {
        myPlayer = player;
    }
    void Update()
    {
        if (myPlayer != null)
        {
            TextHpDescription.text = ((int)Mathf.Clamp((myPlayer.GetHp()),0f,myPlayer.GetPlayerStat(StatType.MaxHp).Value)).ToString();
            HpBar.SetMaxValue(myPlayer.GetPlayerStat(StatType.MaxHp).Value, myPlayer.GetHp());
            TextManaDescription.text = ((int)Mathf.Clamp((myPlayer.GetMana()), 0f, myPlayer.GetPlayerStat(StatType.MaxMana).Value)).ToString();
            ManaBar.SetMaxValue(myPlayer.GetPlayerStat(StatType.MaxMana).Value, myPlayer.GetMana());
            ExBar.SetMaxValue(myPlayer.Level.ValuePriceEx,myPlayer.GetEx());
            TextLevelDescription.text = myPlayer.Level.Level.ToString();
        }
        TextWave.text = "Wave:" + GameManager.Instance.Wave + "(" + (int)(GameManager.Instance.TimeBetweenWaves + GameManager.Instance.Wave * 3 - GameManager.Instance.TimeToWave) + ")";
    }
}
