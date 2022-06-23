using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class BossEnemy : Enemy
{
    public void StartBoss()
    {
        StartEnemy();
    }

    void Update()
    {
        
    }
}
