using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig",menuName = "Enemies",order = 0)]
public class EnemyStats : ScriptableObject
{
    public List<Level> Levels => enemyLevels;
    [SerializeField]
    private List<Level> enemyLevels;
}
[Serializable]
public struct Level
{
    public float Damage => damage;
    public float Hp => hp;
    public float Speed => speed;

    [SerializeField]
    private float damage;
    [SerializeField]
    private float hp;
    [SerializeField]
    private float speed;
}
