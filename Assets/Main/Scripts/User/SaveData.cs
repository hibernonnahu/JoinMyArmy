using System;
using System.Collections.Generic;

public class SaveData
{
    public int coins = 0;
    private Dictionary<int, int> enemyLocalLevel = new Dictionary<int, int>();//id level
    private Dictionary<string, int> other = new Dictionary<string, int>();//id level
    public static SaveData instance;

    public static SaveData GetInstance()
    {
        if (instance == null)
        {
            instance = new SaveData();
        }
        return instance;
    }
    public int GetValue(string name)
    {
        if (!other.ContainsKey(name))
        {
            return 0;
        }
        return other[name];
    }
    public void Save(string name, int value)
    {
        if (!other.ContainsKey(name))
        {
            other.Add(name, value);
        }
        else
        {
            other[name] = value;
        }
    }
    public int AddEnemyLocalLevel(int enemyId)
    {
        if (!enemyLocalLevel.ContainsKey(enemyId))
        {
            enemyLocalLevel.Add(enemyId, 1);
        }
        enemyLocalLevel[enemyId]++;
        return enemyLocalLevel[enemyId];
    }
    public int GetEnemyLocalLevel(int enemyId)
    {
        if (!enemyLocalLevel.ContainsKey(enemyId))
        {
            return 1;
        }
        return enemyLocalLevel[enemyId];
    }

    internal void SaveRam()
    {
        coins += CurrentPlaySingleton.GetInstance().coins;
        CurrentPlaySingleton.GetInstance().coins = 0;
        EventManager.TriggerEvent(EventName.UPDATE_GAME_COINS_TEXT);
    }
}

