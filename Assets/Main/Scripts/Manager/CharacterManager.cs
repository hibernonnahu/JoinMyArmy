
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public Character[] prefabs;
    private List<Character>[] teamList;
    private CharacterMain characterMain;
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
        characterMain=loader.CharacterMain;
        AddCharacterMain(characterMain);
        foreach (var enemy in loader.Enemies)
        {
            AddCharacterEnemy(enemy,characterMain);
        }
        CurrentPlaySingleton.GetInstance().LoadParty(characterMain);
        Destroy(loader);
    }

    internal List<Character> GetTeam(int v)
    {
        return teamList[v];
    }

    public List<Character> GetTeamMatesInRange(int attackTeam,float attackDistanceSqr, Vector3 position)
    {
        List<Character> teammatesList =teamList[ attackTeam];
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
        teamList[enemy.team].Add(enemy);
        enemy.CharacterMain = characterMain;
        enemy.enabled = true;
        return enemy;
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
                FindObjectOfType<ExitController>().OnOpen();
            }
        }
    }
    public void GoMainTeam(CharacterEnemy characterEnemy,bool direct=false,int forcePosition=-1)
    {
        teamList[0].Add(characterEnemy);
        characterEnemy.Revive();
        characterMain.recluitHandler.Recluit(characterEnemy,direct,forcePosition);
    }
}
