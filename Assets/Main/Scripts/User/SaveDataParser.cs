using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataParser
{
    public int coins = 0;
    public Dictionary<int, int> enemyLocalLevel = new Dictionary<int, int>();//id level
    public Dictionary<string, int> other = new Dictionary<string, int>();//id level
    public Dictionary<string, string> metric = new Dictionary<string, string>();


    public SaveDataParser(string data)
    {
       // Debug.Log(data);
        data = data.Substring(1, data.Length - 2);

        data = data.Replace("\"", "");
        string[] split = data.Split("},");
        foreach (var item in split)
        {
            string[] subSplit = item.Split(':', 2);
            if (subSplit[0] == "coins")//esto es kk pero bue
            {
                coins = int.Parse(subSplit[1]);
            }
            else if (subSplit[0] == "enemyLocalLevel")//esto es kk pero bue
            {
                if (subSplit[1].Length > 1)
                    enemyLocalLevel = ParseInt2Int(subSplit[1]);
            }
            else if (subSplit[0] == "other")//esto es kk pero bue
            {
                other = ParseString2Int(subSplit[1]);
            }
            else if (subSplit[0] == "metric")//esto es kk pero bue
            {
                metric = ParseString2String(subSplit[1]);
            }
        }
    }

    //private Dictionary<int, int> enemyLocalLevel = new Dictionary<int, int>();//id level
    //private Dictionary<string, int> other = new Dictionary<string, int>();//id level

    private Dictionary<int, int> ParseInt2Int(string code)
    {
        code = code.Substring(1, code.Length - 1);
        Dictionary<int, int> dic = new Dictionary<int, int>();
        string[] split = code.Split(',');
        foreach (var item in split)
        {
            string[] tuple = item.Split(':');
            dic.Add(int.Parse(tuple[0]), int.Parse(tuple[1]));
        }

        return dic;
    }

    private Dictionary<string, int> ParseString2Int(string code)
    {
        code = code.Substring(1, code.Length - 1);
        Dictionary<string, int> dic = new Dictionary<string, int>();
        if (code != "")
        {
            string[] split = code.Split(',');
            foreach (var item in split)
            {
                string[] tuple = item.Split(':');
                dic.Add(tuple[0], int.Parse(tuple[1]));
            }

        }
        return dic;
    }

    private Dictionary<string, string> ParseString2String(string code)
    {
        code = code.Substring(1, code.Length - 1);
        Dictionary<string, string> dic = new Dictionary<string, string>();
        string[] split = code.Split(',');
        foreach (var item in split)
        {
            string[] tuple = item.Split(':');
            dic.Add(tuple[0], tuple[1]);
        }

        return dic;
    }
}
