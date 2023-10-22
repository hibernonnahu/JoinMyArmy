using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitGameController : MonoBehaviour
{
    private bool expired = false;
    // Start is called before the first frame update
    //public void OnStart(string scene)
    //{
    //    expired = PlayerPrefs.GetInt("Expired", 0) == 1;

    //    if (!expired)
    //    {
    //        //FindObjectOfType<Text>().text = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString();
    //        if (new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds() > 1701313200)
    //        {
    //            PlayerPrefs.SetInt("Expired", 1);
    //            OnExpired();
    //        }
    //        else
    //        {
    //            SceneManager.LoadScene(scene);
    //        }
    //    }
    //    else
    //    {
    //        OnExpired();
    //    }
    //}
    public void OnStart(string scene)
    {
        LeanTween.delayedCall(gameObject, 2, () => { SceneManager.LoadScene(scene); });
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
