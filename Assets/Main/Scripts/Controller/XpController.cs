using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpController
{
    private const int MAX_LEVEL = 20;
    private int currentXp = 0;
    public int CurrentXp { get { return currentXp; } }
    private const int INITIAL_XP = 10;
    private int[] levelXPSimple;
 
    public XpController()
    {
        InitLvlXP();
    }

    private void InitLvlXP()
    {
        float next = INITIAL_XP;
       
        levelXPSimple = new int[MAX_LEVEL];
        for (int i = 0; i < MAX_LEVEL; i++)
        {
            levelXPSimple[i] = Mathf.FloorToInt(next);
            next *= 1.4f;
        }
    }

    public int AddXp(int xp, int currentLevel)//returns lvl
    {
        currentXp += xp;
        if (currentXp > GetLevelXPSimple(currentLevel))
        {
            return UpdateCurrentLevel(currentLevel);
        }
        return currentLevel;
    }

    private int GetLevelXPSimple(int currentLevel)
    {
        return levelXPSimple[currentLevel - 1];
    }
    public float GetXpPercent(int level)
    {
        return (float)currentXp / GetLevelXPSimple(level);
    }
   

    private int UpdateCurrentLevel(int oldCurrentLevel)
    {
        if (oldCurrentLevel < MAX_LEVEL)
        {
            currentXp -= GetLevelXPSimple(oldCurrentLevel);
            oldCurrentLevel++;
        }
        else
        {
            currentXp = GetLevelXPSimple(oldCurrentLevel);         
        }
        return oldCurrentLevel;
    }

    public void ForceXp(int currentXp)
    {
        this.currentXp = currentXp;
    }
}
