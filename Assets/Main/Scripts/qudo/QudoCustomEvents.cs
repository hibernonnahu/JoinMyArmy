using QUDOSDK.Users;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QudoCustomEvents : MonoBehaviour
{
    public static string ACHIEVEMENT = "achievement";
    public static string HIGHSCORE = "highscore";
    public bool isLogging;
    static QudoCustomEvents instance;
    QUDOUser user;
    private void Awake()
    {
        if (instance == null)
        {
           
            instance = this;
            DontDestroyOnLoad(gameObject);
            QUDOUser.OnAnyUserLoggedIn += OnQudoUserLoggin;
            QUDOUser.OnAnyUserLoggedOut += OnQudoUserLoggOut;

            EventManagerGlobal.StartListening(HIGHSCORE, OnHighScore);
            EventManagerGlobal.StartListening(ACHIEVEMENT, OnAchievement);

        }
        else
        {
            Destroy(gameObject);
        }
    }

    internal bool IsLogIn()
    {
        return user != null;
    }

    private void OnAchievement(EventData arg0)
    {
        Achievement(arg0.stringData);
    }

    private void OnHighScore(EventData arg0)
    {
        Highscore(arg0.stringData, arg0.longData);
    }

    private void OnQudoUserLoggOut(QUDOUser obj)
    {
        user = null;
        EventManagerGlobal.TriggerEvent("analitycs_clear");
        
       
    }

    private void OnQudoUserLoggin(QUDOUser user)
    {
        if (user != null)
        {
            isLogging = true;
           
            this.user = user;

        }
    }
    private void Highscore(string highscorealias, long score)
    {

        if (user != null)
        {


            user.SubmitHighscore(highscorealias, score, true);
        }
    }
    private void Achievement(string achievementalias)
    {

        if (user != null )
        {
           
#if !UNITY_EDITOR
                user.GiveAchievement(achievementalias);
#else
                Debug.Log("achievement " + achievementalias);
#endif
            
        }

    }
    // Update is called once per frame
    void Update()
    {

    }

   
    private void OnDestroy()
    {
        if (instance == this)
        {
            QUDOUser.OnAnyUserLoggedIn -= OnQudoUserLoggin;
            QUDOUser.OnAnyUserLoggedOut -= OnQudoUserLoggOut;
            EventManagerGlobal.StopListening("highscore", OnHighScore);
            EventManagerGlobal.StopListening("achievement", OnAchievement);
        }

    }
}
