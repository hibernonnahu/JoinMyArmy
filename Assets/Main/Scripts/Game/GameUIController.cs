
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    public Text timeLeft;
    public Text nextWave;
    public Text enemiesRemain;
    private int lastCounter = -1;
    private int lastWaveCounter = -1;
    private bool ready = false;
    private bool wavesReady = false;

    private void Start()
    {
        EventManager.StartListening(EventName.EXIT_OPEN, OnExitOpen);
        EventManager.StartListening(EventName.ENEMIES_REMAIN, UpdateEnemiesRemain);
        EventManager.StartListening(EventName.ENEMY_KILL, UpdateEnemiesRemain);
    }

    private void UpdateEnemiesRemain(EventData arg0)
    {
        if (arg0.intData == 0)
        {
            enemiesRemain.text = "";
        }
        else
        {
            enemiesRemain.text = "Enemies: " + arg0.intData;
        }
    }

    private void OnExitOpen(EventData arg0)
    {
        ready = true;
        wavesReady = true;
        nextWave.text = "";
        timeLeft.text = "Exit available";
    }

    public void UpdateTimeLeft(int time)
    {
        if (!ready && time != lastCounter)
        {
            if (time == 0)
            {
                ready = true;

                EventManager.TriggerEvent(EventName.EXIT_OPEN);


            }
            else
            {
                lastCounter = time;
                timeLeft.text = "Time to exit: " + time;
            }
        }

    }
    public void UpdateWaveTimeLeft(int time, CharacterManager characterManager)
    {
        if (!wavesReady&& time != lastWaveCounter)
        {
            if (time == 0)
            {
                nextWave.text = "";
               
                if (!characterManager.HasSpawnEnemies())
                {
                    wavesReady = true;
                }
                else
                {
                    characterManager.SpawnNextWave();
                }
            }
            else
            {
                lastWaveCounter = time;
                nextWave.text = "Next wave: " + time +" waves: "+characterManager.GetRemainingWaves();
            }
        }

    }
    private void OnDestroy()
    {
        EventManager.StopListening(EventName.EXIT_OPEN, OnExitOpen);
        EventManager.StopListening(EventName.ENEMIES_REMAIN, UpdateEnemiesRemain);
        EventManager.StopListening(EventName.ENEMY_KILL, UpdateEnemiesRemain);
    }
}
