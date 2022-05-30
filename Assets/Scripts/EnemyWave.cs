using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    
    public void Spawn()
    {
        GameManager.Instance.InstEnemy();
    }
}
