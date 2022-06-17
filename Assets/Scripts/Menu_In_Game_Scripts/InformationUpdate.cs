using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InformationUpdate : MonoBehaviour
{
    public PlayerTop Top;

    [SerializeField] TMP_Text TextHpDescription;
    [SerializeField] TMP_Text TextDamageDescription;
    [SerializeField] TMP_Text TextSpeedDescription;

    [SerializeField] TMP_Text TextWave;

    private Player myPlayer;

    public void SetMyPlayer(Player player)
    {
        myPlayer = player;
    }

    // Update is called once per frame
    void Update()
    {
        Top.SetText(GameManager.Instance.players);
        if (myPlayer != null)
        {
            TextHpDescription.text = myPlayer.GetID() + "Hp - " + (int)myPlayer.Hp + "/" + myPlayer.GetPlayerStat(StatType.MaxHp).Value.ToString();
            TextDamageDescription.text = "Damage - " + (int)myPlayer.GetPlayerStat(StatType.Damage).Value;
            TextSpeedDescription.text = "Speed - " + myPlayer.GetPlayerStat(StatType.Speed).Value;
        }
        TextWave.text = "Wave:" + GameManager.Instance.Wave + "(" + (int)(GameManager.Instance.TimeBetweenWaves + GameManager.Instance.Wave * 3 - GameManager.Instance.timeToWave) + ")";
    }
}
