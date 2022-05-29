using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperIcons : IconLevelup
{
    public Sniper sniper;

    void Start()
    {
        StartSettings();
        pl = sniper.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AttackDamageLevelup()
    {


    }
}
