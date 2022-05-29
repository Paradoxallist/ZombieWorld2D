using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOff : MonoBehaviour
{
    public GameObject[] On;
    public GameObject[] Off;

    public void Push()
    {
        for(int i = 0; i < On.Length; i++)
        {
            if (On[i] != null)
                On[i].SetActive(true);
        }
        for (int i = 0; i < Off.Length; i++)
        {
            if (Off[i] != null)
                Off[i].SetActive(false);
        }
    }
}
