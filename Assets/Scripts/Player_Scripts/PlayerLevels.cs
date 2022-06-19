using UnityEngine;
using System;

[Serializable]
public class PlayerLevels
{
    [SerializeField]
    private float modifierPriceEX;
    public float ValuePriceEX => valuePriceEX;
    [SerializeField]
    private float valuePriceEX;

    [SerializeField]
    private int maxLevel;
    public int Level => level;
    private int level = 1;

    public void LevelUP()
    {
        if(level < maxLevel)
        {
            level++;
            valuePriceEX += modifierPriceEX;
        }
    }
}
