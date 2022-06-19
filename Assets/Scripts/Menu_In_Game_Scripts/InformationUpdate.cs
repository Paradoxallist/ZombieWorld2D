using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InformationUpdate : MonoBehaviour
{

    public BarCharacter HpBar;
    public TMP_Text TextHpDescription;
    public BarCharacter ManaBar;
    public TMP_Text TextManaDescription;
    public BarCharacter ExBar;
    public TMP_Text TextLevelDescription;

    public TMP_Text TextWave;

    private Player myPlayer;

    public void SetMyPlayer(Player player)
    {
        myPlayer = player;
    }
    void Update()
    {
        if (myPlayer != null)
        {
            TextHpDescription.text = ((int)Mathf.Clamp((myPlayer.GetHp()),0f,myPlayer.GetMaxHp())).ToString();
            HpBar.SetMaxValue(myPlayer.GetMaxHp(), myPlayer.GetHp());
            TextManaDescription.text = ((int)Mathf.Clamp((myPlayer.GetMana()), 0f, myPlayer.GetMaxMana())).ToString();
            ManaBar.SetMaxValue(myPlayer.GetMaxMana(), myPlayer.GetMana());
            ExBar.SetMaxValue(myPlayer.Levels.ValuePriceEX,myPlayer.Ex);
            TextLevelDescription.text = myPlayer.Levels.Level.ToString();
        }
        TextWave.text = "Wave:" + GameManager.Instance.Wave + "(" + (int)(GameManager.Instance.TimeBetweenWaves + GameManager.Instance.Wave * 3 - GameManager.Instance.timeToWave) + ")";
    }
}
