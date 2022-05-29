using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAgr : MonoBehaviour
{
    public List<Player> Players = new List<Player>();
    public Player victim;
    public bool agr = false;
    void Update()
    {
        if(victim == null && agr)
            SearchVictim();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            Player p = coll.GetComponent<Player>();
            if (p != null)
            {
                Players.Add(p);
            }
            if(victim == null)
            {
                victim = p;
                agr = true; 
            }
        }
    }
    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            Player player = coll.GetComponent<Player>();
            if (player != null)
            {
                if (player == victim)
                {
                    agr = false;
                    SearchVictim();
                }
                Players.Remove(player);
            }
        }
    }
    private void SearchVictim()
    {
        Player NearPlaer = null;
        float distanse = 10000000;
        foreach (Player player in Players)
        {
            float dist = Vector2.Distance(transform.position,player.transform.position);
            if(dist < distanse)
            {
                distanse = dist;
                NearPlaer = player;
            }
        }
        if (NearPlaer != null)
        {
            victim = NearPlaer;
            agr = true;
        }
    }
}
