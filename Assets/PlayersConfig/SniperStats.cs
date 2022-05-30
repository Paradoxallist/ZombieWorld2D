using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SniperConfig", menuName = "Sniper", order = 1)]
public class SniperStats : ScriptableObject
{
    public List<LevelSniper> SniperLevels => sniperLevels;
    [SerializeField]
    private List<LevelSniper> sniperLevels;
}
[Serializable]
public struct LevelSniper
{
    public float Damage => damage;
    public float MaxHp => maxHp;
    public float Speed => speed;

    [SerializeField]
    private float damage;
    [SerializeField]
    private float maxHp;
    [SerializeField]
    private float speed;
}
