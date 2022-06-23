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


    public float Value => value;
    [SerializeField]
    private float value;


    public float Modifier => modifier;
    [SerializeField]
    private float modifier;

    public float ValuePrice => valuePrice;
    [SerializeField]
    private float valuePrice;

    [SerializeField]
    private float modifierPrice;

    public int MaxLevel => maxLevel;

    [SerializeField]
    private int maxLevel;
    public int Level => level;
    [SerializeField]
    private int level = 0;

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

    /*public float GetValue()
    {
        
    }*/

    public PlayerStat(float modifier, float valuePrice, float value, int  maxLevel, int level, StatType statType,Sprite spriteStat)
    {
        this.statType = statType;
        this.modifier = modifier;
        this.valuePrice = valuePrice;   
        this.value = value;
        this.maxLevel = maxLevel;
        this.level = level;
        this.spriteStat = spriteStat;
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
    AttackSpeed,
    CostAbilityOne,
    CooldownAbilityOne,
    CostAbilityTwo,
    CooldownAbilityTwo
}

[Serializable]
public enum ModifierType
{
    Add,
    Multiply
}