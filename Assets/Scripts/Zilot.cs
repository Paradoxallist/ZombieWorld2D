using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Zilot : Player
{
    

    void Start()
    {
        StartPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayer();
    }

    public override void Attack()
    {
        //throw new System.NotImplementedException();
    }

    public void LevelUpStat(int NumStat)
    {

    }
}
