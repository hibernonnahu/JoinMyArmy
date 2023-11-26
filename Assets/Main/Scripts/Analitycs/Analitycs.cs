
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class Analitycs : MonoBehaviour
{
    private SQLManager sqlManager;

    private SaveData[] savedata;
    public SaveData[] Savedata { get { return savedata; } }

    private string[] allMetricKeys;
    private string[] allIntKeys;
    public AnalitycsPrefab prefab;
    public Transform container;
    // Start is called before the first frame update
    void Start()
    {
        sqlManager = GetComponent<SQLManager>();
        StartCoroutine(sqlManager.LoadUsers(ParseUsers));
    }

    private void ParseUsers(string raw)
    {
        List<SaveData> savedataList = new List<SaveData>();
        string path = Application.persistentDataPath + "/Users data";
        Debug.Log(path);
        string[] users = raw.Split("%");
        for (int i = 0; i < users.Length; i++)
        {
            if (users[i] != "")
            {
                string[] data = users[i].Split("|");//0 data / 1 id / 2 timestamp
                if(i==24)
                {
                    Debug.Log("");
                }
                Debug.Log(users[i].Length+" i"+i);
                Debug.Log("download user " + data[1]);
                Debug.Log(data[0]);
                string fullPath = path + "/" + data[1] + ".json";

                StreamWriter writer = new StreamWriter(fullPath, false);

                writer.Write(data[0]);
                SaveData sd = new SaveData();
                sd.Import(data[0]);
                savedataList.Add(sd);

                writer.Close();
            }
        }
        savedata = savedataList.ToArray();
        UpdateKeys();
        CreatePrefabs();
        FindObjectOfType<AnalitycsFilter>().LoadPrefabs();
    }

    private void CreatePrefabs()
    {
        foreach (var key in allMetricKeys)
        {
            var analitycsItem=GameObject.Instantiate<AnalitycsPrefab>(prefab, container);
            analitycsItem.Init(this, key);
        }
       
        foreach (var key in allIntKeys)
        {
            var analitycsItem = GameObject.Instantiate<AnalitycsPrefab>(prefab, container);
            analitycsItem.Init(this, key,false);
        }
    }

    public void QueryMetric(string key)
    {
        Debug.ClearDeveloperConsole();
        foreach (var item in savedata)
        {
            Debug.Log(item.GetMetric(key, ""));
        }
    }
    private void UpdateKeys()
    {
        List<string> tmp = new List<string>();
        List<string> tmpInt = new List<string>();
        foreach (var item in savedata)
        {
            foreach (var key in item.GetAllMetricKeys())
            {
                if (!tmp.Contains(key))
                {
                    tmp.Add(key);
                }
            }
            foreach (var key in item.GetAllIntKeys())
            {
                if (!tmpInt.Contains(key))
                {
                    tmpInt.Add(key);
                }
            }
        }
        allMetricKeys = tmp.ToArray();
        allIntKeys = tmpInt.ToArray();

        System.Array.Sort(allMetricKeys,
            (a, b) => { return a.CompareTo(b); });
        System.Array.Sort(allIntKeys,
           (a, b) => { return a.CompareTo(b); });
    }
}
