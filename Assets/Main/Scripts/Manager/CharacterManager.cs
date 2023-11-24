
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public Character[] prefabs;
    private List<Character>[] teamList;
    private List<Character>[] teamExtraList;
    private List<List<Character>> waveList;
    private CharacterMain characterMain;
    public GameObject[] spawnPool;
    private int currentMarkIndex = 0;
    public float spawnTime = 2;
    private LevelJsonLoader loader;
    public LevelJsonLoader Loader { get { return loader; } }
    public List<int>[] teamEnemiesID;
    private int currentWave = 1;
    public int CurrentWave { get { return currentWave; } }

    private void Start()
    {

    }
    public void Init()
    {
        loader = GetComponent<LevelJsonLoader>();
        loader.LoadMap();
        int teamsAmount = loader.Lvl.teamEnemiesID.Length;
        ParseTeamEnemiesIDs(loader.Lvl.teamEnemiesID);
        teamList = new List<Character>[teamsAmount];
        for (int i = 0; i < teamList.Length; i++)
        {
            teamList[i] = new List<Character>();
        }
        teamExtraList = new List<Character>[teamsAmount];
        for (int i = 0; i < teamExtraList.Length; i++)
        {
            teamExtraList[i] = new List<Character>();
        }
        loader.EnableCastleDefense();
        InitJson();
    }

    private void ParseTeamEnemiesIDs(int[] teamEnemiesID)
    {
        this.teamEnemiesID = new List<int>[teamEnemiesID.Length];
        for (int i = 0; i < teamEnemiesID.Length; i++)
        {
            this.teamEnemiesID[i] = new List<int>();
            int digits = CustomMath.CountDigits(teamEnemiesID[i]);
            int code = teamEnemiesID[i];
            for (int j = 0; j < digits; j++)
            {
                this.teamEnemiesID[i].Add(code % 10);
                code = code / 10;
            }
        }
    }

    internal void KillExtraTeam(int v)
    {
        foreach (var item in GetEnemyExtraListForTeam(v))
        {
            CharacterEnemy e = item as CharacterEnemy;
            e.Kill();
        }
        foreach (var item in GetEnemyListForTeam(v))
        {
            CharacterEnemy e = item as CharacterEnemy;
            e.Kill();
        }
    }

    internal void ChangeTeam(CharacterStory character, int v)
    {
        teamList[character.characterEnemy.team].Remove(character.characterEnemy);
        teamList[v].Add(character.characterEnemy);
    }

    private void InitJson()
    {
        characterMain = loader.CharacterMain;
        AddCharacterMain(characterMain);
        foreach (var enemy in loader.Enemies)
        {
            AddCharacterEnemy(enemy, characterMain);
        }
        RemoveEmptyWaves();


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

    public List<Character> GetEnemiesInRange(int attackTeam, float attackDistanceSqr, Vector3 position)
    {
        List<Character> inRange = new List<Character>();
        List<int> attackers = new List<int>();
        for (int i = 0; i < teamEnemiesID.Length; i++)
        {
            if (i != attackTeam)
            {
                foreach (var item in teamEnemiesID[i])
                {
                    if (item == attackTeam)
                    {
                        attackers.Add(i);
                        break;
                    }
                }
            }
            else
            {
                foreach (var item in teamEnemiesID[i])
                {
                    attackers.Add(item);
                }
            }
        }
        foreach (var enemyID in attackers)
        {
            List<Character> enemiesList = teamList[enemyID];

            foreach (var enemy in enemiesList)
            {

                if (!enemy.invulnerable && attackDistanceSqr > (position - enemy.transform.position).sqrMagnitude)
                {
                    inRange.Add(enemy);
                }
            }
        }

        foreach (var enemy in GetEnemyExtraListForTeam(attackTeam))
        {
            if (!enemy.invulnerable && attackDistanceSqr > (position - enemy.transform.position).sqrMagnitude)
            {
                inRange.Add(enemy);
            }
        }
        return inRange;
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
            if (!enemy.invulnerable && dist < attackDistanceSqr && dist < closest)
            {
                closest = dist;
                current = enemy;
            }
        }
        enemyList = GetEnemyExtraListForTeam(attackTeam);
        foreach (var enemy in enemyList)
        {
            float dist = (position - enemy.transform.position).sqrMagnitude;
            if (!enemy.invulnerable && dist < attackDistanceSqr && dist < closest)
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
        List<Character> list = new List<Character>();
        foreach (var item in teamEnemiesID[team])
        {
            list.AddRange(teamList[item]);
        }
        return list;
    }
    private List<Character> GetEnemyExtraListForTeam(int team)
    {
        List<Character> list = new List<Character>();
        foreach (var item in teamEnemiesID[team])
        {
            list.AddRange(teamExtraList[item]);
        }
        return list;
    }

    private void AddCharacterMain(CharacterMain character)
    {
        character.CharacterManager = this;
        teamList[0].Add(character);
        character.enabled = true;
    }
    public CharacterEnemy AddCharacterEnemy(CharacterEnemy enemy, CharacterMain characterMain)
    {
        enemy.CharacterManager = this;
        enemy.CharacterMain = characterMain;


        {
            if (enemy.belongToWave == 0)
            {
                AddCharacterToList(enemy);
                enemy.enabled = true;
            }
            else
            {
                AddEnemyForWave(enemy);
            }
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
        if (character.extra)
        {
            teamExtraList[character.team].Remove(character);
        }

        teamList[character.team].Remove(character);

        if (character.team == 0)
        {
            characterMain.recluitController.Remove(character);
        }
        else
        {

            if (NoEnemiesLeft())
            {
                if ((waveList == null || waveList.Count == 0))
                {
                    if (loader.Lvl.time == -1)
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

    private bool NoEnemiesLeft()
    {
        foreach (var item in teamEnemiesID[0])
        {
            if (teamList[item].Count != 0)
            {
                return false;
            }
        }
       return true;
    }

    public void SpawnNextWave()
    {
        if (waveList.Count > 0)
        {
            foreach (var character in waveList[0])
            {
                SpawnMarkAt(character.transform.position);
                character.transform.position = Vector3.right * character.transform.position.x + Vector3.forward * character.transform.position.z + Vector3.down * 1000;

                AddCharacterToList(character);

                character.Spawn(spawnTime);
            }
            waveList.RemoveAt(0);
            currentWave++;
            EventManager.TriggerEvent(EventName.NEXT_WAVE + CurrentWave);
            EventManager.TriggerEvent(EventName.ENEMIES_REMAIN, EventManager.Instance.GetEventData().SetInt(teamList[1].Count));
            if (waveList.Count > 0)
                EventManager.TriggerEvent(EventName.SPAWN_WAVE);
        }

    }

    private void AddCharacterToList(Character character)
    {
        if (character.extra)
        {
            teamExtraList[character.team].Add(character);
        }
        else
        {
            teamList[character.team].Add(character);
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
        SaveData.GetInstance().Save(SaveDataKey.RECLUIT + characterEnemy.id, 1);
        characterEnemy.level = GetEnemyLevel(characterEnemy.id);
        characterEnemy.UpdateStatsOnLevel(characterEnemy.level, true, false);
        characterEnemy.Revive();
        characterEnemy.SetLayer(16, 15, new int[] { 9 });
        characterMain.recluitController.Recluit(characterEnemy, direct, forcePosition);
    }

    private int GetEnemyLevel(int id)
    {
        return SaveData.GetInstance().GetEnemyLocalLevel(id);
    }
}
