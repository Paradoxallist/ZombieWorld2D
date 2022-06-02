using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChooseHero : MonoBehaviour
{
    public TMP_Text text;

    public void Push(string NameHero)
    {
        text.text = NameHero;
    }
}
