
using System.Collections.Generic;
using UnityEngine;


public class LevelJsonLoader : MonoBehaviour
{
    public bool isCreator = false;
    public int book, chapter, level;
    public int forceVariation = -1;
    private ObstacleIdentifier[] obstaclesResources;
    private CharacterEnemy[] enemiesResources;
    private CharacterEnemy[] enemies;
    public CharacterEnemy[] Enemies { get => enemies; }
    private CharacterMain characterMain;
    public CharacterMain CharacterMain { get => characterMain; }

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
            forceVariation = Random.Range(0, variations.Length);
        }
        else
        {
            var creator = FindFirstObjectByType<JsonMapCreator>();
            creator.book = book;
            creator.chapter = chapter;
            creator.level = level;
            creator.variation = forceVariation;
        }
        ParseString(variations[forceVariation].text);

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

    protected void ParseString(string tempString)
    {
        if (tempString != "")
        {
            var lvl = JsonUtility.FromJson<Level>(tempString);
            LoadMainCharacter(lvl.main);
            LoadMap(lvl.enemies, lvl.obstacles);
        }
    }

    private void LoadMainCharacter(int[] main)
    {
        characterMain = Instantiate<CharacterMain>(Resources.Load<CharacterMain>("Prefabs/InGame/Characters/CharacterMain" + main[0])).GetComponent<CharacterMain>();
        characterMain.transform.position = Vector3.right * main[1] + Vector3.forward * main[2];
        characterMain.enabled = false;
        var cam = FindObjectOfType<Camera>();
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
        for (int i = 0; i < obstaclesInfo.Length; i += 3)//0-x , 1-z, 2-id
        {
            ObstacleIdentifier prefab = GetObstacle((int)(obstaclesInfo[i + 2]));
            if (prefab)
            {
                Instantiate<ObstacleIdentifier>(prefab, (Vector3.right * obstaclesInfo[i] + Vector3.forward * obstaclesInfo[i + 1]), Quaternion.identity);

            }
        }
        for (int i = 0; i < charactersInfo.Length; i += 6)//0-x , 1-z, 2-id 3-level 4-team 5-behaviour
        {
            CharacterEnemy prefab = GetCharacter((int)(charactersInfo[i + 2]));
            if (prefab)
            {
                CharacterEnemy go = Instantiate<CharacterEnemy>(prefab, (Vector3.right * charactersInfo[i] + Vector3.forward * charactersInfo[i + 1]), Quaternion.identity);
                CharacterEnemy enemy = go.GetComponent<CharacterEnemy>();
                enemy.SetLevel(charactersInfo[i + 3]);
                enemy.team = charactersInfo[i + 4];
                enemy.behaviour = charactersInfo[i + 5];
                enemy.enabled = false;
                enemiesList.Add(enemy);
                if (isCreator)
                {
                    foreach (var item in enemy.GetComponentsInChildren<Collider>())
                    {
                        item.isTrigger = true;
                    }
                }
            }
        }
        enemies = enemiesList.ToArray();
    }

}
