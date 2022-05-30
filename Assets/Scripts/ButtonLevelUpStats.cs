using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonLevelUpStats : MonoBehaviour
{
    public TMP_Text LevelText;

    void Start()
    {
        
    }

    public void Push(int NumStat)
    {
        GameManager.Instance.LevelUpStats(NumStat,LevelText);
    }
}
