
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public Character[] prefabs;
    private List<Character>[] teamList;
    private List<List<Character>> waveList;
    private CharacterMain characterMain;
    public GameObject[] spawnPool;
    private int currentMarkIndex = 0;
    public float spawnTime = 2;
    private void Start()
    {

    }
    public void Init(int teamsAmount)
    {
        teamList = new List<Character>[teamsAmount];
        for (int i = 0; i < teamList.Length; i++)
        {
            teamList[i] = new List<Character>();
        }

        InitJson();
    }

    private void InitJson()
    {

        var loader = GetComponent<LevelJsonLoader>();
        loader.LoadMap();
        characterMain = loader.CharacterMain;
        AddCharacterMain(characterMain);
        foreach (var enemy in loader.Enemies)
        {
            AddCharacterEnemy(enemy, characterMain);
        }
        RemoveEmptyWaves();

        Destroy(loader);
    }

    private void RemoveEmptyWaves()
    {
        if (waveList != null)
            for (int i = waveList.Count - 1; i >= 0; i--)
            {
                if (waveList[i].Count == 0)
                {
                    waveList.RemoveAt(i);
                }
            }
    }

    internal bool HasSpawnEnemies()
    {
        return waveList != null && waveList.Count != 0;
    }

    internal List<Character> GetTeam(int v)
    {
        return teamList[v];
    }

    public List<Character> GetTeamMatesInRange(int attackTeam, float attackDistanceSqr, Vector3 position)
    {
        List<Character> teammatesList = teamList[attackTeam];
        List<Character> inRange = new List<Character>();
        foreach (var enemy in teammatesList)
        {

            if (attackDistanceSqr > (position - enemy.transform.position).sqrMagnitude)
            {
                inRange.Add(enemy);
            }
        }
        return inRange;
    }
    internal Character GetClosestEnemyInRange(int attackTeam, float attackDistanceSqr, Vector3 position)
    {
        Character current = null;
        float closest = float.MaxValue;
        List<Character> enemyList = GetEnemyListForTeam(attackTeam);
        foreach (var enemy in enemyList)
        {
            float dist = (position - enemy.transform.position).sqrMagnitude;
            if (dist < attackDistanceSqr && dist < closest)
            {
                closest = dist;
                current = enemy;
            }
        }
        return current;
    }

    internal int GetRemainingWaves()
    {
        return waveList.Count;
    }

    internal Character GetRandomEnemy(int attackTeam)
    {
        var list = GetEnemyListForTeam(attackTeam);
        if (list.Count == 0)
        {
            return null;
        }
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    private List<Character> GetEnemyListForTeam(int team)
    {
        return teamList[(team + 1) % 2];
    }

    private void AddCharacterMain(CharacterMain character)
    {
        character.CharacterManager = this;
        teamList[0].Add(character);
        character.enabled = true;
    }
    private CharacterEnemy AddCharacterEnemy(CharacterEnemy enemy, CharacterMain characterMain)
    {
        enemy.CharacterManager = this;
        enemy.CharacterMain = characterMain;
        if (enemy.belongToWave == 0)
        {
            teamList[enemy.team].Add(enemy);
            enemy.enabled = true;
        }
        else
        {
            AddEnemyForWave(enemy);
        }

        return enemy;
    }

    private void AddEnemyForWave(CharacterEnemy enemy)
    {
        if (waveList == null)
        {
            waveList = new List<List<Character>>();
        }
        enemy.belongToWave--;
        if (waveList.Count <= (enemy.belongToWave))
        {
            for (int i = 0; i <= enemy.belongToWave; i++)
            {
                if (waveList.Count <= i)
                {
                    waveList.Add(new List<Character>());
                }
            }
        }
        enemy.gameObject.SetActive(false);
        waveList[enemy.belongToWave].Add(enemy);
    }

    public void RemoveCharacter(Character character)
    {
        teamList[character.team].Remove(character);
        if (character.team == 0)
        {
            characterMain.recluitHandler.Remove(character);
        }
        else
        {

            if (teamList[character.team].Count == 0)
            {
                if ((waveList == null || waveList.Count == 0))
                {
                    EventManager.TriggerEvent(EventName.EXIT_OPEN);
                }
                else
                {
                    SpawnNextWave();
                }
            }
            EventManager.TriggerEvent(EventName.ENEMY_KILL, EventManager.Instance.GetEventData().SetInt(teamList[1].Count));
        }
    }

    public void SpawnNextWave()
    {
        if (waveList.Count > 0)
        {
            foreach (var character in waveList[0])
            {
                SpawnMarkAt(character.transform.position);
                character.transform.position = Vector3.right * character.transform.position.x + Vector3.forward * character.transform.position.z + Vector3.down * 1000;

                teamList[character.team].Add(character);
                character.Spawn(spawnTime);
            }
            waveList.RemoveAt(0);

            EventManager.TriggerEvent(EventName.ENEMIES_REMAIN, EventManager.Instance.GetEventData().SetInt(teamList[1].Count));
            if (waveList.Count > 0)
                EventManager.TriggerEvent(EventName.SPAWN_WAVE);
        }

    }

    private void SpawnMarkAt(Vector3 position)
    {
        var mark = spawnPool[currentMarkIndex];
        currentMarkIndex = (currentMarkIndex + 1) % spawnPool.Length;
        mark.transform.position = position;
        LeanTween.scale(mark, Vector3.one, 0.4f).setEaseInElastic();
        LeanTween.scale(mark, Vector3.zero, 0.4f).setDelay(spawnTime).setEaseOutElastic();
    }


    public void GoMainTeam(CharacterEnemy characterEnemy, bool direct = false, int forcePosition = -1)
    {
        teamList[0].Add(characterEnemy);
        characterEnemy.level = GetEnemyLevel(characterEnemy.id);
        characterEnemy.UpdateStatsOnLevel(characterEnemy.level, true, false);
        characterEnemy.Revive();
        characterMain.recluitHandler.Recluit(characterEnemy, direct, forcePosition);
    }

    private int GetEnemyLevel(int id)
    {
        return SaveData.GetInstance().GetEnemyLocalLevel(id);
    }
}
