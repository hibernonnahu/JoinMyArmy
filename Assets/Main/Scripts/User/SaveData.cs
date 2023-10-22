using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public int coins = 0;
    private Dictionary<int, int> enemyLocalLevel = new Dictionary<int, int>();//id level
    private Dictionary<string, int> other = new Dictionary<string, int>();//id level
    private Dictionary<string, string> metric = new Dictionary<string, string>();
    private static SaveData instance;

    public static SaveData GetInstance()
    {
        if (instance == null)
        {
            instance = new SaveData();

        }
        return instance;
    }
    public void SetNewPlayer()
    {
        //TODO do this only if is a new player
        for (int i = 1; i < 5; i++)
        {
            instance.AddEnemyLocalLevel(12);
        }
        instance.coins = 100;
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

    public void SaveNewMetric(string name, string value)
    {
        if (!metric.ContainsKey(name))
        {
            metric.Add(name, value);
        }
    }

    public void SaveMetric(string name, string value)
    {
        if (!metric.ContainsKey(name))
        {
            metric.Add(name, value);
        }
        else
        {
            metric[name] = value;
        }
    }
    public string GetMetric(string name,string returnValue)
    {
        if (!metric.ContainsKey(name))
        {
            return returnValue;
        }
        return metric[name];
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

    internal void SaveRam(bool updateText = true)
    {
        coins += CurrentPlaySingleton.GetInstance().coins;
        CurrentPlaySingleton.GetInstance().coins = 0;
        if (updateText)
            EventManager.TriggerEvent(EventName.UPDATE_GAME_COINS_TEXT);


    }

    public string Export()
    {
        bool first = true;
        string data = "{";
        data += "\"enemyLocalLevel\":{";
        foreach (var item in instance.enemyLocalLevel)
        {
            if (!first)
            {
                data += ",";
            }
            else
            {
                first = false;
            }
            data += "\"" + item.Key + "\":" + item.Value;
        }
        data += "},";

        first = true;
        data += "\"other\":{";
        foreach (var item in instance.other)
        {
            if (!first)
            {
                data += ",";
            }
            else
            {
                first = false;
            }
            data += "\"" + item.Key + "\":" + item.Value;
        }
        data += "},";

        first = true;
        data += "\"metric\":{";
        foreach (var item in instance.metric)
        {
            if (!first)
            {
                data += ",";
            }
            else
            {
                first = false;
            }
            data += "\"" + item.Key + "\":\"" + item.Value + "\"";
        }
        data += "},";

        data += "\"coins\":" + coins + "}";

        return data;
    }

    internal void Import(string data)
    {
        SaveDataParser sdp = new SaveDataParser(data);
        coins = sdp.coins;
        enemyLocalLevel = sdp.enemyLocalLevel;
        other = sdp.other;
        metric = sdp.metric;
        if (metric.ContainsKey(SaveDataKey.ARMY))
            CurrentPlaySingleton.GetInstance().UpdateArmy(metric[SaveDataKey.ARMY]);
    }
}

