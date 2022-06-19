using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item", order = 1)]
public class Item : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public string Description;
}
