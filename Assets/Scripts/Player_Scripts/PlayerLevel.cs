using UnityEngine;
using System;

[Serializable]
public class PlayerLevel
{
    public float ValuePriceEx => valuePriceEx;
    public float ModifierPriceEx => modifierPriceEx;
    public int Level => level;
    public int MaxLevel => maxLevel;

    [SerializeField]
    private float valuePriceEx;

    [SerializeField]
    private float modifierPriceEx;

    [SerializeField]
    private int maxLevel;
    [SerializeField]
    private int level = 1;

    public void LevelUP()
    {
        if(level < maxLevel)
        {
            level++;
            valuePriceEx += modifierPriceEx;
        }
    }

    public PlayerLevel(float valuePriceEx, float modifierPriceEX, int level, int maxLevel)
    {
        this.valuePriceEx = valuePriceEx;
        this.modifierPriceEx = modifierPriceEX;
        this.level = level;
        this.maxLevel = maxLevel;
    }
}
