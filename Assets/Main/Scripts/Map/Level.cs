using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level  {
    public int[] main;
    public int[] obstacles;
    public int[] enemies;
    public int[] teamEnemiesID=new int[] { 1,0};
    public int floor = 0;
    public int time = -1;
    public int waveTime = 10;
    public string storyJsonFileName = "";
}
