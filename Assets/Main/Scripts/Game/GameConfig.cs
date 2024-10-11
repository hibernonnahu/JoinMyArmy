using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig {
    public int[] maxChapterEnable =new int[]{5,1,0};//index book-1
    private static GameConfig instance;
    public static GameConfig GetInstance()
    {
        if (instance == null)
        {
            instance = new GameConfig();

        }
        return instance;
    }
}
