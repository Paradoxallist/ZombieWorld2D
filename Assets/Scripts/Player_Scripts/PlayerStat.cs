using UnityEngine;
using System;

[Serializable]
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
    private float modifierPrice;
    public float ValuePrice => valuePrice;
    [SerializeField]
    private float valuePrice;

    public int MaxLevel => maxLevel;

    [SerializeField]
    private int maxLevel;
    public int Level => level;
    [SerializeField]
    private int level;

    public Sprite SpriteStat => spriteStat;
    [SerializeField]
    private Sprite spriteStat;

    public void Update()
    {
        if (level < maxLevel)
        {
            level++;
            valuePrice += modifierPrice;
            if (modifierType == ModifierType.Add)
            {
                value += modifier;
            }
            else if (modifierType == ModifierType.Multiply)
            {
                value *= modifier;
            }
        }
    }

    public PlayerStat(float modifier, float value, int maxLevel, int level, StatType statType)
    {
        this.statType = statType;
        this.modifier = modifier;
        this.value = value;
        this.maxLevel = maxLevel;
        this.level = level;
    }

}

[Serializable]
public enum StatType
{
    MaxHp,
    HpRagen,
    MaxMana,
    ManaRegen,
    Damage,
    Speed,
    AttackSpeed
}

[Serializable]
public enum ModifierType
{
    Add,
    Multiply
}