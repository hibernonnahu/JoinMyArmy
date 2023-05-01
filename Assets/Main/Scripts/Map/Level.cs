using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level  {
    public int[] main;
    public int[] obstacles;
    public int[] enemies;
    public int floor = 0;
    public int time = -1;
    public int waveTime = 10;
}
