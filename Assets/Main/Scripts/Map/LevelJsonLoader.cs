﻿
using System.Collections.Generic;
using UnityEngine;


public class LevelJsonLoader : MonoBehaviour
{
    public float multiplier = 1;
    public bool isCreator = false;
    public int book, chapter, level;
    public int forceVariation = -1;
    public MeshRenderer floor;
    private ObstacleIdentifier[] obstaclesResources;
    private CharacterEnemy[] enemiesResources;
    private CharacterEnemy[] enemies;
    public CharacterEnemy[] Enemies { get => enemies; }
    private CharacterMain characterMain;
    public CharacterMain CharacterMain { get => characterMain; }
    private Level lvl;
    public Level Lvl { get { return lvl; } }

    public void Awake()
    {
        var characters = Resources.LoadAll<CharacterEnemy>("Prefabs/InGame/Enemies");
        enemiesResources = new CharacterEnemy[characters.Length];
        foreach (var item in characters)
        {
            enemiesResources[item.id] = item;
        }

        var obstacles = Resources.LoadAll<ObstacleIdentifier>("Prefabs/InGame/Obstacles");
        obstaclesResources = new ObstacleIdentifier[obstacles.Length];
        foreach (var item in obstacles)
        {
            obstaclesResources[item.id] = item;
        }
    }

    public void LoadMap()
    {
        DeleteAll();
        var variations = Resources.LoadAll<TextAsset>("Maps/Campaign/Book" + book + "/Chapter" + chapter + "/Level" + level);
        if (forceVariation == -1)
        {
            lvl= ParseString(variations[Random.Range(0, variations.Length)].text);
        }
        else
        {
            var creator = FindFirstObjectByType<JsonMapCreator>();
            creator.book = book;
            creator.chapter = chapter;
            creator.level = level;
            creator.variation = forceVariation;
            lvl = ParseString(variations[forceVariation].text);
            creator.floor = lvl.floor;
            creator.time = lvl.time;
            creator.waveTime = lvl.waveTime;
            creator.storyJsonFileName = lvl.storyJsonFileName;
            creator.teamEnemiesID = lvl.teamEnemiesID;
        }


    }

    private void DeleteAll()
    {
        foreach (var item in FindObjectsOfType<Character>())
        {
            Destroy(item.gameObject);
        }
        foreach (var item in FindObjectsOfType<ObstacleIdentifier>())
        {
            Destroy(item.gameObject);
        }
    }

    protected Level ParseString(string tempString)
    {
        if (tempString != "")
        {
            var lvl = JsonUtility.FromJson<Level>(tempString);
            floor.material = Resources.Load<Material>("Prefabs/InGame/FloorMaterial/" + lvl.floor);
            LoadMainCharacter(lvl.main);
            var game = GetComponent<Game>();
            if (game != null)
            {
                if(lvl.storyJsonFileName != "")
                {
                    var sm = gameObject.AddComponent<StoryManager>();
                    sm.SetJsonCode(lvl.storyJsonFileName);
                }
                game.time = lvl.time;
                game.waveTime = lvl.waveTime;
                //AddCompanion("RedHairGril", Vector3.right * 3);
                //AddCompanion("Succubus", Vector3.left * 3);
            }

            LoadMap(lvl.enemies, lvl.obstacles);
            return lvl;
        }
        return null;
    }

    private void AddCompanion(string name, Vector3 position)
    {
        var go = Instantiate<CharacterEnemy>(Resources.Load<CharacterEnemy>("Prefabs/InGame/Characters/" + name));
        go.transform.position = characterMain.transform.position + position;
    }

    private void LoadMainCharacter(int[] main)
    {
        characterMain = Instantiate<CharacterMain>(Resources.Load<CharacterMain>("Prefabs/InGame/Characters/CharacterMain" + main[0])).GetComponent<CharacterMain>();
        characterMain.transform.position = Vector3.right * main[1] + Vector3.forward * main[2];
        characterMain.enabled = false;
        var cam = FindObjectOfType<CameraHandler>();
        cam.transform.position = characterMain.transform.position.x * Vector3.right + characterMain.transform.position.z * Vector3.forward + Vector3.up * cam.transform.position.y;
        if (isCreator)
        {
            characterMain.transform.Find("Canvas").gameObject.SetActive(false);
            characterMain.GetComponentInChildren<Collider>().isTrigger = true;
        }
    }

    public CharacterEnemy GetCharacter(int index)
    {
        return enemiesResources[index];
    }
    ObstacleIdentifier GetObstacle(int index)
    {
        return obstaclesResources[index];
    }

    private void LoadMap(int[] charactersInfo, int[] obstaclesInfo)
    {
        List<CharacterEnemy> enemiesList = new List<CharacterEnemy>();
        for (int i = 0; i < obstaclesInfo.Length; i += 9)//0-x , 1-z, 2-id ,3-ratation , 4-size , 5-collider,6,7,8
        {
            ObstacleIdentifier prefab = GetObstacle((int)(obstaclesInfo[i + 2]));
            if (prefab)
            {
                var obstacle=Instantiate<ObstacleIdentifier>(prefab, (Vector3.right * obstaclesInfo[i]*multiplier + Vector3.forward * obstaclesInfo[i + 1]*multiplier), Quaternion.identity);
                obstacle.rotation = obstaclesInfo[i + 3];
                obstacle.size = obstaclesInfo[i + 4];
                obstacle.colliders = obstaclesInfo[i + 5];
            }
        }
        for (int i = 0; i < charactersInfo.Length; i += 10)//0-x , 1-z, 2-id 3-level 4-team 5-behaviour 6-extraAlertRange 7-belongsToWave
        {
            CharacterEnemy prefab = GetCharacter((int)(charactersInfo[i + 2]));
            if (prefab)
            {
                CharacterEnemy go = Instantiate<CharacterEnemy>(prefab, (Vector3.right * charactersInfo[i] + Vector3.forward * charactersInfo[i + 1]), Quaternion.identity);
                CharacterEnemy enemy = go.GetComponent<CharacterEnemy>();
                enemy.SetLevel(charactersInfo[i + 3]);
                enemy.team = charactersInfo[i + 4];
                enemy.behaviour = charactersInfo[i + 5];
                enemy.extraAlertRange = charactersInfo[i + 6];
                enemy.belongToWave = charactersInfo[i + 7];
                enemy.enabled = false;
                enemiesList.Add(enemy);
                if (isCreator)
                {
                    AddToParent(enemy);
                    foreach (var item in enemy.GetComponentsInChildren<Collider>())
                    {
                        item.isTrigger = true;
                    }
                }

            }
        }
        enemies = enemiesList.ToArray();
    }

    private void AddToParent(CharacterEnemy enemy)
    {
        var folder = GameObject.Find("wave " + enemy.belongToWave);
        if (folder == null)
        {
            folder = new GameObject("wave " + enemy.belongToWave);
        }
        enemy.transform.SetParent(folder.transform);
    }
}
