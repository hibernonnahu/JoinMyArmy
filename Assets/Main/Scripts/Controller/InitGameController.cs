using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitGameController : MonoBehaviour
{
    public bool expired = false;
    // Start is called before the first frame update
    public void CheckExpired(Action next)
    {
       

        if (!expired)
        {
         // Debug.Log( new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString());
            if (new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds() > 1732935600)
            {
                PlayerPrefs.SetInt("Expired", 1);
                OnExpired();
            }
            else
            {
                next();
            }
        }
        else
        {
            OnExpired();
        }
    }
    public void OnStart(string scene)
    {
        CheckExpired(() =>
        {
            AdsController.Init();
            LeanTween.delayedCall(gameObject, 2, () => { SceneManager.LoadScene(scene); });
        });
    }

    private void OnExpired()
    {
        FindObjectOfType<Text>().text = "Demo has expired";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
