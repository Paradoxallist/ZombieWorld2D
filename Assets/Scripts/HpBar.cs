using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public float MaxHp;
    public Slider slider;

    public void SetMaxHealth(float maxHp)
    {
        slider.maxValue = maxHp;
        slider.value = maxHp;
    }

    public void SetHealth(float hp)
    {
        slider.value = hp;
    }
}
