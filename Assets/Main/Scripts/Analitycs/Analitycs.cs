using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Analitycs : MonoBehaviour
{
    private SQLManager sqlManager;
    // Start is called before the first frame update
    void Start()
    {
        sqlManager = GetComponent<SQLManager>();
        StartCoroutine(sqlManager.LoadUsers(ParseUsers));
    }

    private void ParseUsers(string raw)
    {
        string path = Application.persistentDataPath + "/Users data";

        string[] users = raw.Split("%");
        for (int i = 0; i < users.Length; i++)
        {
            if (users[i] != "")
            {
                string[] data = users[i].Split("|");//0 data / 1 id / 2 timestamp
                Debug.Log("download user " + data[1]);
                Debug.Log( data[0]);
                string fullPath = path + "/" + data[1] + ".json";

                StreamWriter writer = new StreamWriter(fullPath, false);

                writer.Write(data[0]);
                writer.Close();
            }
        }
        
    }
}
