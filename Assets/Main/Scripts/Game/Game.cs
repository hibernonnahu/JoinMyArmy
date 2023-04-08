using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        var loader = GetComponent<LevelJsonLoader>();
        loader.book = CurrentPlaySingleton.GetInstance().book;
        loader.chapter = CurrentPlaySingleton.GetInstance().chapter;
        loader.level = CurrentPlaySingleton.GetInstance().level;
        GetComponent<CharacterManager>().Init(2);
    }



    // Update is called once per frame
    void Update()
    {

    }
    internal void OnExitTrigger()
    {
        var stats = CurrentPlaySingleton.GetInstance();
        stats.level++;
        var next = Resources.LoadAll<TextAsset>("Maps/Campaign/Book" + stats.book + "/Chapter" + stats.chapter + "/Level" + stats.level);
        if (next == null || next.Length == 0)
        {
            print("FINISH");
        }
        else
        {
            stats.SaveParty();
            SceneManager.LoadScene("Game");
        }
    }
}
