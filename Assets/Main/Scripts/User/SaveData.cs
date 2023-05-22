using System;
using System.Collections.Generic;

public class SaveData
{
    public int coins = 0;
    private Dictionary<int, int> enemyLocalLevel = new Dictionary<int, int>();//id level
    private Dictionary<string, int> other = new Dictionary<string, int>();//id level
    private static SaveData instance;

    public static SaveData GetInstance()
    {
        if (instance == null)
        {
            instance = new SaveData();
            for (int i = 1; i < 5; i++)
            {
                instance.AddEnemyLocalLevel(12);
            }
        }
        return instance;
    }
    public int GetValue(string name, int returnValue = 0)
    {
        if (!other.ContainsKey(name))
        {
            return returnValue;
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

