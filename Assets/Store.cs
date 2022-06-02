using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{

    public void InstButton()
    {

    }
    
    public void UpdateStat(int N)
    {
        GameManager.Instance.MyPlayer.LevelUpStat((StatType)N);
    }

}
