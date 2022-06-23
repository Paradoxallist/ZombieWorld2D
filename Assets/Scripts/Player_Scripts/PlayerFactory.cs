using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player", order = 2)]
public class PlayerFactory : ScriptableObject
{
    [SerializeField]
    private PlayerLevel playerLevel;
    [SerializeField]
    private List<PlayerStat> stats;

    public PlayerLevel Level => playerLevel;

    public List<PlayerStat> Stats => stats;
}


