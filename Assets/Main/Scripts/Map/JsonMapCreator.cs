using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System;


public class JsonMapCreator : MonoBehaviour
{
    JsonData mapJson;
    Level lvl;
    public string gameType;
    public int book = 1;
    public int chapter = 1;
    public int level = 1;
    public int variation = 1;
    public int floor = 0;
    public int time = -1;
    public int waveTime = -1;
    public int castleDefenseEnemy = -1;
    public int extraEnemyLevel = 0;
    public int coinsMultiplier = 10;
    public string storyJsonFileName = "";
    public GameObject[] wavesFolders;
    public int[] teamEnemiesID = new int[] { 1, 0 };
    // Use this for initialization
    private void Awake()
    {

        //PREPARE MAP
        lvl = new Level();

    }

    private void FindCharacters()
    {
        var mainCharacter = FindObjectOfType<CharacterMain>();

        lvl.main = new int[3];
        lvl.main[0] = mainCharacter.id;
        lvl.main[1] = (int)(Math.Floor(mainCharacter.transform.position.x));
        lvl.main[2] = (int)(Math.Floor(mainCharacter.transform.position.z));

        CharacterEnemy[] enemies = FindObjectsOfType<CharacterEnemy>();
        List<int> auxList = new List<int>();

        int coinsTotal = 0;
        foreach (var enemy in enemies)
        {
            auxList.Add((int)(enemy.transform.position.x));
            auxList.Add((int)(enemy.transform.position.z));

            auxList.Add(enemy.id);
            auxList.Add(enemy.level);
            auxList.Add(enemy.team);
            auxList.Add(enemy.behaviour);
            auxList.Add(enemy.extraAlertRange);
            auxList.Add(enemy.belongToWave);
            auxList.Add(0);
            auxList.Add(0);
            coinsTotal += (int)(enemy.GetCoins(extraEnemyLevel) * coinsMultiplier * 0.1f);
        }
        Debug.Log("Total Coins enemies "+coinsTotal);
       
        lvl.enemies = auxList.ToArray();

    }
    private void FindObstacles()
    {
        ObstacleIdentifier[] obstacles = FindObjectsOfType<ObstacleIdentifier>();
        List<int> auxList = new List<int>();


        foreach (var obstacle in obstacles)//0-x , 1-z, 2-id ,3-ratation , 4-size , 5-collider,6,7,8
        {
            auxList.Add((int)(obstacle.transform.position.x * 10));
            auxList.Add((int)(obstacle.transform.position.z * 10));

            auxList.Add(obstacle.id);
            obstacle.rotation = (int)(obstacle.transform.rotation.eulerAngles.y);
            auxList.Add(obstacle.rotation);
            obstacle.size = Mathf.RoundToInt(obstacle.transform.localScale.x * 10);
            auxList.Add(obstacle.size);
            auxList.Add(obstacle.colliders);
            auxList.Add(0);
            auxList.Add(0);
            auxList.Add(0);
        }
        lvl.obstacles = auxList.ToArray();

    }
    private void RemoveExtraDecimals()
    {
        foreach (var item in FindObjectsOfType<Transform>())
        {
            if (item.gameObject.tag != "UI" && item.transform.parent == null)
            {
                var pos = item.position;
                item.position = Vector3.right * ((Mathf.Round(pos.x * 10)) / 10) + Vector3.forward * ((Mathf.Round(pos.z * 10)) / 10);
            }
        }
    }

    public void SaveLocal()
    {
        CheckWavesFolder();
        RemoveExtraDecimals();
        FindCharacters();
        FindObstacles();
        lvl.teamEnemiesID = teamEnemiesID;
        lvl.floor = floor;
        lvl.time = time;
        lvl.waveTime = waveTime;
        lvl.extraEnemyLevel = extraEnemyLevel;
        lvl.coinsMultiplier = coinsMultiplier;
        lvl.storyJsonFileName = storyJsonFileName;
        lvl.castleDefenseEnemy = castleDefenseEnemy;
        mapJson = JsonMapper.ToJson(lvl);

        string fullPath = Application.dataPath + "/Resources/Maps/" + gameType + "/Book" + book + "/Chapter" + chapter + "/Level" + level + "/" + variation + ".json";

        Debug.Log(fullPath);

        StreamWriter writer = new StreamWriter(fullPath, false);
        Debug.Log(mapJson.ToString());

        writer.Write(mapJson.ToString());
        writer.Close();
    }

    private void CheckWavesFolder()
    {
        for (int i = 0; i < wavesFolders.Length; i++)
        {
            wavesFolders[i].SetActive(true);
            foreach (Transform t in wavesFolders[i].transform)
            {
                CharacterEnemy ce = t.GetComponent<CharacterEnemy>();
                if (ce != null)
                {
                    ce.belongToWave = i;
                }
            }
        }
    }
}
