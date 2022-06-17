using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarOverCharacter : MonoBehaviour
{
    public Slider slider;

    public void SetMaxValue(float maxValue, float value)
    {
        slider.maxValue = maxValue;
        slider.value = value;
    }

    public void SetValue(float hp)
    {
        slider.value = hp;
    }
}
