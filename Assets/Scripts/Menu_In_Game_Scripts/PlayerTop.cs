using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class PlayerTop : MonoBehaviour
{
    void Start()
    {
        foreach (var text in GetComponentsInChildren<TMP_Text>())
        {
            text.text = "";
        }
    }

    public void SetText(List<Player> players)
    {
        Player[] top = players
            .OrderByDescending(p => p.Score)
            .ToArray();

        for (int i = 0; i < top.Length; i++)
        {
            if (top[i] != null)
                transform.GetChild(i).GetComponent<TMP_Text>().text = (i + 1) + ". " + top[i].photonView.Owner.NickName + "    " + top[i].Score;
        }
    }
}
