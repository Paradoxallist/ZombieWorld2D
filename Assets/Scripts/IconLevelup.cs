using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconLevelup : MonoBehaviour
{
    public List<string> StatsFile;
    public List<int> LevelStats;
    public Player pl;
    public GameManager gameManager;

    // Start is called before the first frame update
    public void StartSettings()
    {
        gameManager = FindObjectOfType<GameManager>();
        foreach (var file in StatsFile)
        {
            LevelStats.Add(1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetStats(int Level, int NumStats)
    {
        LevelStats[NumStats] = Level;
    }
}
