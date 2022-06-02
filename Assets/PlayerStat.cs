using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStat
{
    [SerializeField]
    private StatType statType;
    public StatType StatType => statType;
    [SerializeField]
    private ModifierType modifierType;
    [SerializeField]
    private float modifier;

    public float Value => value;
    [SerializeField]
    private float value;

    [SerializeField]
    private int MaxLevel;
    private float level;

    public void Update()
    {
        level++;
        if(modifierType == ModifierType.Add)
        {
            value += modifier; 
        }else if(modifierType == ModifierType.Multiply)
        {
            value *= modifier;
        }
    }

    public PlayerStat(float modifier, float value, int maxLevel, float level, StatType statType)
    {
        this.statType = statType;
        this.modifier = modifier;
        this.value = value;
        MaxLevel = maxLevel;
        this.level = level;
    }

}

[System.Serializable]
public enum StatType
{
    MaxHp,
    Damage,
    Speed
}

[System.Serializable]
public enum ModifierType
{
    Add,
    Multiply
}