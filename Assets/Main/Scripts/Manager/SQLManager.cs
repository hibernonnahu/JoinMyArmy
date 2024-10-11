using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SQLManager : MonoBehaviour
{
    private string mainURL = "https://runnerbuilder.dx.am/joinmyarmy";
    private int userid = -1;
    public bool newUser = false;
    public bool alwaysNewGame = false;
    public bool noStart = false;
    public bool localSave = false;
    private int timesPlayed = 0;
    public bool forceMenu = false;
    public bool clearSave = false;
    // Start is called before the first frame update
    void Start()
    {
        if (clearSave)
        {
            PlayerPrefs.DeleteAll();
        }
        if (!noStart)
        {
            EventManager.StartListening(EventName.SAVE_USER_DELAY, OnDelaySave);
            DontDestroyOnLoad(gameObject);

            int id = PlayerPrefs.GetInt("userid", -1);
            if (alwaysNewGame)
            {
                id = -1;
                PlayerPrefs.SetString("LocalSave", "");
            }
            timesPlayed = PlayerPrefs.GetInt("timesPlayed", 0);
            timesPlayed++;
            PlayerPrefs.SetInt("timesPlayed", timesPlayed);
            Debug.Log("timesPlayed " + timesPlayed);


        newUser = false;

            Debug.Log("id " + id);

            if (!forceMenu&&(newUser || (id == -1 && !localSave) || timesPlayed == 1))
            {

                //Debug.Log("init " + newUser + " " + id+" "+ localSave + " " + (timesPlayed == 1));
                SaveData.GetInstance().SetNewPlayer();

                if (!alwaysNewGame)
                {
                    StartCoroutine(GenerateID());
                }
                FindObjectOfType<InitGameController>().OnStart("Game");
                SaveData.GetInstance().SaveNewMetric(SaveDataKey.INSTALL_TIMESTAMP, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString() + "000");
                SaveGenericMetrics();
            }
            else
            {
                userid = id;
                if (localSave)
                {
                    Init(PlayerPrefs.GetString("LocalSave", ""));
                }
                else
                {
                    StartCoroutine(LoadUser(userid));
                }


            }
        }
    }

    private void SaveGenericMetrics()
    {
        SaveData.GetInstance().SaveNewMetric(SaveDataKey.LAST_LOGIN_TIMESTAMP, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString() + "000");
        SaveData.GetInstance().SaveNewMetric(SaveDataKey.VERSION, Application.version);
        SaveData.GetInstance().SaveNewMetric(SaveDataKey.URL, Application.absoluteURL.Replace("-","").Replace("|", "").Replace("&", "").Replace("_", "").Replace("%", "").Replace(":", "").Replace("/", ""));
        SaveData.GetInstance().SaveNewMetric(SaveDataKey.TIMES_PLAYED, timesPlayed.ToString());
    }

    IEnumerator GenerateID()
    {
        if (!localSave)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(mainURL + "/GenerateID.php", ""))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success || www.downloadHandler.text == "-1")
                {
                    Debug.Log(www.error);
                }
                else
                {

                    int userid = -1;
                    if (int.TryParse(www.downloadHandler.text, out userid))
                    {
                        this.userid = userid;
                        PlayerPrefs.SetInt("userid", userid);
                        SaveData.GetInstance().SetNewPlayer();
                        SaveUser();
                    }

                }
            }
        }


    }
    private void OnDelaySave(EventData arg0)
    {
        LeanTween.cancel(gameObject);
        LeanTween.delayedCall(gameObject, 2, SaveUser);
    }
    public void SaveUser()
    {
        StartCoroutine(SaveUser(SaveData.GetInstance().Export()));
    }
    public IEnumerator SaveUser(string json)
    {
        PlayerPrefs.SetString("LocalSave", json);
        if (userid != -1)
        {

            var list = new List<string>();
            list.Add(json);
            int secret = Utils.CreateSecret(list);
            WWWForm form = new WWWForm();
            form.AddField("userid", userid);
            form.AddField("data", json);
            form.AddField("secret", secret);
            //form.AddField("userid", SQLUserData.GetInstance().logInID);
            //form.AddField("icon", icon);
            //form.AddField("secret", Utils.CreateSecret(new List<string>() { alias, icon.ToString() }));
            using (UnityWebRequest www = UnityWebRequest.Post(mainURL + "/SaveData.php", form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success || www.downloadHandler.text == "-1")
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log(www.downloadHandler.text);
                }
                //        WWW test = new WWW(mainURL + "/SaveData.php", form);
                //yield return test;
                //string result = test.text;
                //Debug.Log("savedata resutl " + result);
            }
        }
    }
    public IEnumerator LoadUser(int id)
    {
        if (userid != -1)
        {
            var list = new List<string>();
            list.Add(id.ToString());
            int secret = Utils.CreateSecret(list);
            WWWForm form = new WWWForm();
            form.AddField("userid", id);
            form.AddField("secret", secret);

            using (UnityWebRequest www = UnityWebRequest.Post(mainURL + "/LoadData.php", form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success || www.downloadHandler.text == "-1")
                {
                    Debug.Log(www.error);
                }
                else if (www.downloadHandler.text == "-2")
                {
                    Debug.Log("secret error");
                }
                else
                {
                    //Debug.Log("load " + www.downloadHandler.text);

                    Init(www.downloadHandler.text);
                }
            }


        }
    }
    private void Init(string toParse)
    {
        //Debug.Log("load len " + toParse.Length);
        if (toParse == "")
        {
            SaveData.GetInstance().SetNewPlayer();
        }
        else
        {

            SaveData.GetInstance().Import(toParse, true);
        }
        string old = SaveData.GetInstance().GetMetric(SaveDataKey.LAST_LOGIN_TIMESTAMP, "");
        string currentTime = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString() + "000";
        if (old != "")
        {
            float milSecondsOld = float.Parse(old);
            float milSecondsNew = float.Parse(currentTime);
            SaveData.GetInstance().SaveMetric(SaveDataKey.MINUTES_SINCE_LAST_CONEXION, (Math.Floor((milSecondsNew - milSecondsOld) / 1000)).ToString("f0"));
        }
        SaveGenericMetrics();

        // 

        FindObjectOfType<InitGameController>().OnStart("Main Menu");
    }
    public IEnumerator LoadUsers(Action<string> onComplete)
    {

        WWWForm form = new WWWForm();

        using (UnityWebRequest www = UnityWebRequest.Post(mainURL + "/LoadUsers.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success || www.downloadHandler.text == "-1")
            {
                Debug.Log(www.error);
            }
            else
            {

                onComplete(www.downloadHandler.text);

            }
        }



    }
}

